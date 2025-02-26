using Godot;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class UserData : Resource
{
	[Export] public string userName = "User";
	[Export] public bool isTutorialDone;
	[Export] public int workouts;
	[Export] public Dictionary<string, float> highScores;
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
	}
}
