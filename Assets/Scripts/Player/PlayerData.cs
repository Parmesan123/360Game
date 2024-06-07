using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float MaxFallVelocity { get; private set; }
}
