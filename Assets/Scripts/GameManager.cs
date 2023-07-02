using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected float power = 0, score = 0;
    [SerializeField] protected float powerPickupValue = 1, bigPowerPickupValue = 8, scorePickupValue = 100;
    [SerializeField] protected float bombBreakPoint1 = 0.5f, bombBreakPoint2 = 0.75f;

    public float BombBreakPoint1
    {
        get { return bombBreakPoint1; }
    }
    public float BombBreakPoint2
    {
        get { return bombBreakPoint2; }
    }

    //public delegate void EnableAutoPickup();
    //public event EnableAutoPickup OnEnableAutoPickup;
    //public delegate void DisableAutoPickup();
    //public event DisableAutoPickup OnDisableAutoPickup;

    public event UnityAction EnableAutoPickup;
    public event UnityAction DisableAutoPickup;

    public bool AutoPickup { get; private set; } = false;

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

    public void SetPlayer(Player player)
    {
        this.player = player;
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

    public void PickupPower()
    {
        power += powerPickupValue;
    }

    public void PickupBigPower()
    {
        power += bigPowerPickupValue;
    }

    public void PickupScore()
    {
        power += scorePickupValue;
    }

    public void PickupLife()
    {
        player.GainLife();
    }

    public void SetAutoPickup(bool autoPickup)
    {
        if (AutoPickup != autoPickup)
        {
            AutoPickup = autoPickup;
            if (autoPickup)
            {
                EnableAutoPickup?.Invoke();
            }
        }
    }

    // Used when the player gets hurt
    public void ResetPickupMovement()
    {
        DisableAutoPickup?.Invoke();
    }
}
