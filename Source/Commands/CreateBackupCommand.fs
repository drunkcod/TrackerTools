namespace TrackerTools.Commands
open System
open System.IO
open TrackerTools

[<CommandName("CreateBackup")>]
type CreateBackupCommand(tracker:TrackerApi, configuration:TrackerToolsConfiguration) =
    let SaveSnapshot targetPath (stream:Stream) = 
        use reader = new StreamReader(stream)
        File.WriteAllText(targetPath, reader.ReadToEnd())
    interface ITrackerToolsCommand with
        member this.Invoke() =
            let target = Path.Combine(configuration.OutputDirectory, DateTime.Today.ToString("yyyy-MM-dd"))
            Directory.CreateDirectory(target) |> ignore
            tracker.GetProjects().Projects
            |> Seq.iter (fun x -> tracker.Base.GetStories x.Id (SaveSnapshot (Path.Combine(target, x.Name + ".xml"))))