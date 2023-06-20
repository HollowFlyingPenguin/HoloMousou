using UnityEngine;

public class Character : MovementController
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected SpawnData bulletSpawnData;
    protected float shotTimer = 0;
    protected bool canShoot = true;

    protected virtual void FixedUpdate()
    {
        if (bulletSpawnData)
        {
            CheckShotTimer();
        }
    }

    protected virtual void CheckShotTimer()
    {
        var delay = 1f / bulletSpawnData.fireRate;
        if (shotTimer < delay)
        {
            shotTimer += Time.fixedDeltaTime;
        }
        else
        {
            shotTimer = delay;
        }

        if (shotTimer >= delay && canShoot)
        {
            Shoot();
            shotTimer -= delay;
        }
    }

    public virtual void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Shoot()
    {
        if (bulletSpawnData)
        {
            ObjectPoolManager.Instance.InitializeObject(bulletSpawnData, transform.position);
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Dead");
    }
}