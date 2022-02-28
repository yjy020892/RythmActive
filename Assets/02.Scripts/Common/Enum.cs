namespace Enums_Common
{
	public enum SceneType
	{
		Base,
		Loading,
        FirstLoading,
		Intro,
		Lobby,
        SongSelect,
        Tutorial,
        Game_Single,
		Game_Multi,
        Result,
        Rank,
        GameOver
	}

    public enum ServerState
    {
        None,
        Connect,
        Disconnect
    }
}

namespace Enums_Game
{
	public enum UIType
    {
		None,
		Scale,
        Slider,
        Animation,
        Game
    }

    public enum GameMode
    {
        None,
        Single,
        Multi
    }

    //public enum JointType
    //{
    //    Head = 0,
    //    ShoulderSpine = 1,
    //    LeftShoulder = 2,
    //    LeftElbow = 3,
    //    LeftHand = 4,
    //    RightShoulder = 5,
    //    RightElbow = 6,
    //    RightHand = 7,
    //    MidSpine = 8,
    //    BaseSpine = 9,
    //    LeftHip = 10,
    //    LeftKnee = 11,
    //    LeftFoot = 12,
    //    RightHip = 13,
    //    RightKnee = 14,
    //    RightFoot = 15,
    //    LeftWrist = 16,
    //    RightWrist = 17,
    //    Neck = 18,
    //    Unknown = 255
    //}

    public enum NoteBody
    {
        None,
        HandLeft,
        HandRight,
        LeftAnkle,
        RightAnkle
    }

    public enum NoteType
    {
        Single,
        Press,
        Long,
        Stop
    }

    public enum SongDifficult
    {
        Easy,
        Normal,
        Hard
    }

    public enum GameState
    {
        None,
        Sensor,
        Tutorial,
        UI,
        Wait,
        Play,
        End
    }

    public enum ResultState
    {
        //Title,
        //Good,
        //Perfect,
        //Miss,
        //Combo,
        //Score,
        //End
        None,
        LifeEnd,
        ScoreEnd
    }

    public enum RankingState
    {
        None,
        first,
        second,
        third,
        score,
        End
    }

    public enum Goal
    {
        New,
        Silver,
        Gold,
        Diamond,
        Ruby
    }
}