using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected bool destroyOnHit = false;
    [SerializeField] protected float damage = 1;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController controller = GetComponentInParent<MovementController>();
        Character hitChara = collision.GetComponentInParent<Character>();
        Debug.Log("Hit " + hitChara.name);
        if (hitChara && controller)
        {
            GameManager.Instance.Hit(hitChara, damage);
            if (destroyOnHit)
            {
                ObjectPoolManager.Instance.ReturnObjectToPool(controller);
            }
        }
    }
}