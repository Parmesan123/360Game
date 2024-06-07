using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
	public class RestartView : PanelView, IGameOverHandler
	{
		[SerializeField] private Button _restartButton;
		[SerializeField] private Button _quiteButton;

		private RestartHandler _restartHandler;
		private GameOverHandler _gameOverHandler;

		[Inject]
		private void Construct(RestartHandler restartHandler, GameOverHandler gameOverHandler)
		{
			_restartHandler = restartHandler;
			_gameOverHandler = gameOverHandler;
		}
		
		private void OnEnable()
		{
			_restartButton.onClick.AddListener(RestartButtonListener);
			_quiteButton.onClick.AddListener(QuitButtonListener);
			_gameOverHandler.Register(this);
		}

		private void OnDisable()
		{
			_restartButton.onClick.RemoveListener(RestartButtonListener);
			_quiteButton.onClick.RemoveListener(QuitButtonListener);
			_gameOverHandler.UnRegister(this);
		}

		public void GameOver()
		{
			Invoke(nameof(ShowGameOverPanel), 1f);
		}
		
		private void ShowGameOverPanel()
		{
			ShowPanel(true);
		}
		
		private void RestartButtonListener()
		{
			ShowPanel(false);
			_restartHandler.Restart();
			ShowPanel(false);
		}

		private void QuitButtonListener()
		{
			Application.Quit();
		}
	}
}