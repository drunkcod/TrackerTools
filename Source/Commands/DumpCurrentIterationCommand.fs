namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("DumpCurrentIteration")>]
type DumpCurrentIteration(tracker:TrackerApi, configuration:TrackerToolsConfiguration) =
    let WriteStoryCard(story:TrackerStory) =
        let ProcessTemplate (story:TrackerStory) =
            DataBinder.Bind(configuration.StoryTemplate.GetTemplate(), story).Replace("\n", "<br>")
        File.WriteAllText(String.Format("{0}.html", story.Id), ProcessTemplate story)
        
    interface ITrackerToolsCommand with
        member this.Invoke() = 
            tracker.GetIteration(configuration.ProjectId, "current")        
            |> Seq.collect (fun x -> x.Stories)
            |> Seq.iter WriteStoryCard