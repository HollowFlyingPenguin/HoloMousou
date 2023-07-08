using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Power,
    Score,
    BigPower,
    Life,
    ConvertedScore
}

public class Pickup : MonoBehaviour
{
    [SerializeField] PickupType type = PickupType.Power;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        int hitLayer = collision.gameObject.layer;
        PickupController controller = GetComponentInParent<PickupController>();
        if (hitLayer == LayerMask.NameToLayer("PlayerHurtbox"))
        {
            MovementPoolManager.Instance.ReturnObjectToPool(controller);
            switch (type)
            {
                case PickupType.Power:
                    GameManager.Instance.PickupPower();
                    break;
                case PickupType.Score:
                    GameManager.Instance.PickupScore(transform.position);
                    break;
                case PickupType.BigPower:
                    GameManager.Instance.PickupBigPower();
                    break;
                case PickupType.Life:
                    GameManager.Instance.PickupLife();
                    break;
                case PickupType.ConvertedScore:
                    GameManager.Instance.PickupConvertedScore();
                    break;
                default:
                    break;
            }
        }
        else if (hitLayer == LayerMask.NameToLayer("AutoCollectionHurtbox"))
        {
            controller.EnableAutoPickup();
        }
    }
}
