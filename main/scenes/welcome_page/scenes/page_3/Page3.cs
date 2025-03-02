using Godot;

namespace GOSIjnr;

public partial class Page3 : WelcomeContentPage
{
	private Button _nextButton;

	public override void _Ready()
	{
		_nextButton = GetNodeOrNull<Button>("%Button");

		if (_nextButton == null) return;

		_nextButton.Pressed += NextPage;
	}

	public override void _ExitTree()
	{
		if (_nextButton != null) return;

		_nextButton.Pressed -= NextPage;
	}

	private void NextPage()
	{
		EmitSignalNextPageButtonClick();
	}
}
