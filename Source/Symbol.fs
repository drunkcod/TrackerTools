namespace TrackerTools
open System

type Symbol(name:string, value:obj) =
    member this.Replace (s:string) = s.Replace(String.Format("$({0})", name), this.ToString())
    
    override this.ToString() = 
        match value with
        | null -> String.Empty
        | x -> x.ToString()