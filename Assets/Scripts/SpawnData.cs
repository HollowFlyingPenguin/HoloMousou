using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnBehavior
{
    Automatic,
    OnSpawn,
    OnDeath,
}

[CreateAssetMenu(fileName = "SpawnData", menuName = "SpawnData")]
public class SpawnData : ScriptableObject
{
    public IndividualSpawnData[] autoSpawnArray = new IndividualSpawnData[1];
}

[System.Serializable]
public class IndividualSpawnData
{
    public MovementController prefab;
    public Vector2 spawnOffset;
    public bool targetPlayer = false;
    public float angleOffset = 0;
    public float randomAngleOffset = 0;

    /// <summary>
    /// // Bullets per second
    /// </summary>
    public float fireRate = 1f;

    /// <summary>
    /// The amount the angleOffset changes per second
    /// </summary>
    public float spinRate = 0;
}