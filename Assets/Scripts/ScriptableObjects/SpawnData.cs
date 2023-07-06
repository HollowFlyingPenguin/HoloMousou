using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "SpawnData")]
public class SpawnData : ScriptableObject
{
    public IndividualSpawnData[] autoSpawnArray = new IndividualSpawnData[1];
}

[System.Serializable]
public class IndividualSpawnData
{
    public MovementController[] prefabs = new MovementController[1];
    public Vector2 spawnOffset;
    public bool spawnAsChild = false;
    public bool targetPlayer = false;
    public float bulletCount = 1;

    /// <summary>
    /// Automatically spaces the bullets evenly from each other. Doesn't work with bulletSpread. RandomAngleOffset is applied afterwards. 
    /// </summary>
    public bool autoSpace = false;

    /// <summary>
    /// The space between multiple bullets when shot manually.
    /// </summary>
    public float bulletSpread = 0;

    public float angleOffset = 0;
    public float randomAngleOffset = 0;

    /// <summary>
    /// Bullets shot per second
    /// </summary>
    public float fireRate = 1f;

    public bool reverseSpin = false;

    /// <summary>
    /// The amount the angleOffset changes per second
    /// </summary>
    public float spinRate = 0;
    public float spinAccel = 0;
    public float spinMin = 0, spinMax = 100;
}