using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] protected float slowPercent = 0.3f;

    protected PlayerInput input;

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
    }

    protected override void Update()
    {
        movementDirection = input.actions["Movement"].ReadValue<Vector2>();
        base.Update();

        var shoot = input.actions["Shoot"].IsPressed();
        if (shoot)
        {
            Shoot();
        }
    }

    protected override void Move()
    {
        if (active)
        {
            var velocity = movementDirection.normalized * speed;
            var isSlow = input.actions["Slow"].IsPressed();
            if (isSlow)
            {
                velocity *= slowPercent;
            }
            gameObject.transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
        }
    }
}