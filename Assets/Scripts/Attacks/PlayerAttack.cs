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

    protected override void OnHit(Character hitChara)
    {
        base.OnHit(hitChara);
        GameManager.Instance.PlayerDamage(damage, type);
    }
}