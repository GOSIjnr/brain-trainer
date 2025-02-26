using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace GOSIjnr;

[GlobalClass]
public partial class EventBus : Node
{
	private readonly ConcurrentDictionary<string, List<EventListener>> _eventListenersMap = new();
	private readonly object _eventLock = new();

	private class EventListener
	{
		public Delegate Callback { get; }
		public bool IsOneShot { get; }
		private readonly WeakReference<object> _listenerReference;

		public bool IsValid => _listenerReference == null || _listenerReference.TryGetTarget(out _);

		public EventListener(Delegate callback, bool isOneShot)
		{
			Callback = callback;
			IsOneShot = isOneShot;
			_listenerReference = callback.Target != null ? new WeakReference<object>(callback.Target) : null;
		}
	}

	private List<EventListener> GetListenersForEvent(string eventName)
	{
		_eventListenersMap.TryGetValue(eventName, out var listeners);
		return listeners;
	}

	private void InvokeEventListeners(List<EventListener> listeners, object eventData = null)
	{
		var listenersToRemove = new List<EventListener>();

		foreach (var listener in listeners.ToArray())
		{
			if (!listener.IsValid)
			{
				listenersToRemove.Add(listener);
				continue;
			}

			var parameters = listener.Callback.Method.GetParameters();

			if (parameters.Length == 0 && eventData == null)
			{
				Logger.LogMessage(
					$"Invoked {listener.Callback.Method.Name} with no parameters.",
					Logger.LogLevel.Debug
				);

				((Action)listener.Callback).Invoke();
			}
			else if (parameters.Length == 1 && parameters[0].ParameterType.IsInstanceOfType(eventData))
			{
				Logger.LogMessage(
					$"Invoked {listener.Callback.Method.Name} with parameter of type {parameters[0].ParameterType.Name}.",
					Logger.LogLevel.Debug
				);

				listener.Callback.DynamicInvoke(eventData);
			}
			else
			{
				Logger.LogMessage(
					$"Event data type does not match the expected parameter type for {listener.Callback.Method.Name}.",
					Logger.LogLevel.Warning
				);
			}

			if (listener.IsOneShot)
			{
				listenersToRemove.Add(listener);
			}
		}

		foreach (var listener in listenersToRemove)
		{
			listeners.Remove(listener);
		}

		if (listeners.Count == 0)
		{
			var eventName = _eventListenersMap.FirstOrDefault(pair => pair.Value == listeners).Key;
			if (eventName != null)
			{
				_eventListenersMap.TryRemove(eventName, out _);
			}
		}
	}

	private void RemoveEventListener(string eventName, Delegate callback)
	{
		lock (_eventLock)
		{
			if (_eventListenersMap.TryGetValue(eventName, out var listeners))
			{
				listeners.RemoveAll(listener => Equals(listener.Callback, callback) || !listener.IsValid);

				if (listeners.Count == 0)
				{
					_eventListenersMap.Remove(eventName, out _);
				}
			}
		}
	}

	private static void LogSubscription(string eventName, Delegate callback, bool isOneShot, string callerMethod, string callerFile, int callerLine)
	{
		string callerInfo = $"{callerMethod} in {callerFile}:{callerLine}";

		Logger.LogMessage(
			$"Subscribed {callback.Target?.GetType().Name}:{callback.Method.Name} to {eventName} (OneShot: {isOneShot}) from {callerInfo} (Thread: {System.Environment.CurrentManagedThreadId})",
			Logger.LogLevel.Debug
		);
	}

	private static void LogUnsubscription(string eventName, Delegate callback, string callerMethod, string callerFile, int callerLine)
	{
		string callerInfo = $"{callerMethod} in {callerFile}:{callerLine}";

		Logger.LogMessage(
			$"Unsubscribed {callback.Target?.GetType().Name}:{callback.Method.Name} from {eventName} by from {callerInfo} (Thread: {System.Environment.CurrentManagedThreadId})",
			Logger.LogLevel.Debug
		);
	}

