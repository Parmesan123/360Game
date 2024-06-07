using UnityEngine;
using EzySlice;
using Zenject;
using Random = UnityEngine.Random;


public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private PlatformObjectData _objectData;
    [SerializeField] private Platform _platformPrefab;
    [SerializeField] private int _pieceCount;
    [SerializeField] private float _minAnglePiece;
    [SerializeField] private float _maxAnglePiece;
    [SerializeField] private float _angleOffSet;

    private DiContainer _container;

    [Inject]
    private void Constructor(DiContainer container)
    {
        _container = container;
    }
    
    private void Awake()
    {
        _objectData.transform.position = Vector3.zero;
    }
    
    public Platform CreatePlatform()
    {
        Platform platform = Instantiate(_platformPrefab);
        
        float startOffSetAngle = Random.Range(0, 360) / 10 * 10;
        float startAngle = startOffSetAngle;
        
        
        for (int i = 1; i <= _pieceCount; ++i)
        {
            float cutAngle = Random.Range(_minAnglePiece, _maxAnglePiece);

            GameObject newPiece = CreatePiece(startAngle, cutAngle);
            platform.SetOrangeTrigger(newPiece, startAngle, startAngle + cutAngle);
            
            startAngle = 360 / _pieceCount * i + startOffSetAngle + Random.Range(-_angleOffSet, _angleOffSet);
        }

        CreateGap(platform);
        
        return platform;
    }

    
    private void CreateGap(Platform platform)
    {
        GameObject greyPlatform = platform.GrayPlatform.gameObject;
        Material material = greyPlatform.GetComponent<MeshRenderer>().material;
        GameObject[] gameObjects = new GameObject[2];
        Vector3 direction = Vector3.left;
        
        int index = Random.Range(0, platform.PiecesData.Count);
        float startCutAngle = platform.PiecesData[index].EndAngle;
        index = (index + 1) % platform.PiecesData.Count;
        float endCutAngle = platform.PiecesData[index].StartAngle;

        startCutAngle = startCutAngle > endCutAngle ? 
            Random.Range(startCutAngle + 20, endCutAngle + 315) : 
            Random.Range(startCutAngle + 20, endCutAngle - 45);
        float cutAngle = startCutAngle > endCutAngle ? 
            Random.Range(startCutAngle + 30, endCutAngle + 360) - startCutAngle : 
            Random.Range(startCutAngle + 30, endCutAngle) - startCutAngle;
        cutAngle = Mathf.Clamp(cutAngle, 25, endCutAngle - startCutAngle - 10);
        
        direction = Quaternion.Euler(0, startCutAngle, 0) * direction;

        GameObject[] pieces = greyPlatform.SliceInstantiate(greyPlatform.transform.position,
                                                            direction, material);

        pieces[0].transform.SetParent(platform.transform);
        gameObjects[0] = pieces[0];
        GameObject lowerHull = pieces[1];
        direction = Quaternion.Euler(0, cutAngle, 0) * direction;
        pieces = lowerHull.SliceInstantiate(greyPlatform.transform.position,
                                            direction, material);
        pieces[1].transform.SetParent(platform.transform);
        gameObjects[1] = pieces[1];
        platform.SetGrayTrigger(gameObjects);
        foreach (GameObject trigger in gameObjects)
        {
            trigger.AddComponent<GrayPlatform>();
            MeshCollider collider = trigger.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = true;
        }
        
        Destroy(lowerHull);
        Destroy(pieces[0]);
        
        Destroy(greyPlatform);
    }
    
    private GameObject CreatePiece(float startCutAngle, float endCutAngle)
    {
        Vector3 direction = Quaternion.Euler(0, startCutAngle, 0) * Vector3.left;
        Material material = _objectData.OrangeGameObject.GetComponent<MeshRenderer>().material;
        
        GameObject[] pieces = _objectData.OrangeGameObject.SliceInstantiate(_objectData.Position, direction, 
                                                                            material);
        GameObject lowerObject = pieces[1];
        Destroy(pieces[0]);

        direction = Quaternion.Euler(0, endCutAngle, 0) * direction;
        
        pieces = lowerObject.SliceInstantiate(_objectData.Position, direction,
                                              material);
        Destroy(lowerObject);
        GameObject piece = pieces[0];
        piece.name = "piece";
        Destroy(pieces[1]);

        MeshCollider collider = piece.AddComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;
        _container.InstantiateComponent<OrangeTrigger>(piece);
        
        return piece;
    }
}
