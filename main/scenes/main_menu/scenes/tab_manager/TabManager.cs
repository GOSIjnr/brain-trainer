using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class TabManager : Panel
{
	[Export] public Array<Node> Contents { get; private set; }
	[Export] public TabButton SelectedTab { get; private set; }

	private BoxContainer _tabHolder;
	private Array<TabButton> _tabButtons = [];

	public override void _EnterTree()
	{
		_tabHolder = GetNodeOrNull<BoxContainer>("%HBoxContainer");
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
	}

	public void TabSelected(TabButton tab)
	{
		ResetTabs();

		tab.Modulate = Core.Instance.Data.AppPrimaryColor;
		tab.TabTextureButton.ButtonPressed = true;
	}

	public void ResetTabs()
	{
		foreach (var tabButton in _tabButtons)
		{
			tabButton.Modulate = new Color("#FFFFFF");
			tabButton.TabTextureButton.ButtonPressed = false;

			// reduce the font size after
		}
	}
}
