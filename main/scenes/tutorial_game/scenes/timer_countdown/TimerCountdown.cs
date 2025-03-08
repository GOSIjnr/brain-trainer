using Godot;

namespace GOSIjnr;

public partial class TimerCountdown : Label
{
	[Export] private float _waitTime = 300.0f;
	[Export(PropertyHint.Range, "0, 100, 0.1")] private float _waitTimeWarningThresold = 10.0f;

	private Timer _timer;
	private AudioStreamPlayer _audioStream;

	private float _waitTimeWarning;
	private double _elaspedTime = 0.0f;
	private bool _isTimerOver = false;

	[Signal] public delegate void CountdownOverEventHandler();

	public override void _EnterTree()
	{
		_timer = GetNode<Timer>("%Timer");
		_audioStream = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
		_timer.Timeout += TimerTimeout;

		_waitTimeWarning = _waitTime * _waitTimeWarningThresold / 100.0f;
	}

	public override void _ExitTree()
	{
		_timer.Timeout -= TimerTimeout;
	}

	public override void _Process(double delta)
	{
		var timeLeft = Mathf.Round(_timer.TimeLeft);
		string formattedTime = timeLeft.ToString(":000");
		Text = formattedTime;

		if (timeLeft <= _waitTimeWarning + 1.0f) PlayAudioAfterDelay(1.0f, delta);
	}

	public void StartTimer()
	{
		_timer.WaitTime = _waitTime;
		_timer.Start();
	}

	private void TimerTimeout()
	{
		if (_isTimerOver) return;

		_isTimerOver = true;
		EmitSignalCountdownOver();
	}

	private void PlayAudioAfterDelay(float delayInSeconds, double delta)
	{
		_elaspedTime += delta;

		if (_elaspedTime < delayInSeconds || _audioStream.Playing || _timer.TimeLeft == 0.0f) return;
		_audioStream.Play();
		_elaspedTime = 0.0f;

		if (_isTimerOver) _elaspedTime = 0.0f;
	}
}
