using System;
using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class QuestionController : MarginContainer
{
	private Question _question;
	private Array<Button> _questionButtons;
	private bool isAnsweredPicked;

	private TextureProgressBar _progressBar;
	private Label _timerCountDown;
	private RichTextLabel _questionText;
	private GridContainer _buttonHolder;
	private Button _skipButton;
	private Button _submitButton;

	[Signal] public delegate void QuestionAnsweredEventHandler(float score);

	public override void _EnterTree()
	{
		_progressBar = GetNodeOrNull<TextureProgressBar>("%Progress");
		_timerCountDown = GetNodeOrNull<Label>("%TimerCountdown");
		_questionText = GetNodeOrNull<RichTextLabel>("%Question");
		_buttonHolder = GetNodeOrNull<GridContainer>("%GridContainer");
		_skipButton = GetNodeOrNull<Button>("%Skip");
		_submitButton = GetNodeOrNull<Button>("%Submit");

		_skipButton.Pressed += NextQuestion;
	}

	public override void _Ready()
	{
		var group = new ButtonGroup();
		group.AllowUnpress = true;
		isAnsweredPicked = false;

		foreach (var item in _buttonHolder.GetChildren())
		{
			if (item is Button option)
			{
				option.ToggleMode = true;
				option.ButtonGroup = group;
			}
		}
	}

	private void UpdateUI()
	{

	}

	private void NextQuestion()
	{
		EmitSignalQuestionAnswered(0);
	}
}
