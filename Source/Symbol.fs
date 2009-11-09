namespace TrackerTools
open System
open System.Collections.Generic

type Symbol(name:string, value:obj) =
    let aliases = HashSet<string>()
    do aliases.Add name |> ignore

    member this.Replace (s:string) transform = 
        aliases 
        |> Seq.fold (fun (s:string) x -> s.Replace(String.Format("$({0})", x), transform this)) s
    
    member this.AddAlias alias = aliases.Add(alias) |> ignore
    member this.IsKnownAs name = aliases.Contains(name)
    
    override this.ToString() = 
        match value with
        | null -> String.Empty
        | x -> x.ToString()