namespace TrackerTools

open NUnit.Framework
        
module StoryTemplateItemTests =
    let [<Test>] should_serialize_source_as_attribute() =
        let item = StoryTemplateItem(Source = "StoryTemplate.html")
        Assert.That(item.ToXml(), Is.EqualTo("<StoryTemplate Source=\"StoryTemplate.html\" />"))
        