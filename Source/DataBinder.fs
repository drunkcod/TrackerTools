namespace TrackerTools
open System.Reflection
open System.Xml.Serialization
      
module DataBinder =
    let Bind(format:string, obj:obj) =
        let getNames (mi:MemberInfo) = seq { 
            yield mi.Name 
            for x in mi.GetCustomAttributes(typeof<XmlElementAttribute>, false) do 
                yield (x :?> XmlElementAttribute).ElementName} 
           
        let makeSymbols (mi:MemberInfo) value = getNames mi |> Seq.map (fun name -> Symbol(name, value))
            
        let itemType = obj.GetType()        
        let properties = itemType.GetProperties() |> Seq.collect (fun prop -> makeSymbols prop (prop.GetValue(obj, null)))
        let fields = itemType.GetFields() |> Seq.collect (fun field -> makeSymbols field (field.GetValue(obj)))
        
        Seq.append properties fields
        |> Seq.fold (fun x (s:Symbol) -> s.Replace x) format