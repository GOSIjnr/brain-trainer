using Godot;

namespace GOSIjnr;

public partial class MainMenu : CanvasLayer
{
	[Export] public TabManager TabManager { get; private set; }

	private Control _contentMenu;

	public override void _EnterTree()
	{
		_contentMenu = GetNodeOrNull<Control>("%Contents");

		if (TabManager == null) return;

		TabManager.TabSwitched += OnTabSwitched;
	}

	public override void _Ready()
	{
		Core.Instance.DailyManager.SaveDailies();
	}

	public void OnTabSwitched(int currentTab)
	{
		if (_contentMenu == null) return;

		GD.Print("Tab switched to: " + currentTab);
	}
}
