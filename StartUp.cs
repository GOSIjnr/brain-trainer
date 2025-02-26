using Godot;

namespace GOSIjnr;

public partial class StartUp : CanvasLayer
{
	public override void _Ready()
	{
		Logger.LogMessage("Hello");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Space)
		{
			Core.Instance.ToastManager.ShowToastNotification("Space key pressed!", 1.5f);
		}
	}
}
