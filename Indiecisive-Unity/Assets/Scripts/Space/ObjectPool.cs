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

    [SerializeField]
    protected bool canExpand = false;

    // Create a list of GameObjects, instantiate them, and disable them
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

            //attach bullet movement script to newly created bullet
            obj.AddComponent<NormalBullet>();
            obj.AddComponent<BoxCollider2D>();
            obj.AddComponent<Rigidbody2D>();

            objects.Add(obj);
        }
    }

    // Pull an object from the list if it is not currently active
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].activeInHierarchy)
            {
                return objects[i];
            }
        }
        if (canExpand)
        {
            for (int i = 0; i < countToPool; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);

                //attach bullet movement script to newly created bullet
                obj.AddComponent<NormalBullet>();
                obj.AddComponent<BoxCollider2D>();
                obj.AddComponent<Rigidbody2D>();

                objects.Add(obj);
            }
        }
        return null;
    }
}
