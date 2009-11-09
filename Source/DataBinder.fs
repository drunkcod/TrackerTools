namespace TrackerTools
open System.Reflection
open System.Xml.Serialization
     
module DataBinder =
    let Bind(format:string, obj:obj, transform) =
        let getAliases (mi:MemberInfo) = seq { 
            for x in mi.GetCustomAttributes(typeof<XmlElementAttribute>, false) do 
                yield (x :?> XmlElementAttribute).ElementName} 
           
        let makeSymbols (mi:MemberInfo) value = 
            let sym = Symbol(mi.Name, value)
            getAliases mi |> Seq.iter sym.AddAlias
            sym
            
        let itemType = obj.GetType()        
        let properties = itemType.GetProperties() |> Seq.map (fun prop -> makeSymbols prop (prop.GetValue(obj, null)))
        let fields = itemType.GetFields() |> Seq.map (fun field -> makeSymbols field (field.GetValue(obj)))
        
        Seq.append properties fields
        |> Seq.fold (fun x (s:Symbol) -> s.Replace x transform) format