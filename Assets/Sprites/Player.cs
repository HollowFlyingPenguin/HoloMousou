using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    protected PlayerInput input;

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
    }

    protected override void Update()
    {
        base.Update();
        movementDirection = input.actions["Movement"].ReadValue<Vector2>();
    }
}