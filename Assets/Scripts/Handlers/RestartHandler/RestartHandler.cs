using System.Collections.Generic;

public class RestartHandler : IRestartHandler
{
	private readonly List<IRestartHandler> _restartHandlers = new List<IRestartHandler>();

	public void Register(IRestartHandler restartHandler)
	{
		_restartHandlers.Add(restartHandler);
	}

	public void UnRegister(IRestartHandler restartHandler)
	{
		_restartHandlers.Remove(restartHandler);
	}
	
	public void Restart()
	{
		foreach (IRestartHandler restartHandler in _restartHandlers)
		{
			restartHandler.Restart();
		}
	}
}