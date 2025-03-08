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
	public DailyManager DailyManager { get; private set; }

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
		DailyManager = GetNodeOrNull<DailyManager>("%DailyManager") ?? new DailyManager();

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

		if (OS.GetName() != "Windows") return;
		Logger.LogMessage("Changed the Window Size Beacuse it is on Windows");
		var screenSize = DisplayServer.WindowGetSize();
		var newSize = new Vector2I(1080, 1920) / 3;

		GetWindow().Position = new Vector2I(screenSize.X / 2, (screenSize - newSize).Y / 2);
		DisplayServer.WindowSetSize(newSize);
	}

	private void InitializeApp()
	{
		//TODO: make it false then add backbutton implementation to navigate backwards
		GetTree().QuitOnGoBack = true;

		float refreshRate = DisplayServer.ScreenGetRefreshRate();
		refreshRate = (refreshRate < 0.0f) ? 60.0f : refreshRate;
		Engine.MaxFps = Mathf.FloorToInt(refreshRate);
	}
}
