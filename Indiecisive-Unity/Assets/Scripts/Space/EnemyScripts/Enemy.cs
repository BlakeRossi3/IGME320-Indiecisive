using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static GameManager;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed = 5.0f; // velocity

    [SerializeField]
    protected float maxForce = 10.0f; // acceleration

    [SerializeField]
    protected float seperateRange = 1.0f;

    [SerializeField]
    protected float fireDelay; // delay between shots
    protected float fireCooldown = 1.0f; // delay of initial shot

    [SerializeField]
    protected float seekPointDelay; // delay for finding a new random point to seek
    protected float seekPointCooldown = 0.0f; // delay for initial point

    [SerializeField]
    protected float stayOnScreenCooldown; // how many points to wander to before leaving the screen
    protected float inStayCooldown;

    [SerializeField]
    protected GameObject player;

    [SerializeField]
    protected bool spawnedByManager; // enables / disables things depenent on an enemy manager

    protected Rigidbody2D enemyRB;
    protected BoxCollider2D enemyBoxCollider;
    protected Vector3 TotalForce = Vector3.zero;
    private SpriteRenderer enemySprite;

    public EnemyManager manager;

    private Vector3 cameraSize;
    private Vector3 screenMax = Vector3.zero;
    private Vector3 screenMin = Vector3.zero;

    //TODO: placeholder hp
    protected float currentHP = 2f;
    protected float maxHP = 2f;
    protected bool fleeing = false;
    private float flashPause = 0.1f;

    public float FireDelay { get { return fireDelay; } set { fireDelay = value; } }
    public Vector3 ScreenMax { get { return screenMax; } }
    public Vector3 ScreenMin { get { return screenMin; } }
    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
        enemyBoxCollider.isTrigger = true;

        inStayCooldown = stayOnScreenCooldown;

        player = GameObject.Find("Player");

        enemySprite = GetComponent<SpriteRenderer>();

        cameraSize.y = Camera.main.orthographicSize * 2f;
        cameraSize.x = cameraSize.y * Camera.main.aspect;
        screenMin = new Vector3(-(cameraSize.x / 2), cameraSize.y / 2, 0);
        screenMax = new Vector3(cameraSize.x / 2, -(cameraSize.y / 2), 0);

        SetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        TotalForce = Vector3.zero;

        CalcSteeringForces();

        enemyRB.position = transform.position;

        TotalForce = Vector3.ClampMagnitude(TotalForce, maxForce);

        enemyRB.AddForce(TotalForce);

        ShootBullets();

        if(enemySprite.color == Color.red)
        {
            flashPause -= 0.5f * Time.deltaTime;
            if(flashPause <= 0.0f)
            {
                enemySprite.color = Color.white;
                flashPause = 0.1f;
            }
        }
    }

    protected abstract void CalcSteeringForces();

    protected abstract void ShootBullets();

    protected abstract void SetHealth();

    /// <summary>
    /// Sets the direction of the velocity to the direction of targetPos
    /// relative to the enemy
    /// </summary>
    /// <param name="targetPos">Position of the point the enemy will go to</param>
    /// <returns>The force required for the enemy to move to targetPos</returns>
    protected Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - transform.position;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - (Vector3)enemyRB.velocity;

        // Return seek steering force
        return seekingForce;
    }

    protected Vector3 Seek(Transform target)
    {
        return Seek(target.transform.position);
    }

    /// <summary>
    /// Sets the direction of the velocity opposite to the direction
    /// of targetPos relative to the enemy
    /// </summary>
    /// <param name="targetPos">Position of the point the enemy will flee from</param>
    /// <returns>The force required for the enemy to move away from targetPos</returns>
    protected Vector3 Flee(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = transform.position - targetPos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 fleeingForce = desiredVelocity - (Vector3)enemyRB.velocity;

        // Return seek steering force
        return fleeingForce;
    }

    protected Vector3 Flee(Enemy target)
    {
        return Flee(target.transform.position);
    }

    protected Vector3 Separate()
    {
        // Sum of all flee forces to separate
        Vector3 separateForce = Vector3.zero;
        
        if(manager.Enemies.Count > 1)
        {
            // Go through all agents
            foreach (Enemy enemy in manager.Enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                // Checking if an agent is on top of itself or another
                if (Mathf.Epsilon < dist)
                {
                    separateForce += Flee(enemy) * (seperateRange / dist);
                }
            }
        }

        return separateForce;
    }

    /// <summary>
    /// Selects a random point in an area ahead of the enemy to Seek to
    /// </summary>
    /// <param name="time">How far ahead of the enemy the area will be</param>
    /// <param name="radius">How big the area will be</param>
    /// <returns>Seek to the random point calculated</returns>
    protected Vector3 Wander(float time, float radius)
    {
        Vector3 futurePos = CalcFuturePosition(time);
        float randAngle = Random.Range(0, Mathf.PI * 2); // picks a random direction in a circle around futurePos

        // calculate the point based on radius and direction
        Vector3 targetPos = futurePos;
        targetPos.x += Mathf.Cos(randAngle) * radius;
        targetPos.y += Mathf.Sin(randAngle) * radius;

        return Seek(targetPos);
    }

    public Vector3 CalcFuturePosition(float time)
    {
        return enemyRB.velocity * time + enemyRB.position;
    }

    /// <summary>
    /// Finds a random point within the designated range
    /// </summary>
    /// <returns>The random point found</returns>
    protected Vector3 WanderInZone()
    {
        Vector3 targetPos = Vector3.zero;

        targetPos.x = Random.Range(screenMin.x - screenMin.x / 3.0f, screenMax.x - screenMax.x / 3.0f);
        targetPos.y = Random.Range(screenMax.y - screenMax.y * 1.3f, screenMin.y - screenMin.y / 2.5f);

        return targetPos;
    }

    protected Vector3 WanderInZone(float min_X, float max_X, float min_Y, float max_Y)
    {
        Vector3 targetPos = Vector3.zero;

        targetPos.x = Random.Range(screenMin.x - screenMin.x / min_X, screenMax.x - screenMax.x / max_X);
        targetPos.y = Random.Range(screenMax.y - screenMax.y * min_Y, screenMin.y - screenMin.y / max_Y);

        return targetPos;
    }

    protected Vector3 StayInBoundsForce()
    {
        if (transform.position.x <= screenMin.x ||
            transform.position.x >= screenMax.x ||
            transform.position.y >= screenMin.y ||
            transform.position.y <= screenMax.y)
        {
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }

    //when trigger collisions (player bullets) hit an enemy
    //TODO: may move this to individual enemy scripts due to (assumed) hp differences.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks bullet type with tag
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // this will flash due to the code in update
            enemySprite.color = Color.red;
            currentHP -= 1;
            //destroys bullet that hit the enemy
            Destroy(collision.gameObject);
        }

        // if the enemy is fleeing the screen, don't allow collisions with the player
        if(!fleeing)
        {
            // feedback for collision with enemy
            if (collision.gameObject.CompareTag("Player"))
            {
                // this will flash due to the code in update
                enemySprite.color = Color.red;
                currentHP -= 1;
            }
        }

        //hi this is Julia adding code to enemy again
        //Handling for special attack collision
        //This is separate in case we change damage dealt by special (can also be split further if different specials deal different amounts of damage)
        if (collision.gameObject.CompareTag("PlayerSpecial"))
        {
            currentHP -= 1;
        }

        //destroys the enemy if health is at 0
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            if(spawnedByManager) { manager.Enemies.Remove(this); }
        }
    }
}
