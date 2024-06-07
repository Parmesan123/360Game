using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformObjectData : MonoBehaviour
{
    [field: SerializeField] public OrangeTrigger OrangeTrigger { get; private set; }

    public GameObject OrangeGameObject => OrangeTrigger.gameObject;
    public Vector3 Position => transform.position;
}
