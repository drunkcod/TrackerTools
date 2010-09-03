namespace TrackerTools

open System.IO
open System.Xml.Serialization
open NUnit.Framework

module TrackerStoriesTests =
    let Total = 2
    let mutable stories = TrackerStories()

    [<SetUp>]
    let When_deserializing_sample() =
        let serializer = XmlSerializer(typeof<TrackerStories>)
        use xml = new StringReader(@"<?xml version='1.0' encoding='UTF-8'?><stories type='array' count='1' total='2'/>")
        stories <- serializer.Deserialize(xml) :?> TrackerStories
        
    [<Test>] let total_is_parsed() = Assert.That(stories.Total, Is.EqualTo(Total))
        
