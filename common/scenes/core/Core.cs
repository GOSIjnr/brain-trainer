using Godot;

namespace GOSIjnr;

[GlobalClass]
public partial class Core : Node
{
	public CrashHandler CrashHandler { private set; get; }
	public Data Data { private set; get; }
	public EventBus EventBus { private set; get; }
	public SceneManager SceneManager { private set; get; }

	public static Core Instance { get; private set; }

	public override void _EnterTree()
	{
		Instance = this;
		Data = GetNodeOrNull<Data>("%Data");
		CrashHandler = GetNodeOrNull<CrashHandler>("%CrashHandler");
		EventBus = GetNodeOrNull<EventBus>("%EventBus");
		SceneManager = GetNodeOrNull<SceneManager>("%SceneManager");

		InitializeDebugMode();
		InitializeSignals();
		InitializeApp();
	}

	private void InitializeSignals()
	{
		if (CrashHandler == null || EventBus == null) return;
		EventBus.Subscribe("game_crashed", CrashHandler.HandleCrash);
	}

	private void InitializeDebugMode()
	{
		if (!OS.HasFeature("debug")) return;
		Logger.SetLogLevel(Logger.LogLevel.Debug);
	}

	private void InitializeApp()
	{
		// TODO: Implement a settings manager to load these values from a file

		float refreshRate = DisplayServer.ScreenGetRefreshRate();
		refreshRate = (refreshRate < 0.0f) ? 60.0f : refreshRate;
		Engine.MaxFps = Mathf.FloorToInt(refreshRate);
	}
}
