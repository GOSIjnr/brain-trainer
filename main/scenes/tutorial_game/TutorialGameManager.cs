using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class TutorialGameManager : CanvasLayer
{
	[Export] private string _questionPath;

	private TimerCountdown _quizTimer;
	private TextureProgressBar _questionProgressBar;
	private QuestionController _questionController;

	private Array<TutorialQuestion> _questions = [];
	private int _currentQuestionIndex = -1;
	private Dictionary<Data.Subjects, float> _questionScores = new()
	{
		{Data.Subjects.Writing, 0 },
		{Data.Subjects.Speaking, 0 },
		{Data.Subjects.Reading, 0 },
		{Data.Subjects.Maths, 0 },
		{Data.Subjects.Memory, 0 },
	};

	public override void _EnterTree()
	{
		_quizTimer = GetNode<TimerCountdown>("%TimerCountdown");
		_questionProgressBar = GetNode<TextureProgressBar>("%QuestionProgress");
		_questionController = GetNode<QuestionController>("%QuestionController");

		_quizTimer.CountdownOver += QuizOver;
		_questionController.QuestionAnswered += CheckAnswer;
	}

	public override void _Ready()
	{
		var loadedQuestion = Utils.GetResources<TutorialQuestion>(_questionPath);

		foreach (var question in loadedQuestion)
		{
			_questions.Add(question);
		}

		StartQuiz();
	}

	private void StartQuiz()
	{
		_questionProgressBar.Value = 0;
		_questionProgressBar.MaxValue = _questions.Count;

		_quizTimer.StartTimer();
		_questions.Shuffle();
		NextQuestion();
	}

	private void NextQuestion()
	{
		_currentQuestionIndex++;
		_questionProgressBar.Value = _currentQuestionIndex;

		if (_currentQuestionIndex <= _questions.Count - 1)
		{
			_questionController.SetQuestion(_questions[_currentQuestionIndex]);
		}

		if (_currentQuestionIndex >= _questions.Count)
		{
			QuizOver();
			return;
		}
	}

	private void CheckAnswer(Data.Subjects subject, float score)
	{
		_questionScores[subject] += score;
		NextQuestion();
	}

	private void QuizOver()
	{
		GradeUser();
		Core.Instance.SceneManager.ChangeScene("onboarding_page");
	}

	private void GradeUser()
	{
		var saveManager = Core.Instance.SaveManager;
		saveManager.LoadUserData();

		var userData = saveManager.userData;

		userData.writing.CurrentPoints = _questionScores[Data.Subjects.Writing];
		userData.speaking.CurrentPoints = _questionScores[Data.Subjects.Speaking];
		userData.reading.CurrentPoints = _questionScores[Data.Subjects.Reading];
		userData.maths.CurrentPoints = _questionScores[Data.Subjects.Maths];
		userData.memory.CurrentPoints = _questionScores[Data.Subjects.Memory];

		saveManager.SaveUserData();
	}
}
