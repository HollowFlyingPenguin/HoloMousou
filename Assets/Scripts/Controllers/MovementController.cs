using PowerTools;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for anything that requires movement (mainly bullets). Inherited by player and enemies.
/// </summary>
public enum ObjectType
{
    Bullet,
    Enemy,
    Boss,
    Player,
    Pickup,
    Effect
}

public class MovementController : MonoBehaviour
{
    [SerializeField] protected ObjectType objectType;
    [SerializeField] protected bool active = true;
    [SerializeField] protected bool destroyOnAttack = true;
    [SerializeField] protected float baseSpeed = 0, speedMax = 100;
    [SerializeField] protected Vector2 accel = new(0, 0);
    [SerializeField] protected bool faceMoveDir = true;
    [SerializeField] protected float initialRotation = 0, rotationSpeed = 0;
    [SerializeField] protected float playerTrackTurnSpeed = 0;
    [SerializeField] private List<TimedEvent> timedEvents;
    [SerializeField] protected Vector2 movementDirection = new(0, 0);

    protected SpriteRenderer sprite;
    protected SpriteAnim spriteAnim;
    protected const float BoundCheckFreq = 0.5f, BulletBoundCheckOffset = 1, EnemyBoundCheckOffset = 1, TrackPlayerBoundOffset = 0.5f, PickupBoundCheckOffset = 0.5f;
    protected float speed = 0, boundCheckTimer = 0;
    protected bool grazed = false;
    protected Transform spawnOrigin;

    protected virtual void Awake()
    {
        spriteAnim = GetComponent<SpriteAnim>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        Move();

        boundCheckTimer += Time.deltaTime;
        if (boundCheckTimer >= BoundCheckFreq)
        {
            boundCheckTimer = 0;
            CheckBounds();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (playerTrackTurnSpeed != 0)
        {
            var playerAccel = GameManager.Instance.VectorToPlayer(transform.position).normalized;
            playerAccel *= playerTrackTurnSpeed;
            AccelMovement(playerAccel);
        }
        else if (!Equals(accel, new Vector2(0, 0)))
        {
            AccelMovement(accel);
        }
        if (rotationSpeed != 0)
        {
            if (faceMoveDir)
            {
                Debug.LogWarning("faceMoveDir and rotationSpeed are on " + name);
            }
            transform.rotation = Quaternion.Euler(0, 0, initialRotation + rotationSpeed * Time.time);
        }
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
    }

    protected virtual void StartEventTimers()
    {
        foreach (var eventData in timedEvents)
        {
            if (eventData.autoRepeat)
            {
                InvokeRepeating(eventData.methodName, eventData.delay, eventData.repeatRate);
            }
            else
            {
                Invoke(eventData.methodName, eventData.delay);
            }
        }
    }

    public virtual void ResetValues()
    {
        StartEventTimers();
        speed = baseSpeed;
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);
        grazed = false;
    }

    protected virtual void CheckBounds()
    {
        switch (objectType)
        {
            case ObjectType.Bullet:
                if (!UIManager.Instance.CheckInGameBounds(transform.position, BulletBoundCheckOffset))
                {
                    MovementPoolManager.Instance.ReturnObjectToPool(this);
                }
                break;

            case ObjectType.Enemy:
                if (!UIManager.Instance.CheckInGameBounds(transform.position, EnemyBoundCheckOffset))
                {
                    MovementPoolManager.Instance.ReturnObjectToPool(this);
                }
                break;

            case ObjectType.Pickup:
                if (transform.position.y < UIManager.Instance.GetMinGameY() - PickupBoundCheckOffset)
                {
                    MovementPoolManager.Instance.ReturnObjectToPool(this);
                }
                break;
        }
    }

    protected virtual void Move()
    {
        if (active)
        {
            movementDirection = movementDirection.normalized;
            if (playerTrackTurnSpeed != 0 && objectType == ObjectType.Bullet && !UIManager.Instance.CheckInGameBounds(transform.position, TrackPlayerBoundOffset))
            {
                MovementPoolManager.Instance.ReturnObjectToPool(this);
            }
            var velocity = movementDirection * speed;
            gameObject.transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
            if (faceMoveDir)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            }
        }
    }

    protected virtual void AccelMovement(Vector2 accel)
    {
        Vector2 movement = GetMovementVector();
        movement += accel * Time.deltaTime;
        movement = ClampVector2(movement, speedMax);
        speed = movement.magnitude;
        movementDirection = movement.normalized;
    }

    public virtual void ReturnToPool()
    {
        MovementPoolManager.Instance.ReturnObjectToPool(this);
    }

    public virtual void SetMovementDirection(Vector2 target)
    {
        movementDirection = target - (Vector2)transform.position;
    }

    public virtual void SetMovementDirection(float angle)
    {
        movementDirection = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
    }

    public virtual void SetGrazed(bool isGrazed)
    {
        grazed = isGrazed;
    }

    public void SetOrigin(Transform transform)
    {
        spawnOrigin = transform;
    }

    public virtual bool GetGrazed()
    {
        return grazed;
    }

    public virtual bool GetDestroyOnAttack()
    {
        return destroyOnAttack;
    }

    public virtual ObjectType GetObjectType()
    {
        return objectType;
    }

    protected Vector2 GetMovementVector()
    {
        Vector2 dir = movementDirection.normalized;
        var vector = dir * speed;
        return vector;
    }

    protected Vector2 ClampVector2(Vector2 vector, float max)
    {
        float magnitude = vector.magnitude;
        float clampedMagnitude = Mathf.Clamp(magnitude, 0, max);
        Vector2 clampedVector = vector.normalized * clampedMagnitude;
        return clampedVector;
    }

    public virtual void EventTest()
    {
        Debug.Log("Test");
    }
}