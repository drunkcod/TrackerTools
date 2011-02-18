#r "Build\TrackerTools.dll"

open System
open System.IO
open System.Xml.Serialization
open System.Text.RegularExpressions
open System.Drawing
open System.Text
open TrackerTools

let ProjectId = fsi.CommandLineArgs.[1]

let dateFromPath = 
    let r = Regex(@"\\(\d\d\d\d-\d\d-\d\d)\\")
    fun x -> DateTime.Parse(r.Match(x).Groups.[1].Value)

let readSnapshot = 
    let serializer = new XmlSerializer(typeof<TrackerStories>)
    fun path ->
        use reader = File.OpenText(path)
        serializer.Deserialize(reader) :?> TrackerStories

let computeStatistics (snapshot:TrackerStories) =
    snapshot.Items |> Seq.countBy (fun x -> x.CurrentState)

let states = [|"accepted" ; "delivered"; "finished"  ; "started"   ; "unstarted"  ; "unscheduled" |]
let colors = [|Color.Green; Color.Blue ; Color.Yellow; Color.Orange; Color.Purple ; Color.Gray |]

type IChartDataEncoding =
    abstract member Encode : data:seq<int> -> string

type GoogleExtendedEncoding(min, max) =
    [<Literal>] 
    let Alphabet = "ABCDEFGHIJKLMNOPQSRTUVWXYZabcdefghijklmonpqrstuvwxyz0123456789.-"
    [<Literal>] 
    let MaxValue = 4095
    
    interface IChartDataEncoding with
        member x.Encode data =
            let result = StringBuilder()
            data |> Seq.iter (fun value ->                
                let d,r = Math.DivRem(x.Scale value, Alphabet.Length)
                result.Append([|Alphabet.[d]; Alphabet.[r]|]) |> ignore)
            result.ToString()

    member x.Scale value = 
        if value <= max && value >= min then
            (value - min) * MaxValue / (max - min)
        else 
            Console.WriteLine(x.ToString())
            raise(new ArgumentOutOfRangeException(x.ToString()))

type Axis = X | Y | Top | Left

type ChartMarker = 
    | FillToBottom of Color * int
    | FillBetween of Color * int * int

type ChartAxis = { 
    Axis: Axis
    Range : int * int
    Labels : string seq 
    Positions : int seq }

type ChartSeries = {
    Name : string 
    Color : Color
    Data : int seq }

