using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class WelcomePage : CanvasLayer
{
	[Export] private Array<WelcomeContentPage> _contentPages = [];

	private int _currentPage;

	public override void _Ready()
	{
		foreach (var page in _contentPages)
		{
			page.Visible = false;
			page.NextPageButtonClick += DisplayNextPage;
			page.PreviousPageButtonClick += DisplayPreviousPage;
		}

		_contentPages[0].Visible = true;
	}

	public override void _ExitTree()
	{
		foreach (var page in _contentPages)
		{
			page.NextPageButtonClick -= DisplayNextPage;
			page.PreviousPageButtonClick -= DisplayPreviousPage;
		}
	}

	private void DisplayNextPage()
	{
		_contentPages[_currentPage].Visible = false;

		_currentPage++;
		if (_currentPage < _contentPages.Count) _contentPages[_currentPage].Visible = true;

		if (_currentPage >= _contentPages.Count)
		{
			Core.Instance.SaveManager.SaveUserData();
			Core.Instance.SceneManager.ChangeScene("tutorial_game");
		}
	}

	private void DisplayPreviousPage()
	{
		_contentPages[_currentPage].Visible = false;

		_currentPage--;
		if (_currentPage >= 0) _contentPages[_currentPage].Visible = true;
	}
}
