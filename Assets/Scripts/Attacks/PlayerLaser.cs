using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : LaserAttack
{
    [SerializeField] protected ShotType type = ShotType.Basic;

    protected override void OnHit(Character hitChara)
    {
        base.OnHit(hitChara);
        GameManager.Instance.PlayerDamage(damage, type);
    }
}
