using Godot;

namespace GOSIjnr;

[GlobalClass]
public partial class ToastManager : Node
{
	[Export] private PackedScene _toastScene;

	private Toast _currentToast;

	public void ShowToastNotification(string message, float duration)
	{
		if (_toastScene == null)
		{
			Logger.LogMessage("There is no assigned _toastScene", Logger.LogLevel.Warning);
			return;
		}

		if (_currentToast != null)
		{
			Logger.LogMessage("There is current a toast running", Logger.LogLevel.Debug);
			return;
		}

		var instanceToast = _toastScene.InstantiateOrNull<Toast>();

		if (instanceToast == null)
		{
			Logger.LogMessage("Toast Scene is invalid", Logger.LogLevel.Warning);
			return;
		}

		GetTree().Root.CallDeferred(MethodName.AddChild, instanceToast);
		_ = instanceToast.ShowToastMessage(message, duration);
	}
}
