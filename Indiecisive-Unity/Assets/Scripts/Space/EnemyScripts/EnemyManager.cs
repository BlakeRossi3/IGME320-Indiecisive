using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    protected float spawnCount; // how many enemies to spawn in each batch

    [SerializeField]
    protected float spawnDelay; // how long to wait between batches of enemies
    private float tempSpawnDelay; // save the input spawnDelay

    [SerializeField]
    protected float initialSpawnDelay; // how long it takes for the first set of enemies to spawn

    [SerializeField]
    protected Enemy enemyPrefab;

    // list of all enemies
    [SerializeField]
    protected List<Enemy> enemies;

    public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }
    public float InitialSpawnDelay { get { return initialSpawnDelay; } set { initialSpawnDelay = value; } }


    // Start is called before the first frame update
    void Start()
    {
        tempSpawnDelay = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(initialSpawnDelay <= 0.0f)
        {
            if (spawnDelay <= 0.0f || enemies.Count <= 1)
            {
                // Spawn in some enemies
                Spawn();
                spawnDelay = tempSpawnDelay;
            }
            spawnDelay -= Time.deltaTime;
        }
        initialSpawnDelay -= Time.deltaTime;
    }

    // Spawn methods
    void Spawn()
    {
        float tempSpawnCount = spawnCount;
        spawnCount += enemies.Count;

        for (int i = enemies.Count; i < spawnCount; i++)
        {
            enemies.Add(Instantiate<Enemy>(enemyPrefab));

            // Set position
            Vector2 spawnPosition = Vector2.zero;

            if(Random.Range(0.0f, 1.0f) > 0.5f)
            {
                spawnPosition.x = Random.Range(-15f, -10f);
            }
            else
            {
                spawnPosition.x = Random.Range(10f, 15f);
            }
            spawnPosition.y = Random.Range(8f, 6f);

            enemies[i].transform.position = spawnPosition;

            enemies[i].manager = this;
            //enemies[i].IgnoreCollisionsWithEnemies(enemies[i].GetComponent<Collider2D>());
            enemies[i].FireDelay = Random.Range(enemies[i].FireDelay / 3, enemies[i].FireDelay);
        }
        spawnCount = tempSpawnCount;
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
