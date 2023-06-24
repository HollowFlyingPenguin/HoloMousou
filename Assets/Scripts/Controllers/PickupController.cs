using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MovementController
{
    protected bool autoPickup = false;

    protected override void Start()
    {
        base.Start();
        SetMovementDirection(90);
        autoPickup = GameManager.Instance.AutoPickup;
        GameManager.Instance.OnEnableAutoPickup += () => autoPickup = true;
        GameManager.Instance.OnDisableAutoPickup += () => autoPickup = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (autoPickup)
        {
            movementDirection = GameManager.Instance.VectorToPlayer(transform.position);
        }
    }

    public void SetAutoPickup(bool autoPickup)
    {
        this.autoPickup = autoPickup;
    }
}
