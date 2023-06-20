using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected Player player;

    private static GameManager _instance;
    public static GameManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("GameManager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogWarning("No player detected");
        }
    }

    public Vector2 GetPlayerPosition()
    {
        return player.transform.position;
    }

    public void Hit(Character character, float damage)
    {
        character.Hurt(damage);
    }
}
