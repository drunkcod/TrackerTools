namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("TakeSnapshot")>]
type TakeSnapshotCommand(tracker:TrackerApi, configuration:TrackerToolsConfiguration) =
    let SaveSnapshot targetPath (stream:Stream) = 
        use reader = new StreamReader(stream)
        File.WriteAllText(targetPath, reader.ReadToEnd())
    interface ITrackerToolsCommand with
        member this.Invoke() = 
            let targetPath = Path.Combine(configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".xml")
            tracker.Base.GetStories configuration.ProjectId (SaveSnapshot targetPath)