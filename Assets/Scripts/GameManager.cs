using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected MovementController powerPrefab, scorePrefab, bigPowerPrefab, lifePrefab;
    // Distances for group pickup spawns
    [SerializeField] protected float minPickupDistance = 0, maxPickupDistance = 1;
    [SerializeField] protected float power = 0, score = 0;
    [SerializeField] protected float powerPickupValue = 1, bigPowerPickupValue = 10, scorePickupValue = 500;

    public delegate void EnableAutoPickup();
    public event EnableAutoPickup OnEnableAutoPickup;
    public delegate void DisableAutoPickup();
    public event DisableAutoPickup OnDisableAutoPickup;
    public bool AutoPickup { get; private set; }

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
        power += scorePickupValue;
    }

    public void SetAutoPickup(bool autoPickup)
    {
        AutoPickup = autoPickup;
        if (autoPickup)
            OnEnableAutoPickup.Invoke();
        else
            OnDisableAutoPickup.Invoke();
    }

    public void SpawnPickup(Vector2 centerPos, PickupSpawnData pickupSpawnData)
    {
        int totalCount = pickupSpawnData.powerCount + pickupSpawnData.scoreCount;
        MovementController obj;
        // Single power ups
        if (totalCount == 1)
        {
            switch (pickupSpawnData)
            {
                case var data when data.powerCount == 1 && data.scoreCount == 0 && data.bigPowerCount == 0:
                    obj = ObjectPoolManager.Instance.InitializeObject(powerPrefab);
                    obj.ResetValues();
                    obj.transform.position = centerPos;
                    return;
                case var data when data.powerCount == 0 && data.scoreCount == 1 && data.bigPowerCount == 0:
                    obj = ObjectPoolManager.Instance.InitializeObject(scorePrefab);
                    obj.ResetValues();
                    obj.transform.position = centerPos;
                    return;
            }
        }
        GroupPickupSpawn(centerPos, pickupSpawnData);
    }

    private void GroupPickupSpawn(Vector2 centerPos, PickupSpawnData pickupSpawnData)
    {
        int powerCount = pickupSpawnData.powerCount;
        int scoreCount = pickupSpawnData.scoreCount;
        int bigPowerCount = pickupSpawnData.bigPowerCount;
        int lifeCount = pickupSpawnData.lifeCount;

        if (pickupSpawnData.lifeCount == 1)
        {
            MovementController obj;
            obj = ObjectPoolManager.Instance.InitializeObject(lifePrefab);
            obj.ResetValues();
            obj.transform.position = centerPos;
            lifeCount--;
        } else if (pickupSpawnData.bigPowerCount == 1)
        {
            MovementController obj;
            obj = ObjectPoolManager.Instance.InitializeObject(bigPowerPrefab);
            obj.ResetValues();
            obj.transform.position = centerPos;
            bigPowerCount--;
        }

        int totalCount = bigPowerCount + powerCount + scoreCount + lifeCount;
        float angleStep = 360 / totalCount;
        float startAngle = Random.Range(0, 360);
        int remainingCount = totalCount;
        for (int i = 0; i < totalCount; i++)
        {
            int selectionIndex = Random.Range(0, remainingCount);
            MovementController prefab;
            if (selectionIndex < powerCount)
            {
                prefab = powerPrefab;
                powerCount--;
            }
            else if (selectionIndex < powerCount + scoreCount)
            {
                prefab = scorePrefab;
                scoreCount--;
            }
            else if (selectionIndex < powerCount + scoreCount + bigPowerCount)
            {
                prefab = bigPowerPrefab;
                bigPowerCount--;
            }
            else
            {
                prefab = lifePrefab;
                lifeCount--;
            }
            remainingCount--;
            MovementController obj = ObjectPoolManager.Instance.InitializeObject(prefab);
            obj.ResetValues();

            float angle = Mathf.Rad2Deg * (startAngle + i * angleStep);
            float distance = Random.Range(minPickupDistance, maxPickupDistance);
            obj.transform.position = centerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        }
    }
}
