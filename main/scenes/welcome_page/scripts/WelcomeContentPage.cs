using Godot;

namespace GOSIjnr;

[GlobalClass]
public partial class WelcomeContentPage : CanvasLayer
{
	[Signal] public delegate void NextPageButtonClickEventHandler();
	[Signal] public delegate void PreviousPageButtonClickEventHandler();
}
