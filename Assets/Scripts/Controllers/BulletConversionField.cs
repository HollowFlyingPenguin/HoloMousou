using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConversionField : PlayerAttack
{
    [SerializeField] protected float expansionSpeed = 1;

    private void Start()
    {
        transform.localScale = new Vector2(0, 0);
    }

    protected virtual void Update()
    {
        Vector2 scale = transform.localScale;
        scale.x += expansionSpeed * Time.deltaTime;
        scale.y += expansionSpeed * Time.deltaTime;
        transform.localScale = scale;
    }
}
