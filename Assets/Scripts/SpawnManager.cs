using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected Player defaultPlayer;
    [SerializeField] protected float playerSpawnHeightRatio;
    [SerializeField] protected MovementController powerPrefab, scorePrefab, bigPowerPrefab, lifePrefab;

    // Distances for group pickup spawns
    [SerializeField] protected float minPickupDistance = 0, maxPickupDistance = 1;

    private static SpawnManager _instance;

    public static SpawnManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("SpawnManager is Null");
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
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Vector3 pos = GetPlayerSpawnPos();
        Player player = Instantiate(defaultPlayer, pos, Quaternion.identity);
        GameManager.Instance.SetPlayer(player);
    }

    public Vector3 GetPlayerSpawnPos()
    {
        float x = UIManager.Instance.GetGameCenter().x;
        float y = UIManager.Instance.GetMinGameY() + UIManager.Instance.GetGameSize().y * playerSpawnHeightRatio;
        Vector3 pos = new(x, y);
        return pos;
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
                    obj = MovementPoolManager.Instance.InitializeObject(powerPrefab);
                    obj.ResetValues();
                    obj.transform.position = centerPos;
                    return;

                case var data when data.powerCount == 0 && data.scoreCount == 1 && data.bigPowerCount == 0:
                    obj = MovementPoolManager.Instance.InitializeObject(scorePrefab);
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
            obj = MovementPoolManager.Instance.InitializeObject(lifePrefab);
            obj.ResetValues();
            obj.transform.position = centerPos;
            lifeCount--;
        }
        else if (pickupSpawnData.bigPowerCount == 1)
        {
            MovementController obj;
            obj = MovementPoolManager.Instance.InitializeObject(bigPowerPrefab);
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
            MovementController controller = MovementPoolManager.Instance.InitializeObject(prefab);
            controller.ResetValues();

            float angle = Mathf.Rad2Deg * (startAngle + i * angleStep);
            float distance = Random.Range(minPickupDistance, maxPickupDistance);
            controller.transform.position = centerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        }
    }
}