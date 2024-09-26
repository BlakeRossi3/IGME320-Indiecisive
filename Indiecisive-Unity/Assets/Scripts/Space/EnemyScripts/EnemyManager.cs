using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    float spawnCount = 10;

    [SerializeField]
    Enemy enemyPrefab;

    // list of all agents
    [SerializeField]
    protected List<Enemy> enemies;

    public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }


    // Start is called before the first frame update
    void Start()
    {
        // Spawn in some agents
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn methods
    void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            enemies.Add(Instantiate<Enemy>(enemyPrefab));

            enemies[i].manager = this;
        }
    }
}
