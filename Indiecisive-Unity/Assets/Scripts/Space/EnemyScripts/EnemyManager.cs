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

    // list of all enemies
    [SerializeField]
    protected List<Enemy> enemies;

    public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }


    // Start is called before the first frame update
    void Start()
    {
        // Spawn in some enemies
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

            // Set position
            Vector2 spawnPosition = Vector2.zero;

            spawnPosition.x = Random.Range(-5f, 5f);
            spawnPosition.y = Random.Range(4f, 1f);

            enemies[i].transform.position = spawnPosition;

            enemies[i].manager = this;
            //enemies[i].IgnoreCollisionsWithEnemies(enemies[i].GetComponent<Collider2D>());
            enemies[i].FireDelay = Random.Range(0.8f, enemies[i].FireDelay);
        }
    }

    public Enemy FindClosest(GameObject inEnemy)
    {
        float minDist = Mathf.Infinity;
        Enemy nearest = null;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(inEnemy.transform.position, enemy.transform.position);

            if (dist < minDist)
            {
                nearest = enemy;
                minDist = dist;
            }
        }

        return nearest;
    }
}
