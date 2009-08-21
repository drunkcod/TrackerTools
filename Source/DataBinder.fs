namespace TrackerTools
open System
open System.Xml.Serialization

module DataBinder =
    let ToString = function
        | null -> String.Empty
        | x -> x.ToString()
        
    let Bind(format:string, x:obj) = 
        let result = ref format
        let itemType = x.GetType()
        itemType.GetProperties()
        |> Seq.iter (fun prop ->
            let value = ToString(prop.GetValue(x, null))
            result := (!result).Replace(String.Format("$({0})", prop.Name), value)
            prop.GetCustomAttributes(typeof<XmlElementAttribute>, false)
            |> Seq.cast<XmlElementAttribute>
            |> Seq.iter (fun x -> result := (!result).Replace(String.Format("$({0})", x.ElementName), value)))
                                    
        itemType.GetFields()
        |> Seq.iter (fun field ->
            let value = ToString (field.GetValue(x))
            result := (!result).Replace(String.Format("$({0})", field.Name), value)
            field.GetCustomAttributes(typeof<XmlElementAttribute>, false)
            |> Seq.cast<XmlElementAttribute>
            |> Seq.iter (fun x -> result := (!result).Replace(String.Format("$({0})", x.ElementName), value)))         
        !result