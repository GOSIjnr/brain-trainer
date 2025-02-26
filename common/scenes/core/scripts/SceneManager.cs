using Godot;
using System.Linq;
using Godot.Collections;
using System.Threading.Tasks;

namespace GOSIjnr;

[GlobalClass]
public partial class SceneManager : Node
{
	private PackedScene _currentActiveScene;

	private Dictionary<string, PackedScene> _loadedScenes = new();
	private Dictionary<string, int> _sceneUsage = new();
	private const int SceneMemoryLimit = 3;

	private bool _isLoading = false;
	private string _currentLoadingScene = string.Empty;
	private Control _loadingIndicator;

	private readonly Dictionary<string, PackedScene> _loadingUI = new()
	{
		{ "random", null }
	};

	private readonly Dictionary<string, string> _scenePaths = new()
	{
		{ "welcome_page", "uid://cvklhkmwyvuap" }
	};

	public async void ChangeScene(string sceneKey, string loadingKey = "none")
	{
		lock (this)
		{
			if (_isLoading)
			{
				if (_currentLoadingScene == sceneKey)
				{
					Logger.LogMessage($"Scene is already loading: {_scenePaths[sceneKey]}");
				}
				else
				{
					Logger.LogMessage("Another scene is currently loading. Please wait.");
				}

				return;
			}

			if (!_scenePaths.ContainsKey(sceneKey))
			{
				Logger.LogMessage($"Invalid scene key: {sceneKey}");
				return;
			}

			if (_loadedScenes.ContainsKey(sceneKey))
			{
				SwitchToScene(sceneKey);
				return;
			}

			_isLoading = true;
			_currentLoadingScene = sceneKey;
		}

		ShowLoadingUI();
		await LoadScene(sceneKey);
		SwitchToScene(sceneKey);
		HideLoadingUI();

		lock (this)
		{
			_isLoading = false;
			_currentLoadingScene = string.Empty;
		}
	}

	private async Task LoadScene(string sceneKey)
	{
		if (!_scenePaths.ContainsKey(sceneKey) || _loadedScenes.ContainsKey(sceneKey))
			return;

		string scenePath = _scenePaths[sceneKey];

		if (!IsValidResource(scenePath))
		{
			Logger.LogMessage($"Scene not found: {scenePath}");
			return;
		}

		if (_loadedScenes.Count >= SceneMemoryLimit)
			UnloadLeastRecentlyUsedScene();


		ResourceLoader.LoadThreadedRequest(scenePath);

		while (ResourceLoader.LoadThreadedGetStatus(scenePath) == ResourceLoader.ThreadLoadStatus.InProgress)
		{
			await ToSignal(GetTree(), "process_frame");
		}

		var sceneResource = ResourceLoader.LoadThreadedGet(scenePath) as PackedScene;

		if (sceneResource != null)
		{
			_loadedScenes[sceneKey] = sceneResource;
			_sceneUsage[sceneKey] = (int)Time.GetTicksMsec();
			Logger.LogMessage($"Scene loaded: {scenePath}");
		}
		else
		{
			Logger.LogMessage($"Failed to load scene: {scenePath}");
		}

	}

	private bool IsValidResource(string path)
	{
		return ResourceLoader.Exists(path);
	}

	private void SwitchToScene(string sceneKey)
	{
		if (!_loadedScenes.ContainsKey(sceneKey))
		{
			Logger.LogMessage($"Scene not loaded: {sceneKey}");
			return;
		}

		_currentActiveScene = _loadedScenes[sceneKey];
		_sceneUsage[sceneKey] = (int)Time.GetTicksMsec();
		Logger.LogMessage($"Switching to scene: {_scenePaths[sceneKey]}");

		GetTree().ChangeSceneToPacked(_currentActiveScene);
	}

	private void UnloadLeastRecentlyUsedScene()
	{
		if (_sceneUsage.Count == 0)
			return;

		var leastUsedKey = _sceneUsage.OrderBy(kv => kv.Value).First().Key;
		_loadedScenes.Remove(leastUsedKey);
		_sceneUsage.Remove(leastUsedKey);

		Logger.LogMessage($"Unloaded least recently used scene: {leastUsedKey}");
	}

	private void ShowLoadingUI()
	{
		if (_loadingUI["random"] == null)
			return;

		if (_loadingIndicator != null)
			GetTree().Root.AddChild(_loadingIndicator);
	}

	private void HideLoadingUI()
	{
		_loadingIndicator?.QueueFree();
		_loadingIndicator = null;
	}
}
