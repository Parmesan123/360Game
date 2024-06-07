using NaughtyAttributes;
using System;
using UnityEngine;

namespace UI
{
	public abstract class PanelView : MonoBehaviour
	{
		[SerializeField] private bool _needSubPanel;

		[SerializeField, ShowIf(nameof(_needSubPanel))] private bool _defaultActiveSubPanel;
		[SerializeField, ShowIf(nameof(_needSubPanel))] protected GameObject _subPanel;
		[SerializeField] private bool _defaultActiveMainPanel;
		[SerializeField] protected GameObject _mainPanel;

		private void Awake()
		{
			if (_needSubPanel)
				_subPanel.SetActive(_defaultActiveSubPanel);

			_mainPanel.SetActive(_defaultActiveMainPanel);
		}

		protected void ShowPanel(bool value)
		{
			if (_needSubPanel)
				_subPanel.SetActive(!value);

			_mainPanel.SetActive(value);
		}
	}
}