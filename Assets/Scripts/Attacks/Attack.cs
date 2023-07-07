using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float damage = 1;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController controller = GetComponentInParent<MovementController>();
        Character hitChara = collision.GetComponentInParent<Character>();
        if (hitChara && controller)
        {
            int hitLayer = collision.gameObject.layer;
            ObjectType objectType = controller.GetObjectType();
            if (hitLayer == LayerMask.NameToLayer("GrazeHurtbox"))
            {
                if (objectType.Equals(ObjectType.Bullet) && !controller.GetGrazed())
                {
                    GameManager.Instance.Graze();
                    controller.SetGrazed(true);
                }
            }
            else if (hitLayer == LayerMask.NameToLayer("EnemyHurtbox") || hitLayer ==  LayerMask.NameToLayer("PlayerHurtbox"))
            {
                OnHit(hitChara);
                if (controller.GetDestroyOnAttack())
                {
                    MovementPoolManager.Instance.ReturnObjectToPool(controller);
                }
            }
        }
    }

    protected virtual void OnHit(Character hitChara)
    {
        GameManager.Instance.Hit(hitChara, damage);
    }
}