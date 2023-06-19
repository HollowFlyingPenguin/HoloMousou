using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float damage = 1;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Character c = collision.transform.root.GetComponent<Character>();
        if (c)
        {
            Debug.Log("damage");
            GameManager.Instance.Hit(c, damage);
        }
    }
}
