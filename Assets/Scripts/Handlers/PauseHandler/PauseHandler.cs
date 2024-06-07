using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : IPauseHandler
{
	private readonly List<IPauseHandler> _pauseHandlers = new List<IPauseHandler>();

	public bool IsPaused { get; private set; }

	public void Register(IPauseHandler pauseHandler)
	{
		_pauseHandlers.Add(pauseHandler);
	}

	public void UnRegister(IPauseHandler pauseHandler)
	{
		_pauseHandlers.Remove(pauseHandler);
	}
	
	public void Pause(bool value)
	{
		IsPaused = value;
		foreach (IPauseHandler pausePerformed in _pauseHandlers)
		{
			pausePerformed.Pause(value);
		}
	}
}
