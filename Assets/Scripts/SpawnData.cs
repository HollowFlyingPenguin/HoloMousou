using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "SpawnData")]
public class SpawnData : ScriptableObject
{
    public MovementController controller;
    public Vector2 spawnOffset;
}