using Godot;

namespace GOSIjnr;

public partial class TouchInputHandler(Vector2 boundSize) : Node
{
	private Rect2 _currentTouchRect;
	private Vector2 _boundSize = boundSize;

	[Signal] public delegate void TouchStartedEventHandler(Vector2 position);
	[Signal] public delegate void TouchCompletedEventHandler();

	public void ProcessTouchInput(InputEvent @event)
	{
		if (@event is not InputEventScreenTouch touchInput) return;

		if (touchInput.IsPressed())
		{
			CreateTouchBound(touchInput.Position);
			EmitSignalTouchStarted(touchInput.Position);
			return;
		}

		if (touchInput.IsReleased() && ValidateRelease(touchInput.Position))
		{
			EmitSignalTouchCompleted();
		}
	}

	private void CreateTouchBound(Vector2 position)
	{
		Vector2 rectPosition = position - (_boundSize / 2);
		_currentTouchRect = new Rect2(rectPosition, _boundSize);
	}

	private bool ValidateRelease(Vector2 position)
	{
		return _currentTouchRect.HasPoint(position);
	}
}