using Godot;
using Godot.Collections;
using System.Runtime.CompilerServices;

namespace GOSIjnr;

public static class Logger
{
	private static readonly Array<string> _logEntriesBuffer = [];
	private static bool _isBatchModeEnabled = false;
	private static int _maxBatchSize = 50;

	private static LogLevel _currentLogLevel = LogLevel.Info;
	private static LogLevel _errorLogLevel = LogLevel.Error;
	private static LogLevel _fatalLogLevel = LogLevel.Fatal;

	private const string LogSeparator = "==========================================================================";

	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error,
		Fatal,
	}

	public static bool IsBatchModeEnabled
	{
		set
		{
			if (_isBatchModeEnabled != value)
			{
				if (!value) FlushBufferedLogs();

				_isBatchModeEnabled = value;
				LogBatchModeChange(value);
			}
		}
	}

	public static int MaxBatchSize
	{
		get => _maxBatchSize;
		set => _maxBatchSize = Mathf.Clamp(value, 2, int.MaxValue);
	}

	public static void SetLogLevel(LogLevel value) => ChangeLogLevel(ref _currentLogLevel, value, "CurrentLogLevel");
	public static void SetErrorLogLevel(LogLevel value) => ChangeLogLevel(ref _errorLogLevel, value, "ErrorLogLevel");
	public static void SetFatalLogLevel(LogLevel value) => ChangeLogLevel(ref _fatalLogLevel, value, "FatalLogLevel");

	private static void ChangeLogLevel(ref LogLevel targetLogLevel, LogLevel newLevel, string propertyName)
	{
		targetLogLevel = newLevel;
		LogMessage($"{propertyName} has been set to {newLevel.ToString().ToUpper()}", LogLevel.Debug);
	}

	private static void LogBatchModeChange(bool isActivated)
	{
		string status = isActivated ? "activated" : "deactivated";
		LogMessage($"BatchMode has been {status}");
	}

	public static void LogMessage(string message = "", LogLevel level = LogLevel.Info, [CallerFilePath] string callerFile = "")
	{
		if (level < _currentLogLevel) return;

		string timestamp = GeneratePreciseTimestamp();
		string callerScriptName = callerFile.GetFile().GetBaseName();
		string logEntry = $"[{timestamp}] [{level.ToString().ToUpper()}] [{callerScriptName}] {message}";

		bool isMessageLogged = false;

		if (_isBatchModeEnabled)
		{
			_logEntriesBuffer.Add(logEntry);

			if (_logEntriesBuffer.Count >= _maxBatchSize)
			{
				FlushBufferedLogs();
				isMessageLogged = true;
			}
		}
		else
		{
			GD.Print(logEntry);
			isMessageLogged = true;
		}

		if (level >= _errorLogLevel)
		{
			FlushBufferedLogs();
			isMessageLogged = true;
			GD.PrintErr(System.Environment.StackTrace);
		}

		if (isMessageLogged) GD.Print(LogSeparator);

		if (level >= _fatalLogLevel) Core.Instance.EventBus.Publish("game_crashed");
	}

	private static void FlushBufferedLogs()
	{
		if (_logEntriesBuffer.Count == 0) return;

		GD.Print(string.Join("\n", _logEntriesBuffer));
		_logEntriesBuffer.Clear();
	}

	private static string GeneratePreciseTimestamp()
	{
		string dateTime = Time.GetDatetimeStringFromSystem(true, true);
		ulong milliseconds = Time.GetTicksMsec() % 1000;
		return $"{dateTime}.{milliseconds:D3}";
	}
}
