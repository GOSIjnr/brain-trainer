using Godot;

namespace GOSIjnr
{
	public partial class MainMenu : CanvasLayer
	{
		[Export] public TabManager TabManager { get; private set; }

		private Control _contentMenu;

		public override void _EnterTree()
		{
			_contentMenu = GetNodeOrNull<Control>("%Contents");

			if (TabManager != null)
			{
				TabManager.TabSwitched += OnTabSwitched;
			}
		}

		public override void _Ready()
		{
			Core.Instance.DailyManager.SaveDailies();
		}

		/// <summary>
		/// Event handler that is called when a tab is switched.
		/// Logs the current tab index to the Godot console.
		/// </summary>
		/// <param name="currentTab">The index of the newly selected tab.</param>
		public void OnTabSwitched(int currentTab)
		{
			if (_contentMenu == null)
			{
				return;
			}

			GD.Print("Tab switched to: " + currentTab);
		}
	}
}