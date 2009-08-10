namespace TrackerTools

type TrackerStory() =
    [<DefaultValue>] val mutable id : int
    [<DefaultValue>] val mutable story_type : string
    [<DefaultValue>] val mutable url : string
    [<DefaultValue>] val mutable estimate : int
    [<DefaultValue>] val mutable current_state : string
    [<DefaultValue>] val mutable description : string
    [<DefaultValue>] val mutable name : string
    [<DefaultValue>] val mutable requested_by : string
    [<DefaultValue>] val mutable owned_by : string
    [<DefaultValue>] val mutable created_at : string
    [<DefaultValue>] val mutable accepted_at : string
    [<DefaultValue>] val mutable labels : string