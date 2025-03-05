using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class TutorialGameManager : CanvasLayer
{
	[Export] private string _questionPath;

	private QuestionController _questionController;
	private Array<TutorialQuestion> _questions = [];
	private int _currentQuestionIndex = 0;
	private Dictionary<Data.Subjects, float> _questionScores = new()
	{
		{Data.Subjects.Writing, 0 },
		{Data.Subjects.Speaking, 0 },
		{Data.Subjects.Reading, 0 },
		{Data.Subjects.Maths, 0 },
		{Data.Subjects.Memory, 0 },
	};

	public override void _Ready()
	{
		var loadedQuestion = Utils.GetResources<TutorialQuestion>(_questionPath);

		foreach (var question in loadedQuestion)
		{
			_questions.Add(question);
		}
	}

	private void StartQuiz()
	{
		_questions.Shuffle();
	}

	private void NextQuestion()
	{

	}

	private void CheckAnswer()
	{

	}

	private void QuizOver()
	{

	}
}
