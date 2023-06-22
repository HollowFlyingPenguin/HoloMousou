using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float damage = 1;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController controller = GetComponentInParent<MovementController>();
        Character hitChara = collision.GetComponentInParent<Character>();
        if (hitChara && controller)
        {
            int hitLayer = collision.gameObject.layer;
            ObjectType objectType = controller.GetObjectType();
            if (hitLayer == LayerMask.NameToLayer("GrazeHurtbox") && objectType.Equals(ObjectType.Bullet) && !controller.GetGrazed())
            {
                Debug.Log("Graze ");
                controller.SetGrazed(true);
            }
            else
            {
                GameManager.Instance.Hit(hitChara, damage);
                if (controller.GetDestroyOnAttack())
                {
                    ObjectPoolManager.Instance.ReturnObjectToPool(controller);
                }
            }
        }
    }
}