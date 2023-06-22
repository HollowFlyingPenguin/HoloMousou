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

    private void Start()
    {
        // Fixes an issue where controllers don't call ResetValue on game start which effects Timers and speed.
        MovementController[] initialControllers = FindObjectsByType<MovementController>(FindObjectsSortMode.None);
        foreach (var controller in initialControllers)
        {
            controller.ResetValues();
        }
    }

    public Vector2 GetPlayerPosition()
    {
        return player.transform.position;
    }

    public Vector2 VectorToPlayer(Vector2 pos)
    {
        return (Vector2) player.transform.position - pos;
    }

    public float AngleToPlayer(Vector2 pos)
    {
        Vector2 distance = (Vector2) player.transform.position - pos;
        var angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void Hit(Character character, float damage)
    {
        character.Hurt(damage);
    }
}
