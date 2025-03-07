using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class QuestionController : BoxContainer
{
	private TutorialQuestion _question;
	private Array<TutorialOption> _questionOptions;
	private ButtonGroup _questionButtons;

	private RichTextLabel _questionText;
	private GridContainer _buttonHolder;
	private Button _skipButton;
	private Button _submitButton;

	private bool _isNextButtonLocked;
	private Button _selectedButton;

	[Signal] public delegate void QuestionAnsweredEventHandler(Data.Subjects subject, float score);

	public bool IsNextButtonLocked
	{
		get => _isNextButtonLocked;
		private set
		{
			_isNextButtonLocked = value;

			if (_isNextButtonLocked)
			{
				_selectedButton = null;
				_skipButton.Disabled = false;
				_submitButton.Disabled = true;
				return;
			}

			_skipButton.Disabled = true;
			_submitButton.Disabled = false;
		}
	}

	public override void _EnterTree()
	{
		_questionText = GetNode<RichTextLabel>("%QuestionText");
		_buttonHolder = GetNode<GridContainer>("%GridContainer");
		_skipButton = GetNode<Button>("%Skip");
		_submitButton = GetNode<Button>("%Submit");

		_skipButton.Pressed += SkipQuestion;
		_submitButton.Pressed += SubmitAnswer;
	}

	public override void _Ready()
	{
		_questionButtons = new();
		_questionButtons.Pressed += AnswerButtonPressed;
		_questionButtons.AllowUnpress = true;


		foreach (var item in _buttonHolder.GetChildren())
		{
			if (item is Button option)
			{
				option.ButtonGroup = _questionButtons;
			}
		}
	}

	private void AnswerButtonPressed(BaseButton button)
	{
		_selectedButton = (Button)button;
		IsNextButtonLocked = !button.ButtonPressed;
	}

	public void SetQuestion(TutorialQuestion question)
	{
		_question = question;

		_questionText.Text = _question.QuestionText;
		_questionOptions = _question.QuestionOptions.Duplicate(true);
		_questionOptions.Shuffle();

		IsNextButtonLocked = true;

		foreach (var button in _questionButtons.GetButtons())
		{
			button.ButtonPressed = false;
		}

		if (_questionOptions.Count <= 2)
		{
			_buttonHolder.Columns = 1;
		}
		else
		{
			_buttonHolder.Columns = 2;
		}

		var optionCount = _questionOptions.Count;

		for (int index = 0; index < _questionButtons.GetButtons().Count; index++)
		{
			if (index >= 0 && index < optionCount)
			{
				var button = _questionButtons.GetButtons()[index] as Button;
				button.Visible = true;
				button.Text = _questionOptions[index].OptionText;
			}
			else
			{
				var button = _questionButtons.GetButtons()[index];
				button.Visible = false;
			}
		}
	}

	private void SkipQuestion()
	{
		EmitSignalQuestionAnswered(_question.QuestionType, 0);
	}

	private void SubmitAnswer()
	{

		var selectedOptionIndex = _selectedButton.GetIndex();
		var selectedOptionPoints = _questionOptions[selectedOptionIndex].Points;
		EmitSignalQuestionAnswered(_question.QuestionType, selectedOptionPoints);
	}
}
