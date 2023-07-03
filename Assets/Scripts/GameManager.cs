using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    protected int power = 0, score = 0, highscore = 0;
    [SerializeField] protected int powerPickupValue = 1, bigPowerPickupValue = 8, scorePickupValue = 100, grazeScoreValue = 100, playerDamageScoreValue = 10;
    [SerializeField] protected float bombBreakpoint1 = 0.5f, bombBreakpoint2 = 0.75f;
    [SerializeField] protected int[] powerBreakpoints = new int[4];

    public float BombBreakPoint1
    {
        get { return bombBreakpoint1; }
    }
    public float BombBreakPoint2
    {
        get { return bombBreakpoint2; }
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
        UpdatePower(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PickupPower();
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

    public void PlayerDamage(float damage, ShotType shotType)
    {
        UpdateScore(playerDamageScoreValue);
        // Increase score and Bomb
    }

    public void Hit(Character character, float damage)
    {
        character.Hurt(damage);
    }

    public void PickupPower()
    {
        UpdatePower(powerPickupValue);
    }

    public void PickupBigPower()
    {
        UpdatePower(bigPowerPickupValue);
    }

    public void PickupScore()
    {
        UpdateScore(scorePickupValue);
    }

    public void PickupLife()
    {
        player.GainLife();
    }

    public void Graze()
    {
        UpdateScore(grazeScoreValue);
    }

    protected void UpdatePower(int value)
    {
        power += value;
        power = Mathf.Clamp(power, 0, powerBreakpoints[^1]);
        var breakpoint = CalculatePowerBreakpoint();
        UIManager.Instance.UpdatePower(power, breakpoint);
    }

    protected int CalculatePowerBreakpoint()
    {
        int breakpoint = powerBreakpoints[0];
        if (power >= breakpoint)
        {
            int maxPower = powerBreakpoints[^1];
            if (power == maxPower)
            {
                breakpoint = maxPower;
            }
            else
            {
                int i = 1;
                while (power >= powerBreakpoints[i])
                {
                    i++;
                }
                breakpoint = powerBreakpoints[i];
            }
        }
        return breakpoint;
    }

    protected void UpdateScore(int value)
    {
        score += value;
        UIManager.Instance.UpdateScore(score);
        if (score > highscore)
        {
            highscore = score;
            UIManager.Instance.UpdateHighscore(highscore);
        }
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
