using Godot;

namespace GOSIjnr;

public partial class TabButton : BoxContainer
{
	[Export] public string TabText { get; private set; }
	[Export] public Texture2D TabSelected { get; private set; }
	[Export] public Texture2D TabDeselected { get; private set; }

	private TextureButton _tabTextureButton;
	private Label _tabLabel;
	private TouchInputHandler _touchHandler;

	[Signal] public delegate void ButtonClickedEventHandler(TabButton currentButton);

	public override void _EnterTree()
	{
		_tabTextureButton = GetNodeOrNull<TextureButton>("%TextureButton");
		_tabLabel = GetNodeOrNull<Label>("%Label");

		_touchHandler = new(new Vector2(100, 100));
		_touchHandler.TouchCompleted += OnBoxContainerClicked;
		AddChild(_touchHandler);
	}

	public override void _ExitTree()
	{
		_touchHandler.TouchCompleted -= OnBoxContainerClicked;
	}

	public override void _Ready()
	{
		if (_tabTextureButton == null || _tabLabel == null) return;

		_tabLabel.Text = TabText.Trim();

		_tabTextureButton.TextureNormal = TabDeselected;
		_tabTextureButton.TexturePressed = TabSelected;
	}

	public override void _GuiInput(InputEvent @event)
	{
		_touchHandler.ProcessTouchInput(@event);
	}

	public void OnBoxContainerClicked()
	{
		EmitSignalButtonClicked(this);
	}

	public void SelectTab(Color color)
	{
		Modulate = color;
		_tabTextureButton.ButtonPressed = true;
	}

	public void DeselectTab(Color color)
	{
		Modulate = color;
		_tabTextureButton.ButtonPressed = false;
	}
}
