namespace TrackerTools.Commands
open System.IO
open TrackerTools

[<CommandName("CreateStoryCard")>]
type CreatestoryCardCommand(tracker:TrackerApi, configuration:TrackerToolsConfiguration, [<FromCommandLine(Position = 1)>] storyId) =
    
    let WriteStoryCard(story:TrackerStory) =
        let ProcessTemplate (story:TrackerStory) =
            DataBinder.Bind(StoryTemplate(), story).Replace("\n", "<br>")
        File.WriteAllText(String.Format("{0}.html", story.Id), ProcessTemplate story)

    interface ITrackerToolsCommand with
        member this.Invoke() = tracker.GetStory(configuration.ProjectId, storyId) |> WriteStoryCard