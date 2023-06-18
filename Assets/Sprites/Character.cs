using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MovementController
{
    [SerializeField] protected float health = 100;

    public virtual void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Dead");
    }
}
