namespace TrackerTools
open System
open System.IO
open System.Net
open System.Xml
open System.Xml.Serialization
open System.Text
 
module Program =
    let StoryTemplate = File.ReadAllText("StoryTemplate.html")
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    let FromXml<'a> (stream:Stream) =
        use reader = new StreamReader(stream)
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(reader) :?> 'a

    let TakeSnapshot (tracker:Tracker) =
        let SaveSnapshot (stream:Stream) = 
            use reader = new StreamReader(stream)
            let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
            File.WriteAllText(targetPath, reader.ReadToEnd())
        tracker.GetStories Configuration.ProjectId SaveSnapshot
    
    let CreateStoryCard (storyId:int) (tracker:Tracker) =
        let FormatDescription (story:TrackerStory) = story.Description.Replace("\n", "<br>")
        let ProcessTemplate (story:TrackerStory) =
            StoryTemplate.Replace("$(Name)", story.Name).Replace("$(Description)",FormatDescription story )
        let story = tracker.GetStory Configuration.ProjectId storyId FromXml<TrackerStory>
        File.WriteAllText(String.Format("{0}.html", storyId), ProcessTemplate story)

    let DumpToConsole (stream:Stream) =
        use reader = new StreamReader(stream)
        reader.ReadToEnd() |> Console.WriteLine

    let ShowTasks (storyId:int) (tracker:Tracker) =
        tracker.GetTasks Configuration.ProjectId storyId DumpToConsole
    
    let WriteFragment (stream:Stream) (x:obj) =
        use writer = 
            XmlTextWriter.Create(stream, 
                XmlWriterSettings(OmitXmlDeclaration = true, Encoding = Encoding.UTF8, CloseOutput = true))
        let serializer = XmlSerializer(x.GetType())
        let ns = XmlSerializerNamespaces()
        ns.Add("", "")
        serializer.Serialize(writer, x, ns)      
    
    let XmlResult x (request:HttpWebRequest) =
        request.ContentType <- "text/xml"
        WriteFragment (request.GetRequestStream()) x
    
    let AddTask (storyId:int) (description:string) (tracker:Tracker) =
        tracker.AddTask Configuration.ProjectId storyId 
            (XmlResult(TrackerTask(Description = description)))
            DumpToConsole
    
    let ShowHelp x = ()

    [<EntryPoint>]
    let main args =
        try
            Tracker(Configuration.ApiToken)
            |> match args.[0] with
                | "TakeSnaphot" -> TakeSnapshot
                | "CreateStoryCard" -> CreateStoryCard (Int32.Parse(args.[1]))
                | "ShowTasks" -> ShowTasks (Int32.Parse(args.[1]))
                | "AddTask" -> AddTask (Int32.Parse(args.[1])) (args.[2])
                | _ -> ShowHelp
        with :? WebException as e ->
            Console.WriteLine e.Message       
            use reader = new StreamReader(e.Response.GetResponseStream())
            reader.ReadToEnd() |> Console.WriteLine                    
        0