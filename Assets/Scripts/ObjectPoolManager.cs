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

    private void CreateObjectPool(MovementController prefab)
    {
        string poolName = prefab.name;
        if (!objectPools.ContainsKey(poolName))
        {
            objectPools[poolName] = new Queue<MovementController>();
        }
    }

    public MovementController InitializeObject(MovementController prefab)
    {
        string poolName = prefab.name;
        if (!objectPools.ContainsKey(poolName))
        {
            CreateObjectPool(prefab);
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
        if (!objectPools.ContainsKey(poolName))
        {
            CreateObjectPool(obj);
        }
        if (objectPools.ContainsKey(poolName))
        {
            obj.gameObject.SetActive(false);
            objectPools[poolName].Enqueue(obj);
        }
    }
}
