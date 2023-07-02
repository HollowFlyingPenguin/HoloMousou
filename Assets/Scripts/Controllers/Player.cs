using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] protected float pointOfCollectionRatio = 0.4f;
    [SerializeField] protected float slowPercent = 0.3f, edgeBoundOffsetX = 0.5f, edgeBoundOffsetY = 0.5f;
    [SerializeField] protected float recoverTime = 1;
    protected float recoverTimer = 0;
    protected PlayerInput input;
    protected Vector2 primaryInput;
    protected static float StickDiagonalZone = 0.3f;

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckPointOfCollection();
    }

    protected override void Update()
    {
        GetControllerInput();
        GetKeyboardInput();
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
        // Debug
        if (Input.GetKeyDown(KeyCode.H)) {
            Hurt(1);
        }
    }

    protected virtual void GetControllerInput()
    {
        movementDirection = input.actions["ControllerMove"].ReadValue<Vector2>();
        if (movementDirection.x != 0 && Mathf.Abs(movementDirection.x) > StickDiagonalZone)
        {
            movementDirection.x = Mathf.Sign(movementDirection.x);
        }
        else
        {
            movementDirection.x = 0;
        }
        if (movementDirection.y != 0 && Mathf.Abs(movementDirection.y) > StickDiagonalZone)
        {
            movementDirection.y = Mathf.Sign(movementDirection.y);
        }
        else
        {
            movementDirection.y = 0;
        }
    }

    protected virtual void GetKeyboardInput()
    {
        bool up = input.actions["Up"].IsPressed();
        bool down = input.actions["Down"].IsPressed();
        bool left = input.actions["Left"].IsPressed();
        bool right = input.actions["Right"].IsPressed();
        Vector2 inputDir = movementDirection;

        // Prioritizes the latest direction
        if (up && down)
        {
            inputDir.y = -primaryInput.y;
        }
        else
        {
            if (up)
            {
                inputDir.y = 1;
                primaryInput.y = 1;
            }
            else if (down)
            {
                inputDir.y = -1;
                primaryInput.y = -1;
            }
        }

        // Prioritizes the latest direction
        if (left && right)
        {
            inputDir.x = -primaryInput.x;
        }
        else
        {
            if (right)
            {
                inputDir.x = 1;
                primaryInput.x = 1;
            }
            else if (left)
            {
                inputDir.x = -1;
                primaryInput.x = -1;
            }
        }
        movementDirection = inputDir;
    }

    public override void Hurt(float damage)
    {
        base.Hurt(damage);
        if (damageable)
        {
            transform.position = SpawnManager.Instance.GetPlayerSpawnPos();
            GameManager.Instance.ResetPickupMovement();
            StartCoroutine(InvincibilityTimer());
        }
    }

    protected override void Die()
    {
        Debug.Log("Player Dead");
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

    protected virtual void CheckPointOfCollection()
    {
        float pointOfCollection = UIManager.Instance.GetMaxGameY() - pointOfCollectionRatio * UIManager.Instance.GetGameSize().y;
        if (transform.position.y >= pointOfCollection)
        {
            GameManager.Instance.SetAutoPickup(true);
        }
        else
        {
            GameManager.Instance.SetAutoPickup(false);
        }
    }

    public virtual void GainLife()
    {
        health += 1;
    }
}