using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MovementController
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected SpawnData bulletSpawnData;
    protected List<float> shotTimerList = new();
    protected bool canShoot = true, damageable = true;

    protected override void Start()
    {
        base.Start();
        if (bulletSpawnData)
        {
            for (int i = 0; i < bulletSpawnData.autoSpawnArray.Length; i++)
            {
                shotTimerList.Add(0);
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
        float timer = shotTimerList[index];
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
        shotTimerList[index] = timer;
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
        var bullet = ObjectPoolManager.Instance.InitializeObject(data, transform.position);
        bullet.ResetValues();
        GameObject obj = bullet.gameObject;

        obj.transform.position = (Vector2) transform.position + data.spawnOffset;

        float angle = 0;
        //obj.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        if (data.targetPlayer)
        {
            angle = GameManager.Instance.AngleToPlayer(transform.position);
        }
        angle += data.angleOffset + Random.Range(-data.randomAngleOffset / 2, data.randomAngleOffset / 2);
        angle += data.spinRate * Time.time;
        //obj.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward) * Quaternion.Euler(0, 0, spinSpeed * Time.time);

        bullet.SetMovementDirection(angle);
        obj.SetActive(true);
    }

    protected virtual void Die()
    {
        Debug.Log("Dead");
    }
}