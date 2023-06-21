using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<MovementController>> objectPools = new Dictionary<string, Queue<MovementController>>();
    private static ObjectPoolManager _instance;

    public static ObjectPoolManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("ObjectPoolManager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void CreateObjectPool(IndividualSpawnData spawnData)
    {
        string poolName = spawnData.prefab.name;
        if (!objectPools.ContainsKey(poolName))
        {
            objectPools[poolName] = new Queue<MovementController>();

            //for (int i = 0; i < initialPoolSize; i++)
            //{
            //    MovementController newObj = Instantiate(prefab);
            //    newObj.SetActive(false);
            //    objectPools[poolName].Enqueue(newObj);
            //}
        }
    }

    //private void ActivateObject(MovementController controller, IndividualSpawnData spawnData, Vector2 pos)
    //{
    //    controller.ResetValues();
    //    GameObject obj = controller.gameObject;
    //    obj.transform.position = pos + spawnData.spawnOffset;

    //    float angle = 0;
    //    transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

    //    if (spawnData.targetPlayer)
    //    {
    //        angle = GameManager.Instance.AngleToPlayer(pos);
    //    }
    //    angle += spawnData.angleOffset + Random.Range(-spawnData.randomAngleOffset / 2, spawnData.randomAngleOffset / 2);
    //    controller.SetMovementDirection(angle);
    //    obj.SetActive(true);
    //}

    public MovementController InitializeObject(IndividualSpawnData spawnData, Vector2 pos)
    {
        MovementController prefab = spawnData.prefab;
        string poolName = prefab.name;
        if (!objectPools.ContainsKey(poolName))
        {
            CreateObjectPool(spawnData);
        }
        if (objectPools[poolName].Count > 0)
        {
            MovementController obj = objectPools[poolName].Dequeue();
            return obj;
        }
        else
        {
            MovementController newObj = Instantiate(prefab, transform);
            newObj.name = poolName;
            return newObj;
        }
    }

    public void ReturnObjectToPool(MovementController obj)
    {
        string poolName = obj.name;
        if (objectPools.ContainsKey(poolName))
        {
            obj.gameObject.SetActive(false);
            objectPools[poolName].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Cannot return object to pool. Pool for " + poolName + " does not exist.");
        }
    }
}
