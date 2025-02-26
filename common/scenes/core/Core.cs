using Godot;

namespace GOSIjnr;

[GlobalClass]
public partial class Core : Node
{
	public CrashHandler CrashHandler { get; private set; }
	public Data Data { get; private set; }
	public EventBus EventBus { get; private set; }
	public SceneManager SceneManager { get; private set; }
	public SaveManager SaveManager { get; private set; }
	public ToastManager ToastManager { get; private set; }

	public static Core Instance { get; private set; }

	public override void _EnterTree()
	{
		Instance = this;
		Data = GetNodeOrNull<Data>("%Data") ?? new Data();
		CrashHandler = GetNodeOrNull<CrashHandler>("%CrashHandler") ?? new CrashHandler();
		EventBus = GetNodeOrNull<EventBus>("%EventBus") ?? new EventBus();
		SceneManager = GetNodeOrNull<SceneManager>("%SceneManager") ?? new SceneManager();
		SaveManager = GetNodeOrNull<SaveManager>("%SaveManager") ?? new SaveManager();
		ToastManager = GetNodeOrNull<ToastManager>("%ToastManager") ?? new ToastManager();

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
		GetTree().QuitOnGoBack = false;

		float refreshRate = DisplayServer.ScreenGetRefreshRate();
		refreshRate = (refreshRate < 0.0f) ? 60.0f : refreshRate;
		Engine.MaxFps = Mathf.FloorToInt(refreshRate);
	}
}
