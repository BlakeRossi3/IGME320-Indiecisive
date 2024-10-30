using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField]
    protected GameObject prefab;

    [SerializeField]
    protected List<GameObject> objects;

    [SerializeField]
    protected int countToPool = 20;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        objects = new List<GameObject>();

        for (int i = 0; i < countToPool; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].activeInHierarchy)
            {
                return objects[i];
            }
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
