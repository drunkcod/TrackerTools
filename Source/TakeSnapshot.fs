namespace TrackerTools
open System
open System.IO
                    
module Program =   
    let Configuration = TrackerToolsConfiguration.FromAppConfig()
    
    let SaveSnapshot (stream:Stream) = 
        use reader = new StreamReader(stream)
        let targetPath = Path.Combine(Configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
        File.WriteAllText(targetPath, reader.ReadToEnd())
       
    [<EntryPoint>]
    let main array =
        let tracker = Tracker(Configuration.ApiToken)
        tracker.GetStories Configuration.ProjectId SaveSnapshot
        0