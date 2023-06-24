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
        if (controller)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(controller);
            switch (type)
            {
                case PickupType.Power:
                    GameManager.Instance.PickupPower();
                    break;
                case PickupType.Score:
                    GameManager.Instance.PickupScore();
                    break;
                case PickupType.BigPower:
                    GameManager.Instance.PickupBigPower();
                    break;
                case PickupType.Life:
                    GameManager.Instance.PickupLife();
                    break;
                default:
                    break;
            }
        }
    }
}
