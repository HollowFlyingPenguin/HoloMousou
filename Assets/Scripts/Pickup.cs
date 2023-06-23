using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Power,
    Score,
    BigPower,
    Life
}

public class Pickup : MonoBehaviour
{
    [SerializeField] PickupType type = PickupType.Power;
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
                if (controller.GetDestroyOnAttack())
                {
                    ObjectPoolManager.Instance.ReturnObjectToPool(controller);
                }
            }
        }
    }
}
