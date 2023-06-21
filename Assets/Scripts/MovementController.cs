using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ObjectType
{
    Bullet,
    Enemy,
    Boss,
    Player,
}

public class MovementController : MonoBehaviour
{
    [SerializeField] protected ObjectType type;
    [SerializeField] protected bool active = true;
    [SerializeField] protected float baseSpeed = 0, acceleration = 0, speedMin = 0, speedMax = 100;
    [SerializeField] protected bool faceMoveDir = true;
    [SerializeField] protected float initialRotation = 0, rotationSpeed = 0;
    [SerializeField] List<TimedEvent> timedEvents;

    protected Vector2 movementDirection = new(0, 0);
    protected const float BoundCheckFreq = 1, BulletBoundCheckOffset = 1, EnemyBoundCheckOffset = 1;
    protected float speed = 0, boundCheckTimer = 0;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        speed = baseSpeed;
        StartEventTimers();
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
            Mathf.Clamp(speed, speedMin, speedMax);
        }
        if (rotationSpeed != 0)
        {
            if (faceMoveDir)
            {
                Debug.LogWarning("faceMoveDir and rotationSpeed are on");
            }
            transform.rotation = Quaternion.Euler(0, 0, initialRotation + rotationSpeed * Time.time);
        }
    }

    protected virtual void StartEventTimers()
    {
        foreach (var eventData in timedEvents)
        {
            Invoke(eventData.methodName, eventData.delay);
        }
    }

    public virtual void ResetValues()
    {
        speed = baseSpeed;
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);
    }

    protected virtual void CheckBounds()
    {
        if (type == ObjectType.Bullet)
        {
            if (!UIManager.Instance.CheckInGameBounds(transform.position, BulletBoundCheckOffset))
            {
                ObjectPoolManager.Instance.ReturnObjectToPool(this);
            }
        }
        else if (type == ObjectType.Enemy)
        {
            if (!UIManager.Instance.CheckInGameBounds(transform.position, EnemyBoundCheckOffset))
            {
                ObjectPoolManager.Instance.ReturnObjectToPool(this);
            }
        }
    }

    protected virtual void Move()
    {
        if (active)
        {
            var velocity = movementDirection.normalized * speed;
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
}