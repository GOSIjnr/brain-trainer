using Godot;

namespace GOSIjnr;

public partial class CrashHandlerScene : CanvasLayer
{
	public override void _Ready()
	{
		Button closeButton = GetNodeOrNull<Button>("%CloseButton");

		if (closeButton != null) closeButton.Pressed += OnCloseButtonPressed;
	}

	private void OnCloseButtonPressed()
	{
		GetTree().Quit();
	}
}
