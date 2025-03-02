using Godot;

namespace GOSIjnr;

public partial class Page1 : WelcomeContentPage
{
	private Button _nextButton;

	public override void _EnterTree()
	{
		_nextButton = GetNode<Button>("%Button");

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
