using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementPoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<MovementController>> objectPools = new Dictionary<string, Queue<MovementController>>();
    private static MovementPoolManager _instance;

    public static MovementPoolManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("MovementPoolManager is Null");
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
        MovementController obj;
        if (objectPools[poolName].Count > 0)
        {
            obj = objectPools[poolName].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
            obj.name = poolName;
        }
        obj.gameObject.SetActive(true);
        return obj;
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
