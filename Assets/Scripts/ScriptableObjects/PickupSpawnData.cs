using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupSpawnData", menuName = "PickupSpawnData")]
public class PickupSpawnData : ScriptableObject
{
    public int powerCount = 0;
    public int scoreCount = 0;
    public int bigPowerCount = 0;
    public int lifeCount = 0;
}