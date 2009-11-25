namespace TrackerTools
open System.IO
open NUnit.Framework

module When_parsing_GetIterations_sample_response =
    let result = FromXml<TrackerIterations> <| File.OpenRead("TestData\GetIterationsSampleResponse.xml")

    [<Test>]
    let number_of_iterations_should_match() = Assert.That(result.Items.Count, Is.EqualTo(5))
    [<Test>]
    let iterations_should_contain_stories() =
        Assert.That(result.Items.[0].Stories.Count, Is.EqualTo(2))
    [<Test>]
    let first_iteration_first_story_should_be_a_feature() =
        Assert.That(result.Items.[0].Stories.[0].StoryType, Is.EqualTo("feature"))