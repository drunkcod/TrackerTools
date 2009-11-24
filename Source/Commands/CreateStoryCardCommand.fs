namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("CreateStoryCard")>]
type CreateStoryCardCommand(tracker:TrackerApi, configuration:TrackerToolsConfiguration, [<FromCommandLine(Position = 1)>] storyId:int) =
    let WriteStoryCard(story:TrackerStory) = story.WriteStoryCard(configuration.StoryTemplate.GetTemplate())
    interface ITrackerToolsCommand with
        member this.Invoke() = tracker.GetStory(configuration.ProjectId, storyId) |> WriteStoryCard