type LineChart = {
    Width : int
    Height : int 
    MinValue : int
    MaxValue : int
    Axes : ChartAxis seq 
    Series : ChartSeries seq 
    Markers : ChartMarker seq } with
    override x.ToString() =
        let axisToString = function
            | X -> "x"
            | Y -> "y"
            | Top -> "t"
            | Left -> "r"

        let result = StringBuilder("http://chart.apis.google.com/chart?cht=lc")
        let hex (c:Color) = 
            if c.A = 255uy then
                String.Format("{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B)
            else String.Format("{0:x2}{1:x2}{2:x2}{3:x2}", c.R, c.G, c.B, c.A)

        result.AppendFormat("&chs={0}x{1}", x.Width, x.Height) |> ignore

        let format = ref "&chxt={0}"
        x.Axes |> Seq.iter (fun axis -> 
            result.AppendFormat(!format, axisToString axis.Axis) |> ignore
            format := ",{0}")

        format := "&chxr={0},{1},{2}"
        x.Axes |> Seq.iteri (fun n axis ->
            match axis.Range with
            | (0, 100) -> ()
            | (min, max) ->
                result.AppendFormat(!format, n, min, max) |> ignore
                format := "|{0},{1},{2}")

        format := "&chxl={0}:"
        x.Axes |> Seq.iteri (fun n axis -> 
            if not(Seq.isEmpty axis.Labels) then 
                result.AppendFormat(!format, n) |> ignore
                axis.Labels |> Seq.iter (fun x -> result.AppendFormat("|{0}", x) |> ignore)
                format := "|{0}:")

        format := "&chxp={0}"
        x.Axes |> Seq.iteri (fun n axis -> 
            if not(Seq.isEmpty axis.Positions) then 
                result.AppendFormat(!format, n) |> ignore
                axis.Positions |> Seq.iter (fun x -> result.AppendFormat(",{0}", x) |> ignore)
                format := "|{0}")

        let sep = ref "&chm="
        x.Markers |> Seq.iter (fun marker ->
            match marker with
            | FillToBottom(color, series) -> result.AppendFormat("{0}B,{1},{2},0,0", !sep, hex color, series)
            | FillBetween(color, startSeries, endSeries) -> result.AppendFormat("{0}b,{1},{2},{3},0", !sep, hex color, startSeries, endSeries)
            |> ignore
            sep := "|")

        let format = ref "&chdl={0}"
        x.Series |> Seq.iter (fun series ->
            result.AppendFormat(!format, series.Name) |> ignore
            format := "|{0}")

        let format = ref "&chco={0}"
        x.Series |> Seq.iter (fun series ->
            result.AppendFormat(!format, hex series.Color) |> ignore
            format := ",{0}")

        let all = x.Series |> Seq.collect (fun x -> x.Data)
        let encoding = GoogleExtendedEncoding(x.MinValue, x.MaxValue) :> IChartDataEncoding            

        let format = ref "&chd=e:{0}"
        x.Series |> Seq.iter (fun series -> 
            result.AppendFormat(!format, encoding.Encode(series.Data)) |> ignore
            format := ",{0}")
            
        result.ToString()

let snapshots = 
    Directory.GetFiles(@"R:\PivotalSnapshots", "*.xml", SearchOption.AllDirectories)
    |> Seq.filter (fun x -> Path.GetFileNameWithoutExtension(x) = ProjectId)
    |> Seq.map (fun x -> dateFromPath x, (readSnapshot >> computeStatistics) x)

let month = function
    | 1 -> "Jan" | 2 -> "Feb" | 3 -> "Mar" | 4 -> "Apr" | 5 -> "May" | 6 -> "Jun"
    | 7 -> "Jul" | 8 -> "Aug" | 9 -> "Sep" | 10 -> "Oct" | 11 -> "Nov" | 12 -> "Dec"
    | _ -> raise(new ArgumentOutOfRangeException())

snapshots 
|> Seq.sortBy fst
|> Seq.collect (fun (key, xs) ->
    let count =
        let map = Map(xs)
        fun x -> 
            match Map.tryFind x map with
            | None -> 0
            | Some(x) -> x
    states |> Seq.map (fun x -> (x, count x)))
|> Seq.groupBy fst
|> Seq.map (fun (key, xs) -> key, Seq.map snd xs)
|> fun series -> 
    let data = Map(series)

    states |> Seq.map (fun state -> state, data.[state])
    |> Seq.scan (fun (_, prev) (key, curr) ->
        (key, Seq.map2 (+) prev curr))
        ("zero", Seq.initInfinite (fun _ -> 0))
    |> Seq.skip 1
|> fun series ->

    let all = series |> Seq.collect snd |> Seq.cache

    let min = Seq.min all
    let max = Seq.max all

    let data =
        let lut = Map(series)
        fun x -> lut.[x]

    let yAxis = {
        Axis = Axis.Y 
        Range = 0, max
        Labels = []
        Positions = [] }

    let xAxis = {
        Axis = Axis.X
        Range = 0,150
        Labels = snapshots |> Seq.map (fun (date, _) -> date.Day.ToString()) 
        Positions = [] }

    let months = snapshots |> Seq.mapi (fun n (x,_) -> x.Month, n) |> Seq.distinctBy fst

    let x2Axis = {
        Axis = Axis.X
        Range = 0, Seq.length snapshots 
        Labels = months |> Seq.map (fst >> month)
        Positions = months |> Seq.map snd }
    
    let translucent (x:Color) = Color.FromArgb(192, x)

    let chart = { 
        Width = 400
        Height = 650
        MinValue = min
        MaxValue = max
        Axes = [yAxis; xAxis; x2Axis]
        Series = 
            [
                { 
                    Name = "Accepted"
                    Color = colors.[0]
                    Data = data "accepted"
                }
                { 
                    Name = "Delivered"
                    Color = colors.[1] 
                    Data = data "delivered"
                }
                { 
                    Name = "Finished"
                    Color = colors.[2] 
                    Data = data "finished"
                }
                { 
                    Name = "Started"
                    Color = colors.[3]
                    Data = data "started"
                }
                {
                    Name = "Unstarted"
                    Color = colors.[4] 
                    Data = data "unstarted"
                }    
                {
                    Name = "Unscheduled"
                    Color = colors.[5]
                    Data = data "unscheduled"
                }
            ]
        Markers = 
            [
                FillToBottom(translucent colors.[0], 0)
                FillBetween(translucent colors.[1], 1, 0)
                FillBetween(translucent colors.[2], 2, 1)
                FillBetween(translucent colors.[3], 3, 2)
                FillBetween(translucent colors.[4], 4, 3)
                FillBetween(translucent colors.[5], 5, 4)
            ]
    }

    Console.WriteLine("{{html}}<img src='{0}'/>{{html}}", chart)
