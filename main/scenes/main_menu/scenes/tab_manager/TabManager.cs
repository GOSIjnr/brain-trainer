using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class TabManager : Panel
{
	[Export] public Array<Node> Contents { get; private set; } = [];

	private BoxContainer _tabHolder;
	private Array<TabButton> _tabButtons = [];
	private Color _deselectedTabColor = new("#BFBFBF");

	[Signal] public delegate void TabSwitchedEventHandler(int currentTab);

	public override void _EnterTree()
	{
		_tabHolder = GetNodeOrNull<BoxContainer>("%BoxContainer");
	}

	public override void _Ready()
	{
		if (_tabHolder == null) return;

		foreach (var child in _tabHolder.GetChildren())
		{
			if (child is not TabButton) return;

			var tabButton = (TabButton)child;

			_tabButtons.Add(tabButton);
			tabButton.ButtonClicked += TabSelected;
		}

		ResetTabs();
		_tabButtons[0].OnBoxContainerClicked();
	}

	public override void _ExitTree()
	{
		foreach (var tabButton in _tabButtons)
		{
			if (IsInstanceValid(tabButton))
			{
				tabButton.ButtonClicked -= TabSelected;
			}
		}
	}

	public void TabSelected(TabButton tab)
	{
		ResetTabs();

		tab.SelectTab(Core.Instance.Data.AppPrimaryColor);
		EmitSignalTabSwitched(tab.GetIndex());
	}

	public void ResetTabs()
	{
		foreach (var tabButton in _tabButtons)
		{
			tabButton.DeselectTab(_deselectedTabColor);
		}
	}
}
