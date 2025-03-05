using Godot;
using Godot.Collections;
using System.Threading.Tasks;

namespace GOSIjnr;

public partial class EPQ : BoxContainer
{
	[Export] public Data.Subjects BarType { get; private set; }

	private TextureProgressBar _singleBar;
	private Control _multiBarHolder;
	private Array<TextureProgressBar> _progressBars = [];

	private RichTextLabel _scoreLabel;
	private Label _titleLabel;

	private Color _progressColor = new("#2BBFA9");
	public Color ProgressColor
	{
		get => _progressColor;
		set
		{
			_progressColor = value;
			_ = UpdateBar();
		}
	}

	public override void _EnterTree()
	{
		_scoreLabel = GetNodeOrNull<RichTextLabel>("%Score");
		_titleLabel = GetNodeOrNull<Label>("%Title");
		_singleBar = GetNodeOrNull<TextureProgressBar>("%SingleBar");
		_multiBarHolder = GetNodeOrNull<BoxContainer>("%MultiBar");
	}

	public override void _Ready()
	{
		foreach (var child in _multiBarHolder.GetChildren())
		{
			if (child is TextureProgressBar bar)
			{
				_progressBars.Add(bar);
			}
		}
	}

	private async Task UpdateBar()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		var userData = Core.Instance.SaveManager.userData;

		switch (BarType)
		{
			case Data.Subjects value when value == Data.Subjects.Writing:
				UpdateUI(true, userData.writing.CurrentPoints, "WRITING");
				break;
			case Data.Subjects value when value == Data.Subjects.Speaking:
				UpdateUI(true, userData.speaking.CurrentPoints, "SPEAKING");
				break;
			case Data.Subjects value when value == Data.Subjects.Reading:
				UpdateUI(true, userData.reading.CurrentPoints, "READING");
				break;
			case Data.Subjects value when value == Data.Subjects.Maths:
				UpdateUI(true, userData.maths.CurrentPoints, "MATHS");
				break;
			case Data.Subjects value when value == Data.Subjects.Memory:
				UpdateUI(true, userData.memory.CurrentPoints, "MEMORY");
				break;
			case Data.Subjects value when value == Data.Subjects.Average:
				UpdateUI(false, userData.average.CurrentPoints, "AVERAGE");
				break;
			default:
				break;
		}
	}

	private void UpdateUI(bool isMultibar, float score, string epqName)
	{
		if (isMultibar)
		{
			_titleLabel.Text = GetScoreTitle(score);
			_multiBarHolder.Visible = true;
		}
		else
		{
			_titleLabel.Visible = false;
			_singleBar.Visible = true;
		}

		_scoreLabel.Text = $"[color=#999999]{epqName}: [/color][b] {score} + [/b]";

		foreach (var bar in _progressBars)
		{
			bar.Value = score;
		}
	}

	private string GetScoreTitle(float score)
	{
		switch (score)
		{
			case float value when value >= 4750:
				return "MASTER";
			case float value when value >= 4250:
				return "ELITE";
			case float value when value >= 3750:
				return "EXPERT";
			case float value when value >= 2500:
				return "ADVANCED";
			case float value when value >= 1250:
				return "INTERMEDIATE";
			default:
				return "NOVICE";
		}
	}
}
