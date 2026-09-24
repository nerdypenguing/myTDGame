using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolsize = 5;
    private List<GameObject> pool;

    void Start()
    {
        pool = new List<GameObject>();
        for(int i = 0; i < poolsize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }
        return CreateNewObject();
    }

}
