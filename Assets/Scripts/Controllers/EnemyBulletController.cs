using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MovementController
{
    /// <summary>
    /// Turns into score.
    /// </summary>
    public void Dispel()
    {
        GameManager.Instance.SpawnConvertedScore(transform.position);
        MovementPoolManager.Instance.ReturnObjectToPool(this);
    }
}
