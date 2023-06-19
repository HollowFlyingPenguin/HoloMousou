using System.Collections;
using System.Collections.Generic;
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

    private void CreateObjectPool(MovementController controller)
    {
        string poolName = controller.name;
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

    private void ActivateObject(MovementController controller, SpawnData spawnData, Vector2 pos)
    {
        GameObject obj = controller.gameObject;
        obj.transform.position = pos + spawnData.spawnOffset;
        obj.SetActive(true);
    }

    public MovementController InitializeObject(SpawnData spawnData, Vector2 pos)
    {
        MovementController prefab = spawnData.controller;
        string poolName = prefab.name;
        if (objectPools.ContainsKey(poolName))
        {
            if (objectPools[poolName].Count > 0)
            {
                MovementController obj = objectPools[poolName].Dequeue();
                ActivateObject(obj, spawnData, pos);
                return obj;
            }
            else
            {
                MovementController newObj = Instantiate(prefab, transform);
                ActivateObject(newObj, spawnData, pos);
                return newObj;
            }
        }
        else
        {
            CreateObjectPool(prefab);
            return InitializeObject(spawnData, pos);
        }
    }

    public void ReturnObjectToPool(MovementController controller)
    {
        string poolName = controller.name;
        if (objectPools.ContainsKey(poolName))
        {
            controller.gameObject.SetActive(false);
            objectPools[poolName].Enqueue(controller);
        }
        else
        {
            Debug.LogWarning("Cannot return object to pool. Pool for " + poolName + " does not exist.");
        }
    }
}
