using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    Basic,
    Bomb
}

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] protected ShotType type = ShotType.Basic;
    [SerializeField] protected float damage = 1;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController controller = GetComponentInParent<MovementController>();
        Character hitChara = collision.GetComponentInParent<Character>();
        if (hitChara && controller)
        {
            GameManager.Instance.Hit(hitChara, damage);
            if (controller.GetDestroyOnAttack())
            {
                ObjectPoolManager.Instance.ReturnObjectToPool(controller);
            }
        }
    }
}
