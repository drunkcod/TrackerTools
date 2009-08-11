namespace TrackerTools
open System
open System.IO
open System.Xml.Serialization
 
module Program =
    let StoryTemplate = File.ReadAllText("StoryTemplate.html")
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    let FromXml<'a> (stream:Stream) =
        use reader = new StreamReader(stream)
        let serializer = XmlSerializer(typeof<'a>)
        serializer.Deserialize(reader) :?> 'a
    
    let SaveSnapshot (stream:Stream) = 
        use reader = new StreamReader(stream)
        let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
        File.WriteAllText(targetPath, reader.ReadToEnd())
       
    [<EntryPoint>]
    let main array =
        let tracker = Tracker(Configuration.ApiToken)
        let story = tracker.GetStory Configuration.ProjectId 989680 FromXml<TrackerStory>
        File.WriteAllText("Output.html", StoryTemplate.Replace("$(Name)", story.Name).Replace("$(Description)", story.Description.Replace("\n", "<br>")))
        //tracker.GetStories Configuration.ProjectId SaveSnapshot
        0