using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected int scoreValue = 100;

    protected override void Die()
    {
        GameManager.Instance.GainEnemyScore(scoreValue);
        base.Die();
    }
}
