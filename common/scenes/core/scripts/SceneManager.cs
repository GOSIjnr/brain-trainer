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
		{ "none", null }
	};

	private readonly Dictionary<string, string> _scenePaths = new()
	{
		{ "welcome_page", "uid://5v24jc0gxja7" },
		{ "main_menu", "uid://blpko71pew773" },
	};

	public async void ChangeScene(string sceneKey)
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

		await LoadScene(sceneKey);
		SwitchToScene(sceneKey);

		lock (this)
		{
			_isLoading = false;
			_currentLoadingScene = string.Empty;
		}
	}

	private async Task LoadScene(string sceneKey, bool waitForLoad = true)
	{
		if (!_scenePaths.TryGetValue(sceneKey, out string value) || _loadedScenes.ContainsKey(sceneKey))
			return;

		string scenePath = value;

		if (!IsValidResource(scenePath))
		{
			Logger.LogMessage($"Scene not found: {scenePath}");
			return;
		}

		if (_loadedScenes.Count >= SceneMemoryLimit)
			UnloadLeastRecentlyUsedScene();

		ResourceLoader.LoadThreadedRequest(scenePath);

		if (waitForLoad)
		{
			while (ResourceLoader.LoadThreadedGetStatus(scenePath) == ResourceLoader.ThreadLoadStatus.InProgress)
			{
				await ToSignal(GetTree(), "process_frame");
			}
		}

		var sceneResource = ResourceLoader.LoadThreadedGet(scenePath) as PackedScene;

		if (sceneResource != null)
		{
			_loadedScenes[sceneKey] = sceneResource;
			_sceneUsage[sceneKey] = (int)Time.GetTicksMsec();
			Logger.LogMessage($"Scene loaded: {sceneKey}");
		}
		else
		{
			Logger.LogMessage($"Failed to load scene: {sceneKey}");
		}
	}

	public void LoadSceneSync(string sceneKey)
	{
		LoadScene(sceneKey, false).Wait();
	}

	private bool IsValidResource(string path)
	{
		return ResourceLoader.Exists(path);
	}

	private void SwitchToScene(string sceneKey)
	{
		if (!_loadedScenes.TryGetValue(sceneKey, out PackedScene value))
		{
			Logger.LogMessage($"Scene not loaded: {sceneKey}");
			return;
		}

		_currentActiveScene = value;
		_sceneUsage[sceneKey] = (int)Time.GetTicksMsec();
		Logger.LogMessage($"Switching to scene: {sceneKey}");

		GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, _currentActiveScene);
	}

	private void UnloadLeastRecentlyUsedScene()
	{
		if (_sceneUsage.Count == 0) return;

		var leastUsedKey = _sceneUsage.OrderBy(sceneData => sceneData.Value).First().Key;
		_loadedScenes.Remove(leastUsedKey);
		_sceneUsage.Remove(leastUsedKey);

		Logger.LogMessage($"Unloaded least recently used scene: {leastUsedKey}");
	}
}
