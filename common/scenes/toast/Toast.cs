using Godot;
using System.Threading.Tasks;

namespace GOSIjnr;

public partial class Toast : CanvasLayer
{
	[Export] private Vector2 _toastPadding = new(100, 60);
	[Export] private float _animationTime = 0.2f;

	private Panel _toastPanel;
	private Label _toastLabel;

	public override void _EnterTree()
	{
		_toastPanel = GetNodeOrNull<Panel>("%Panel");
		_toastLabel = GetNodeOrNull<Label>("%Label");
	}

	public async Task ShowToastMessage(string toastMessage, float toastDuration)
	{
		await ToSignal(this, SignalName.Ready);
		if (_toastPanel == null || _toastLabel == null) return;

		_toastLabel.Text = toastMessage;
		_toastPanel.UpdateMinimumSize();
		_toastPanel.CustomMinimumSize = _toastLabel.GetMinimumSize() + _toastPadding;

		await AnimateToast(0, 1, _animationTime);
		await ToSignal(GetTree().CreateTimer(toastDuration), SceneTreeTimer.SignalName.Timeout);
		await HideToastMessage();
	}

	private async Task HideToastMessage()
	{
		await AnimateToast(1, 0, _animationTime);
		await ToSignal(GetTree().CreateTimer(_animationTime), SceneTreeTimer.SignalName.Timeout);
		QueueFree();
	}

	private async Task AnimateToast(float currentValue, float newValue, float duration)
	{
		var modulateNodePath = new NodePath(Panel.PropertyName.Modulate).GetAsPropertyPath();
		_toastPanel.Modulate = new(1, 1, 1, currentValue);

		var tween = CreateTween();
		tween.TweenProperty(_toastPanel, modulateNodePath, new Color(1, 1, 1, newValue), duration).SetEase(Tween.EaseType.In);
		await ToSignal(tween, Tween.SignalName.Finished);
	}
}
