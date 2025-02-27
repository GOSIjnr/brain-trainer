using Godot;

namespace GOSIjnr;

public partial class TabButton : BoxContainer
{
	[Export] public string TabText { get; private set; }
	[Export] public Texture2D TabSelected { get; private set; }
	[Export] public Texture2D TabDeselected { get; private set; }

	public TextureButton TabTextureButton { get; private set; }
	public Label label;

	[Signal] public delegate void ButtonClickedEventHandler(TabButton currentButton);

	public override void _EnterTree()
	{
		TabTextureButton = GetNodeOrNull<TextureButton>("%TextureButton");
		label = GetNodeOrNull<Label>("%Label");
	}

	public override void _Ready()
	{
		if (TabTextureButton == null || label == null) return;

		label.Text = TabText.Trim();

		TabTextureButton.TextureNormal = TabDeselected;
		TabTextureButton.TexturePressed = TabSelected;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			OnBoxContainerClicked();
		}
	}

	public void OnBoxContainerClicked()
	{
		EmitSignalButtonClicked(this);
	}
}
