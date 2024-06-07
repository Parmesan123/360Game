using EzySlice;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class Platform : MonoBehaviour
{
    public event Action<Platform> OnDestroyEvent;

    [SerializeField] private Trigger _platformTrigger; 
    
    [field: SerializeField] public GameObject GrayPlatform { get; private set; }
    private readonly List<OrangePieceData> _piecesData = new List<OrangePieceData>();
    private readonly List<GameObject> _platformObjects = new List<GameObject>();

    public IReadOnlyList<OrangePieceData> PiecesData => _piecesData;

    private void OnEnable()
    {
        _platformTrigger.OnTriggerEvent += DestroyPlatform;
    }

    private void OnDisable()
    {
        _platformTrigger.OnTriggerEvent -= DestroyPlatform;
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 newPosition = transform.position + direction * (speed * Time.fixedDeltaTime);
        transform.position = newPosition;
    }
    
    public void SetOrangeTrigger(GameObject piece, float startAngle, float endAngle)
    {
        piece.transform.SetParent(transform);
        OrangePieceData newData = new OrangePieceData(startAngle, endAngle);
        _piecesData.Add(newData);
        _platformObjects.Add(piece);
    }

    public void SetGrayTrigger(IEnumerable<GameObject> pieces)
    {
        foreach (GameObject piece in pieces)
        {
            piece.transform.SetParent(transform);
        }
        
        _platformObjects.AddRange(pieces);
    }

    private void DestroyPlatform()
    {
        OnDestroyEvent.Invoke(this);
        
        BrokeAnimation();   
    }
    
    public void BrokeAnimation()
    {
        _platformTrigger.gameObject.SetActive(false);
        
        List<GameObject> gameObjects = new List<GameObject>();
        
        for (int i = 0; i < 4; ++i)
        {
            Vector3 position = new Vector3(Random.Range(0f, 3f), 0 , Random.Range(0f, 3f));
            Vector3 direction = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.left;

            foreach (GameObject platformObject in _platformObjects)
            {
                Material material = platformObject.GetComponent<MeshRenderer>().material;

                GameObject[] pieces = platformObject.SliceInstantiate(position, direction, material);

                if (pieces == null)
                {
                    gameObjects.Add(platformObject);
                    continue;
                }
                gameObjects.AddRange(pieces);
                Destroy(platformObject);
            }
            
            _platformObjects.Clear();
            _platformObjects.AddRange(gameObjects);
            gameObjects.Clear();
        }
        
        foreach (GameObject platform in _platformObjects)
        {
            Rigidbody rb = platform.AddComponent<Rigidbody>();
            platform.AddComponent<MeshCollider>().convex = true;
            rb.AddExplosionForce(150, transform.position, 0);
        }

        Invoke(nameof(Destroy), 3);
    }

    private void Destroy()
    {
        foreach (GameObject platformObject in _platformObjects)
        {
            Destroy(platformObject);
        }
        
        Destroy(gameObject);
    }
}
