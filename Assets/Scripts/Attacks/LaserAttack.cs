using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    protected static float hitTickRate = 0.016f;
    [SerializeField] protected float damage = 1;
    protected List<CollisionData> collidedEnemies = new();
    protected bool isDamaging = false;
    protected struct CollisionData
    {
        public Character character;
        public Collider2D collider;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Character hitChara = collision.GetComponentInParent<Character>();
        if (hitChara)
        {
            CollisionData collisionData = new()
            {
                character = hitChara,
                collider = collision
            };
            collidedEnemies.Add(collisionData);
            if (!isDamaging)
            {
                StartDamageOverTime();
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        Character hitChara = collision.GetComponentInParent<Character>();
        if (hitChara)
        {
            foreach (CollisionData collisionData in collidedEnemies)
            {
                if (collisionData.collider == collision)
                {
                    collidedEnemies.Remove(collisionData);
                    break;
                }
            }

            if (collidedEnemies.Count == 0)
            {
                StopDamageOverTime();
            }
        }
    }

    private void StartDamageOverTime()
    {
        isDamaging = true;
        StartCoroutine(DamageOverTime());
    }

    private void StopDamageOverTime()
    {
        isDamaging = false;
        StopCoroutine(DamageOverTime());
    }

    private IEnumerator DamageOverTime()
    {
        while (isDamaging)
        {
            for (int i = collidedEnemies.Count - 1; i >= 0; i--)
            {
                CollisionData collisionData = collidedEnemies[i];
                if (!collisionData.character.gameObject.activeSelf)
                {
                    collidedEnemies.RemoveAt(i);
                    continue;
                }

                if (collisionData.collider.gameObject.layer == LayerMask.NameToLayer("GrazeHurtbox"))
                {
                    GameManager.Instance.Graze();
                }
                else
                {
                    OnHit(collisionData.character);
                }
            }

            if (collidedEnemies.Count == 0)
            {
                StopDamageOverTime();
                yield break;
            }

            yield return new WaitForSeconds(hitTickRate);

        }
    }

    protected virtual void OnHit(Character hitChara)
    {
        GameManager.Instance.Hit(hitChara, damage);
    }
}
