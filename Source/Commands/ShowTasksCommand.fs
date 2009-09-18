namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("ShowTasks")>]
type ShowTasksCommand(tracker:TrackerApi, configuration:TrackerToolsConfiguration, [<FromCommandLine(Position = 1)>] storyId:int) =
    
    let DumpToConsole (stream:Stream) =
        use reader = new StreamReader(stream)
        reader.ReadToEnd() |> Console.WriteLine

    interface ITrackerToolsCommand with
        member this.Invoke() = tracker.Base.GetTasks configuration.ProjectId storyId DumpToConsole