using ModestTree;
using System;
using UnityEngine;
using Zenject;


public class MoveHandler : MonoBehaviour, IPauseHandler, IRestartHandler, IGameOverHandler
{
	[SerializeField] private Level _level;
	[SerializeField] private Transform _stopPoint;
	[SerializeField] private float _gravityVelocity;

	private Player _player;
	private float _velocity;
	private PauseHandler _pauseHandler;
	private RestartHandler _restartHandler;
	private GameOverHandler _gameOverHandler;

	public bool IsPaused { get; private set; }

	[Inject]
	private void Construct(Player player, PauseHandler pauseHandler, 
						   RestartHandler restartHandler, GameOverHandler gameOverHandler)
	{
		_player = player;
		_pauseHandler = pauseHandler;
		_restartHandler = restartHandler;
		_gameOverHandler = gameOverHandler;
	}

	private void FixedUpdate()
	{
		if (IsPaused)
			return;
		
		Gravity();
		Move();
	}

	private void OnEnable()
	{
		_player.OnJumpEvent += Jump;
		_pauseHandler.Register(this);
		_restartHandler.Register(this);
		_gameOverHandler.Register(this);
	}

	private void OnDisable()
	{
		_player.OnJumpEvent -= Jump;
		_pauseHandler.UnRegister(this);
		_restartHandler.UnRegister(this);
		_gameOverHandler.UnRegister(this);
	}

	public void Pause(bool value)
	{
		IsPaused = value;
	}

	public void Restart()
	{
		IsPaused = false;
	}

	public void GameOver()
	{
		IsPaused = true;
	}

	private void Move()
	{
		if (_player.Position.y <= _stopPoint.position.y && _velocity > 0)
		{
			MovePlatforms();
			return;
		}

		MovePlayer();
	}

	private void MovePlatforms()
	{
		if (_level.Platforms.IsEmpty())
			return;

		foreach (Platform platform in _level.Platforms)
		{
			platform.Move(Vector3.up, _velocity);
		}
	}

	private void MovePlayer()
	{
		_player.Move(Vector3.down, _velocity);
	}

	private void Jump(float jumpForce)
	{
		_velocity = -jumpForce;
	}

	private void Gravity()
	{
		_velocity += _gravityVelocity * Time.fixedDeltaTime;
	}
}