using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    Basic,
    Bomb
}

public class PlayerAttack : Attack
{
    [SerializeField] protected ShotType type = ShotType.Basic;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (type == ShotType.Bomb)
        {
            int hitLayer = collision.gameObject.layer;
            EnemyBulletController enemyBullet = collision.GetComponentInParent<EnemyBulletController>();
            if (enemyBullet && hitLayer == LayerMask.NameToLayer("EnemyHitbox"))
            {
                enemyBullet.Dispel();
            }
        }
    }

    protected override void OnHit(Character hitChara)
    {
        base.OnHit(hitChara);
        GameManager.Instance.PlayerDamage(damage, type);
    }
}