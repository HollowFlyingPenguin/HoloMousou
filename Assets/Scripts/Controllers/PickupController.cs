using UnityEngine;

public class PickupController : MovementController
{
    protected Vector2 defaultAccel;
    protected bool autoPickup = false;
    protected static float MinPickupSpeed = 5;
    [SerializeField] protected bool alwaysAutoPickup = false;

    protected override void Awake()
    {
        base.Awake();
        defaultAccel = accel;
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.EnableAutoPickup += EnableAutoPickup; 
        GameManager.Instance.DisableAutoPickup += DisableAutoPickup;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (autoPickup || alwaysAutoPickup)
        {
            accel = GameManager.Instance.VectorToPlayer(transform.position).normalized * accel.magnitude;
            movementDirection = GameManager.Instance.VectorToPlayer(transform.position);
            if (speed < MinPickupSpeed)
            {
                speed = MinPickupSpeed;
            }
        }
    }

    public virtual void EnableAutoPickup()
    {
        autoPickup = true;
    }

    protected virtual void DisableAutoPickup()
    {
        if (autoPickup)
        {
            ResetValues();
            autoPickup = false;
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        if (!alwaysAutoPickup)
        {
            autoPickup = GameManager.Instance.AutoPickup;
            SetMovementDirection(90);
            accel = defaultAccel;
        }
    }

    protected override void AccelMovement(Vector2 accel)
    {
        Vector2 movement = GetMovementVector();
        movement += accel * Time.deltaTime;
        if (!autoPickup && !alwaysAutoPickup)
            movement = ClampVector2(movement, speedMax);
        speed = movement.magnitude;
        movementDirection = movement.normalized;
    }
}