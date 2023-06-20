using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "SpawnData")]
public class SpawnData : ScriptableObject
{
    public MovementController prefab;
    public Vector2 spawnOffset;
    public bool targetPlayer = false;

    /// <summary>
    /// // Bullets per second
    /// </summary>
    public float fireRate = 1f; 
}