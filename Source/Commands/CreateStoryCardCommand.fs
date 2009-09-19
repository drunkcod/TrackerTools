namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("CreateStoryCard")>]
type CreateStoryCardCommand(tracker:TrackerApi, configuration:TrackerToolsConfiguration, [<FromCommandLine(Position = 1)>] storyId:int) =

    let WriteStoryCard(story:TrackerStory) =
        let ProcessTemplate (story:TrackerStory) =
            DataBinder.Bind(configuration.StoryTemplate.GetTemplate(),  story).Replace("\n", "<br>")
        File.WriteAllText(String.Format("{0}.html", story.Id), ProcessTemplate story)

    interface ITrackerToolsCommand with
        member this.Invoke() = tracker.GetStory(configuration.ProjectId, storyId) |> WriteStoryCard