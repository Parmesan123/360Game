using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class Level : MonoBehaviour, IPauseHandler, IRestartHandler, IGameOverHandler
{
	[SerializeField] private float _yOffSet;
	[SerializeField] private int _maxNumberPlatform;
	[SerializeField] private float _rotationSpeed;
	[SerializeField] private PlatformSpawner _platformSpawner;

	private PauseHandler _pauseHandler;
	private RestartHandler _restartHandler;
	private GameOverHandler _gameOverHandler;

	private readonly List<Platform> _platforms = new List<Platform>();

	public bool IsPaused { get; private set; }
	public IReadOnlyList<Platform> Platforms => _platforms;

	[Inject]
	private void Construct(PauseHandler pauseHandler, RestartHandler restartHandler, GameOverHandler gameOverHandler)
	{
		_pauseHandler = pauseHandler;
		_restartHandler = restartHandler;
		_gameOverHandler = gameOverHandler;
	}

	private void Start()
	{
		AddPlatforms();
	}

	private void OnEnable()
	{
		_pauseHandler.Register(this);
		_restartHandler.Register(this);
		_gameOverHandler.Register(this);
	}

	private void OnDisable()
	{
		_pauseHandler.UnRegister(this);
		_restartHandler.UnRegister(this);
		_gameOverHandler.UnRegister(this);
	}

	private void FixedUpdate()
	{
		if (IsPaused)
			return;

		RotatePlatforms();
	}

	public void Restart()
	{
		Clear();
		AddPlatforms();
	}

	public void Pause(bool value)
	{
		IsPaused = value;
	}

	public void GameOver()
	{
		foreach (Platform platform in _platforms)
		{
			platform.BrokeAnimation();
		}
		
		_platforms.Clear();
	}

	private void RotatePlatforms()
	{
		float direction = -Input.GetAxisRaw("Horizontal");

		if (direction == 0)
			return;

		float angle = direction * _rotationSpeed * Time.fixedDeltaTime;

		foreach (Platform platform in _platforms)
		{
			platform.transform.rotation *= Quaternion.Euler(0, angle, 0);
		}
	}

	private void AddPlatforms()
	{
		if (_platforms.Count == 0)
		{
			StartAddPlatforms();
			return;
		}

		if (_platforms.Count == _maxNumberPlatform)
			return;

		AddPlatform();
		return;

		void StartAddPlatforms()
		{
			for (; _platforms.Count < _maxNumberPlatform;)
			{
				Platform newPlatform = _platformSpawner.CreatePlatform();
				newPlatform.transform.position = new Vector3(0, -_yOffSet * _platforms.Count, 0);
				newPlatform.transform.SetParent(transform);
				newPlatform.OnDestroyEvent += PlatformDestroyed;

				_platforms.Add(newPlatform);
			}
		}

		void AddPlatform()
		{
			Platform newPlatform = _platformSpawner.CreatePlatform();
			newPlatform.transform.position = new Vector3(0, -_yOffSet * _maxNumberPlatform - 1, 0);
			newPlatform.transform.SetParent(transform);
			newPlatform.OnDestroyEvent += PlatformDestroyed;

			_platforms.Add(newPlatform);
		}
	}

	private void PlatformDestroyed(Platform platform)
	{
		platform.OnDestroyEvent -= PlatformDestroyed;

		_platforms.Remove(platform);
		AddPlatforms();
	}

	private void Clear()
	{
		foreach (Platform platform in _platforms)
		{
			Destroy(platform.gameObject);
		}

		_platforms.Clear();
	}
}