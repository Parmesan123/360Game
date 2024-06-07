using System;
using UnityEngine;
using Zenject;


[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IRestartHandler, IGameOverHandler
{
    public event Action<float> OnJumpEvent;

    [SerializeField] private ParticleSystem _particleSystem;
    [field: SerializeField] public PlayerData PlayerData { get; private set; }

    private Rigidbody _rigidbody;
    private Vector3 _startPosition;
    private RestartHandler _restartHandler;
    private GameOverHandler _gameOverHandler;
    
    public Vector3 Position => transform.position;

    [Inject]
    private void Construct(RestartHandler restartHandler, GameOverHandler gameOverHandler)
    {
        _restartHandler = restartHandler;
        _gameOverHandler = gameOverHandler;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        _restartHandler.Register(this);
        _gameOverHandler.Register(this);
    }

    private void OnDisable()
    {
        _restartHandler.UnRegister(this);
        _gameOverHandler.UnRegister(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<GrayPlatform>())
            return;
        
        _particleSystem.Play();
        Jump();
    }

    public void Restart()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }

    public void GameOver()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }
    
    public void Move(Vector3 direction, float speed)
    {
        Vector3 newPosition = Position + direction * Time.fixedDeltaTime * speed;
        _rigidbody.MovePosition(newPosition);
    }
    
    private void Jump()
    {
        OnJumpEvent.Invoke(PlayerData.JumpForce);
    }
}
