using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorController : MovementController
{
    [SerializeField] protected SpriteRenderer chainSprite;
    protected float yOffset = 0, heightOffset = 0;

    protected override void Awake()
    {
        base.Awake();
        heightOffset = chainSprite.size.y - (chainSprite.transform.localPosition.y * 2);
    }

    protected override void Update()
    {
        base.Update();
        //SetRotation();
        SetChain();
    }

    protected void SetRotation()
    {
        Vector3 direction = transform.position - spawnOrigin.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, 0f, angle - 90);
    }

    protected void SetChain()
    {
        if (spawnOrigin)
        {
            float distance = (spawnOrigin.position - transform.position).magnitude;
            chainSprite.transform.localPosition = new Vector2(0, distance / 2);
            chainSprite.size = new Vector2(chainSprite.size.x, distance + heightOffset);
        }
    }
}
