using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
	public class PauseView : PanelView
	{
		[SerializeField] private Button _pauseButton;
		[SerializeField] private Button _unPauseButton;
		[SerializeField] private Button _restartButton;
 		[SerializeField] private Button _quiteButton;
		
		private IPauseHandler _pauseHandler;
		private IRestartHandler _restartHandler;
		
		[Inject]
		private void Construct(PauseHandler pauseHandler, RestartHandler restartHandler)
		{
			_pauseHandler = pauseHandler;
			_restartHandler = restartHandler;
		}

		private void OnEnable()
		{
			_pauseButton.onClick.AddListener(PauseButtonListener);
			_unPauseButton.onClick.AddListener(UnPauseButtonListener);
			_restartButton.onClick.AddListener(RestartButtonListener);
			_quiteButton.onClick.AddListener(QuitButtonListener);
		}

		private void OnDisable()
		{
			_pauseButton.onClick.RemoveListener(PauseButtonListener);
			_unPauseButton.onClick.RemoveListener(UnPauseButtonListener);
			_restartButton.onClick.RemoveListener(RestartButtonListener);
			_quiteButton.onClick.RemoveListener(QuitButtonListener);
		}

		private void PauseButtonListener()
		{
			ShowPanel(true);
			_pauseHandler.Pause(true);
		}

		private void RestartButtonListener()
		{
			ShowPanel(false);
			_restartHandler.Restart();
		}

		private void UnPauseButtonListener()
		{
			ShowPanel(false);
			_pauseHandler.Pause(false);
		}

		private void QuitButtonListener()
		{
			Application.Quit();
		}
	}
}