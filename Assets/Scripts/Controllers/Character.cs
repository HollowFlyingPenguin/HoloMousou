using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for any object that takes damage or shoots bullets. Used by damageable bullets and things that spawn bullets. Inherited by player and enemies.
/// </summary>
public class Character : MovementController
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected SpawnData bulletSpawnData;
    [SerializeField] protected PickupSpawnData pickupSpawnData;
    protected List<float> bulletTimerList = new();
    protected int bulletPrefabIndex = 0;
    protected float spawnTime;
    protected bool canShoot = true, damageable = true;

    protected override void Start()
    {
        base.Start();
        if (bulletSpawnData)
        {
            for (int i = 0; i < bulletSpawnData.autoSpawnArray.Length; i++)
            {
                bulletTimerList.Add(0);
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bulletSpawnData)
        {
            for (int i = 0; i < bulletSpawnData.autoSpawnArray.Length; i++)
            {
                CheckShotTimer(i);
            }
        }
    }

    protected virtual void CheckShotTimer(int index)
    {
        IndividualSpawnData data = bulletSpawnData.autoSpawnArray[index];
        float fireRate = data.fireRate;
        float timer = bulletTimerList[index];
        float delay = 1f / fireRate;
        if (timer < delay)
        {
            timer += Time.fixedDeltaTime;
        }
        else
        {
            timer = delay;
        }

        if (timer >= delay && canShoot)
        {
            Shoot(data);
            timer -= delay;
        }
        bulletTimerList[index] = timer;
    }

    public virtual void Hurt(float damage)
    {
        if (damageable)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    protected virtual void Shoot(IndividualSpawnData data)
    {
        for (int i = 0; i < data.bulletCount; i++)
        {
            int arrayLength = data.prefabs.Length;
            if (arrayLength == 0)
            {
                Debug.LogWarning("No bullet in shoot for " + name);
                return;
            }
            MovementController prefab = data.prefabs[bulletPrefabIndex % arrayLength];
            bulletPrefabIndex++;

            MovementController bullet = ObjectPoolManager.Instance.InitializeObject(prefab);

            bullet.ResetValues();
            GameObject obj = bullet.gameObject;
            obj.transform.position = (Vector2)transform.position + data.spawnOffset;

            float angle = 0;

            if (data.targetPlayer)
            {
                angle = GameManager.Instance.AngleToPlayer(transform.position);
            }
            angle += data.angleOffset + Random.Range(-data.randomAngleOffset / 2, data.randomAngleOffset / 2);

            // Spin calculations
            var spinRate = data.spinRate;
            spinRate += data.spinAccel * (Time.time - spawnTime);
            spinRate = Mathf.Clamp(spinRate, data.spinMin, data.spinMax);
            spinRate = data.reverseSpin ? -spinRate : spinRate;
            angle += spinRate * (Time.time - spawnTime);

            if (data.bulletCount > 1)
            {
                if (data.autoSpace)
                {
                    angle += i * 360 / data.bulletCount;
                }
                else
                {
                    angle += (i * data.bulletSpread / data.bulletCount) - (data.bulletSpread / data.bulletCount);
                }
            }

            bullet.SetMovementDirection(angle);
            //obj.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward) * Quaternion.Euler(0, 0, spinSpeed * Time.time);
            obj.SetActive(true);
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        bulletPrefabIndex = 0;
        spawnTime = Time.time;
    }

    protected virtual void Die()
    {
        if (pickupSpawnData)
        {
            GameManager.Instance.SpawnPickup(transform.position, pickupSpawnData);
        }
        ObjectPoolManager.Instance.ReturnObjectToPool(this);
    }
}