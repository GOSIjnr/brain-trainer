using Godot;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class UserData : Resource
{
	[Export] public string userName = "User";
	[Export] public bool isTutorialDone;
	[Export] public int workouts;
	[Export] public Dictionary<string, float> highScores = [];
	[Export] public PrionfenyQuotient writing = new();
	[Export] public PrionfenyQuotient speaking = new();
	[Export] public PrionfenyQuotient reading = new();
	[Export] public PrionfenyQuotient maths = new();
	[Export] public PrionfenyQuotient memory = new();
	[Export] public PrionfenyQuotient average = new();

	public partial class PrionfenyQuotient : Resource
	{
		[Export] public bool isRecommendedByUser;
		[Export] public float startingPoints = 10;
		[Export] private float _currentPoints;
		[Export] public Dictionary<string, float> dailyPoints = [];

		public float CurrentPoints
		{
			get => _currentPoints;
			set => _currentPoints = Mathf.Max(value, startingPoints);
		}

		public void ApplyData(Dictionary<string, Variant> newValue)
		{
			foreach (var key in newValue.Keys)
			{
				switch (key)
				{
					case "is_recommended_by_user":
						isRecommendedByUser = (bool)newValue[key];
						break;
					case "starting_points":
						startingPoints = (float)newValue[key];
						break;
					case "current_points":
						_currentPoints = (float)newValue[key];
						break;
					case "daily_points":
						dailyPoints = (Dictionary<string, float>)newValue[key];
						break;
				}
			}
		}

		public Dictionary<string, Variant> GetData()
		{
			return new Dictionary<string, Variant>
			{
				{ "is_recommened_by_user", isRecommendedByUser},
				{ "starting_points", startingPoints},
				{ "current_points", _currentPoints},
				{ "daily_points", dailyPoints},
			};
		}
	}

	public void ApplyData(Dictionary<string, Variant> newValue)
	{
		foreach (var key in newValue.Keys)
		{
			switch (key)
			{
				case "user_name":
					userName = (string)newValue[key];
					break;
				case "is_tutorial_done":
					isTutorialDone = (bool)newValue[key];
					break;
				case "workouts":
					workouts = (int)newValue[key];
					break;
				case "high_scores":
					highScores = (Dictionary<string, float>)newValue[key];
					break;
				case "writing":
					writing.ApplyData((Dictionary<string, Variant>)newValue[key]);
					break;
				case "speaking":
					speaking.ApplyData((Dictionary<string, Variant>)newValue[key]);
					break;
				case "reading":
					reading.ApplyData((Dictionary<string, Variant>)newValue[key]);
					break;
				case "maths":
					maths.ApplyData((Dictionary<string, Variant>)newValue[key]);
					break;
				case "memory":
					memory.ApplyData((Dictionary<string, Variant>)newValue[key]);
					break;
				case "average":
					average.ApplyData((Dictionary<string, Variant>)newValue[key]);
					break;
			}
		}
	}

	public Dictionary<string, Variant> GetData()
	{
		return new Dictionary<string, Variant>
		{
			{ "user_name", userName },
			{ "is_tutorial_done", isTutorialDone },
			{ "workouts", workouts },
			{ "high_scores", highScores },
			{ "writing", writing.GetData() },
			{ "speaking", speaking.GetData() },
			{ "reading", reading.GetData() },
			{ "maths", maths.GetData() },
			{ "memory", memory.GetData() },
			{ "average", average.GetData() },
		};
	}
}
