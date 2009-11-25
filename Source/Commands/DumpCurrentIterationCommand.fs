namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("DumpCurrentIteration")>]
type DumpCurrentIteration(tracker:TrackerApi, configuration:TrackerToolsConfiguration) =
    interface ITrackerToolsCommand with
        member this.Invoke() =
            let WriteStoryCard(story:TrackerStory) = story.WriteStoryCard(configuration.StoryTemplate.GetTemplate())
            tracker.GetIteration(configuration.ProjectId, "current")
            |> Seq.collect (fun x -> x.Stories)
            |> Seq.iter WriteStoryCard