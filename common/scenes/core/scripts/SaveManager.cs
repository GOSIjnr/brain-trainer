using Godot;

namespace GOSIjnr;

[GlobalClass]
public partial class SaveManager : Node
{
	[Export] public UserData userData;
	[Export] private bool _isUserSaveDataPresent = true;

	public override void _EnterTree()
	{
		CheckUserData();
	}

	private void CheckUserData()
	{
		_isUserSaveDataPresent = FileAccess.FileExists(Core.Instance.Data.UserDataSavePath);
	}

	public void CreateUserData()
	{
		userData = new();

		var newScores = Utils.MergeDictionaries(userData.highScores, Core.Instance.Data.TemplateScores);
		userData.highScores.Clear();

		foreach (var score in newScores)
		{
			userData.highScores.Add(score.Key, score.Value);
		}
	}

	public void LoadUserData()
	{
		userData = ResourceLoader.Load<UserData>(Core.Instance.Data.UserDataSavePath);
	}

	public void SaveUserData()
	{
		ResourceSaver.Save(userData, Core.Instance.Data.UserDataSavePath);
	}
}
