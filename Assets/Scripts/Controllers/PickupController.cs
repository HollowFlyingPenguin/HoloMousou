using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MovementController
{
    protected override void Start()
    {
        base.Start();
        SetMovementDirection(90);
    }
}
