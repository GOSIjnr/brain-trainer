using Godot;

namespace GOSIjnr;

[GlobalClass]
public partial class CrashHandler : Node
{
	[Export] private PackedScene _crashScene;

	public void HandleCrash()
	{
		GetTree().Paused = true;

		if (_crashScene == null)
		{
			Logger.LogMessage("No crash scene assigned.", Logger.LogLevel.Warning);
			GetTree().Quit();
			return;
		}

		Node crashSceneInstance = _crashScene.InstantiateOrNull<CrashHandlerScene>();

		if (crashSceneInstance == null)
		{
			Logger.LogMessage("Failed to instantiate crash scene.", Logger.LogLevel.Error);
			GetTree().Quit();
			return;
		}

		var rootNode = GetTree().Root;

		if (rootNode != null)
		{
			rootNode.CallDeferred(MethodName.AddChild, crashSceneInstance);
			Logger.LogMessage("Crash scene added successfully.", Logger.LogLevel.Debug);
		}
		else
		{
			Logger.LogMessage("Tree root is null, quitting.", Logger.LogLevel.Error);
			GetTree().Quit();
		}
	}
}
