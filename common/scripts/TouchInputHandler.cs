using Godot;

namespace GOSIjnr;

/// <summary>
/// Handles touch input events by creating a bounding rectangle around the touch position
/// and emitting signals when the touch starts or is completed.
/// </summary>
public partial class TouchInputHandler(Vector2 boundSize) : Node
{
	private Rect2 _currentTouchRect;
	private readonly Vector2 _boundSize = boundSize;

	/// <summary>
	/// Emitted when a touch starts.
	/// </summary>
	[Signal] public delegate void TouchStartedEventHandler(Vector2 position);

	/// <summary>
	/// Emitted when a touch is completed.
	/// </summary>
	[Signal] public delegate void TouchCompletedEventHandler();

	/// <summary>
	/// Processes the provided input event for touch actions.
	/// </summary>
	/// <param name="event">Input event to process.</param>
	public void ProcessTouchInput(InputEvent @event)
	{
		if (@event is not InputEventScreenTouch touchInput)
			return;

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

	/// <summary>
	/// Creates a bounding rectangle centered on the provided touch position.
	/// </summary>
	/// <param name="position">The touch position.</param>
	private void CreateTouchBound(Vector2 position)
	{
		Vector2 rectPosition = position - (_boundSize / 2);
		_currentTouchRect = new Rect2(rectPosition, _boundSize);
	}

	/// <summary>
	/// Validates whether the release position is within the touch bound.
	/// </summary>
	/// <param name="position">The release position.</param>
	/// <returns>True if the position is inside the touch bound; otherwise, false.</returns>
	private bool ValidateRelease(Vector2 position) => _currentTouchRect.HasPoint(position);
}