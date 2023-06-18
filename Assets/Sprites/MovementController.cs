using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] protected float speed = 0;
    [SerializeField] protected Vector2 movementDirection = new(0, 0);

    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        var velocity = movementDirection.normalized * speed;
        gameObject.transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;

    }
}