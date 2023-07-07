using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
    private static EffectsPoolManager _instance;

    public static EffectsPoolManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("EffectsPoolManager is Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void CreateObjectPool(GameObject prefab)
    {
        string poolName = prefab.name;
        if (!objectPools.ContainsKey(poolName))
        {
            objectPools[poolName] = new Queue<GameObject>();
        }
    }

    public GameObject InitializeObject(GameObject prefab)
    {
        string poolName = prefab.name;
        if (!objectPools.ContainsKey(poolName))
        {
            CreateObjectPool(prefab);
        }
        if (objectPools[poolName].Count > 0)
        {
            GameObject obj = objectPools[poolName].Dequeue();
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.name = poolName;
            return newObj;
        }
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        string poolName = obj.name;
        if (!objectPools.ContainsKey(poolName))
        {
            CreateObjectPool(obj);
        }
        if (objectPools.ContainsKey(poolName))
        {
            obj.SetActive(false);
            objectPools[poolName].Enqueue(obj);
        }
    }
}
