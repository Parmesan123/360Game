using Zenject;


public class OrangeTrigger : Trigger
{
	private GameOverHandler _gameOverHandler;

	[Inject]
	private void Construct(GameOverHandler gameOverHandler)
	{
		_gameOverHandler = gameOverHandler;
	}
	
	protected override void TriggerOnPlayer()
	{
		_gameOverHandler.GameOver();
	}
}

