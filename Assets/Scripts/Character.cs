using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MovementController
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected SpawnData bulletSpawnData;

    public virtual void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Shoot()
    {
        if (bulletSpawnData)
        {
            ObjectPoolManager.Instance.InitializeObject(bulletSpawnData, transform.position);
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Dead");
    }
}
