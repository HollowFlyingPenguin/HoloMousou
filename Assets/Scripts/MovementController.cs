using UnityEngine;

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
    [SerializeField] protected float speed = 0;
    [SerializeField] protected Vector2 movementDirection = new(0, 0);
    [SerializeField] protected bool faceMoveDir = false;

    protected const float BoundCheckFreq = 1, BulletBoundCheckOffset = 0, EnemyBoundCheckOffset = 0;
    protected float boundCheckTimer = 0;

    protected virtual void Awake()
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
}