using Godot;

namespace GOSIjnr;

public partial class StartUp : CanvasLayer
{
	public override void _Ready()
	{
		var saveManager = Core.Instance.SaveManager;

		if (saveManager.IsUserSaveDataPresent)
		{
			saveManager.LoadUserData();

			var data = saveManager.userData;
			var newScores = Utils.MergeDictionaries(data.highScores, Core.Instance.Data.TemplateScores);
			data.highScores.Clear();

			foreach (var score in newScores)
			{
				data.highScores.Add(score.Key, score.Value);
			}

			saveManager.SaveUserData();
		}
		else
		{
			saveManager.CreateUserData();
			saveManager.SaveUserData();
		}

		if (saveManager.userData.isTutorialDone)
		{
			Core.Instance.SceneManager.ChangeScene("main_menu");
		}
		else
		{
			Core.Instance.SceneManager.ChangeScene("welcome_page");
		}
	}
}
