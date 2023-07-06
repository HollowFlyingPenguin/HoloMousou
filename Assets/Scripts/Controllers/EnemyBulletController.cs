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
        Debug.Log("dispel");
        ObjectPoolManager.Instance.ReturnObjectToPool(this);
    }
}