	private static void LogEventPublishing(string eventName, string callerMethod, string callerFile, int callerLine)
	{
		string callerInfo = $"{callerMethod} in {callerFile}:{callerLine}";

		Logger.LogMessage(
			$"Publishing {eventName} from {callerInfo} (Thread: {System.Environment.CurrentManagedThreadId})",
			Logger.LogLevel.Debug
		);
	}

	private void RegisterEventListener(string eventName, EventListener eventListener)
	{
		lock (_eventLock)
		{
			if (!_eventListenersMap.TryGetValue(eventName, out var listeners))
			{
				listeners = [];
				_eventListenersMap[eventName] = listeners;
			}
			listeners.Add(eventListener);
		}
	}

	private bool IsSubscribed(string eventName, Delegate callback)
	{
		if (_eventListenersMap.TryGetValue(eventName, out var listeners))
		{
			bool isSubscribed = listeners.Any(listener => listener.Callback == callback);

			if (isSubscribed)
			{
				Logger.LogMessage($"{callback.Target?.GetType().Name}:{callback.Method.Name} is already subscribed to event '{eventName}'", Logger.LogLevel.Warning);
			}

			return isSubscribed;
		}

		return false;
	}

	public void Subscribe(string eventName, Action callback, bool isOneShot = false, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (IsSubscribed(eventName, callback)) return;

		var eventListener = new EventListener(callback, isOneShot);
		RegisterEventListener(eventName, eventListener);
		LogSubscription(eventName, callback, isOneShot, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Subscribe<T>(string eventName, Action<T> callback, bool isOneShot = false, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		if (IsSubscribed(eventName, callback)) return;

		var eventListener = new EventListener(callback, isOneShot);
		RegisterEventListener(eventName, eventListener);
		LogSubscription(eventName, callback, isOneShot, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Unsubscribe(string eventName, Action callback, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		RemoveEventListener(eventName, callback);
		LogUnsubscription(eventName, callback, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Unsubscribe<T>(string eventName, Action<T> callback, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		RemoveEventListener(eventName, callback);
		LogUnsubscription(eventName, callback, callerMethod, callerFile.GetFile(), callerLine);
	}

	public void Publish(string eventName, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		lock (_eventLock)
		{
			var listeners = GetListenersForEvent(eventName);

			if (listeners == null || listeners.Count == 0) return;

			LogEventPublishing(eventName, callerMethod, callerFile.GetFile(), callerLine);
			InvokeEventListeners(listeners);
		}
	}

	public void Publish<T>(string eventName, T eventData, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
	{
		lock (_eventLock)
		{
			var listeners = GetListenersForEvent(eventName);

			if (listeners == null || listeners.Count == 0) return;

			LogEventPublishing(eventName, callerMethod, callerFile.GetFile(), callerLine);
			InvokeEventListeners(listeners, eventData);
		}
	}

	public void PrintAllActiveEvents()
	{
		Logger.IsBatchModeEnabled = true;
		Logger.LogMessage("Active Events:");

		if (_eventListenersMap.IsEmpty)
		{
			Logger.LogMessage("	No active events.");
			return;
		}

		foreach (var (eventName, listeners) in _eventListenersMap)
		{
			Logger.LogMessage($"	Event: {eventName} ({listeners.Count} listeners)");

			foreach (var listener in listeners)
			{
				string methodName = listener.Callback.Method.Name;
				string targetType = listener.Callback.Target?.GetType().Name ?? "Static Method";
				string validityStatus = listener.IsValid ? "✅ Valid" : "❌ Invalid";

				Logger.LogMessage(
					$"		↳ [{validityStatus}] Method: {methodName} in {targetType}, OneShot: {listener.IsOneShot}"
				);
			}
		}

		Logger.IsBatchModeEnabled = false;
	}
}
