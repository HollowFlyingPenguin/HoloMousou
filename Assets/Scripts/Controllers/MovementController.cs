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
    Pickup
}

public class MovementController : MonoBehaviour
{
    [SerializeField] protected ObjectType objectType;
    [SerializeField] protected bool active = true;
    [SerializeField] protected bool destroyOnAttack = true;
    [SerializeField] protected float baseSpeed = 0, acceleration = 0, speedMin = 0, speedMax = 100;
    [SerializeField] protected bool faceMoveDir = true;
    [SerializeField] protected float initialRotation = 0, rotationSpeed = 0;
    [SerializeField] protected float playerTrackTurnSpeed = 0;

    [SerializeField] private List<TimedEvent> timedEvents;

    protected Vector2 movementDirection = new(0, 0);
    protected const float BoundCheckFreq = 0.5f, BulletBoundCheckOffset = 1, EnemyBoundCheckOffset = 1, TrackPlayerBoundOffset = 0.5f, PickupBoundCheckOffset = 0.5f;
    protected float speed = 0, boundCheckTimer = 0;
    protected bool grazed = false;

    protected virtual void Awake()
    {
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
        if (acceleration != 0)
        {
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, speedMin, speedMax);
        }
        //else if (acceleration < 0)
        //{
        //    speed += acceleration * Time.deltaTime;
        //    Mathf.Clamp(speed, speedMin, speedMax);
        //    if (speed < 0)
        //    {
        //        acceleration *= -1;
        //        speed *= -1;
        //        movementDirection *= -1;
        //    }
        //}
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
                    ObjectPoolManager.Instance.ReturnObjectToPool(this);
                }
                break;
            case ObjectType.Enemy:
                if (!UIManager.Instance.CheckInGameBounds(transform.position, EnemyBoundCheckOffset))
                {
                    ObjectPoolManager.Instance.ReturnObjectToPool(this);
                }
                break;
            case ObjectType.Pickup:
                if (transform.position.y < UIManager.Instance.GetMinGameY() - PickupBoundCheckOffset)
                {
                    ObjectPoolManager.Instance.ReturnObjectToPool(this);
                }
                break;
        }
    }

    protected virtual void Move()
    {
        if (active)
        {
            movementDirection = movementDirection.normalized;
            if (playerTrackTurnSpeed != 0)
            {
                var vectorToPlayer = GameManager.Instance.VectorToPlayer(transform.position);
                vectorToPlayer = vectorToPlayer.normalized;
                if (speed < 0)
                {
                    vectorToPlayer *= -1;
                }
                //var moveAddition = vectorToPlayer.normalized * playerTrackTurnSpeed;
                //gameObject.transform.position += new Vector3(moveAddition.x, moveAddition.y, 0) * Time.deltaTime;
                movementDirection = Vector2.Lerp(movementDirection, vectorToPlayer, playerTrackTurnSpeed * Time.deltaTime);
                if (objectType == ObjectType.Bullet && !UIManager.Instance.CheckInGameBounds(transform.position, TrackPlayerBoundOffset))
                {
                    ObjectPoolManager.Instance.ReturnObjectToPool(this);
                }
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

    public virtual void EventTest()
    {
        Debug.Log("Test");
    }
}