using System.Collections.Generic;


public class GameOverHandler : IGameOverHandler
{
	private readonly List<IGameOverHandler> _handlers = new List<IGameOverHandler>();

	public void Register(IGameOverHandler handler)
	{
		_handlers.Add(handler);
	}

	public void UnRegister(IGameOverHandler handler)
	{
		_handlers.Remove(handler);
	}

	public void GameOver()
	{
		foreach (IGameOverHandler handler in _handlers)
		{
			handler.GameOver();
		}
	}
}