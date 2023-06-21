using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] protected float slowPercent = 0.3f, edgeBoundOffsetX = 0.5f, edgeBoundOffsetY = 0.5f;

    protected PlayerInput input;
    [SerializeField] protected float recoverTime = 1;
    protected float recoverTimer = 0;

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
    }

    protected override void Update()
    {
        movementDirection = input.actions["Movement"].ReadValue<Vector2>();
        base.Update();

        canShoot = input.actions["Shoot"].IsPressed();
        bool justPressed = input.actions["Shoot"].WasPressedThisFrame();
        if (justPressed)
        {
            for (int i = 0; i < bulletSpawnData.autoSpawnArray.Length; i++)
            {
                CheckShotTimer(i);
            }
        }
    }

    public override void Hurt(float damage)
    {
        base.Hurt(damage);
        if (damageable)
            StartCoroutine(InvincibilityTimer());
    }

    IEnumerator InvincibilityTimer()
    {
        damageable = false;
        yield return new WaitForSeconds(recoverTime);
        damageable = true;
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
            var position = transform.position + new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
            position = new Vector2(
                Mathf.Clamp(position.x, UIManager.Instance.GetMinGameX() - edgeBoundOffsetX, UIManager.Instance.GetMaxGameX() + edgeBoundOffsetX),
                Mathf.Clamp(position.y, UIManager.Instance.GetMinGameY() - edgeBoundOffsetY, UIManager.Instance.GetMaxGameY() + edgeBoundOffsetY)
            );
            transform.position = position;
        }
    }
}