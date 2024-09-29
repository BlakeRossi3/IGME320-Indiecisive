using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed = 5.0f;

    [SerializeField]
    protected float maxForce = 10.0f;

    [SerializeField]
    protected float sperateRange = 1.0f;

    [SerializeField]
    protected bool shootsBullets;

    [SerializeField]
    protected float fireDelay = 5.0f;
    protected float fireCooldown = 0.5f;

    protected Rigidbody2D enemyRB;
    protected Vector3 TotalForce = Vector3.zero;

    public EnemyManager manager;

    private Vector3 cameraSize;
    private Vector3 screenMax = Vector3.zero;
    private Vector3 screenMin = Vector3.zero;

    public Vector3 ScreenMax { get { return screenMax; } }

    public Vector3 ScreenMin { get { return screenMin; } }

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyRB.isKinematic = true;

        cameraSize.y = Camera.main.orthographicSize * 2f;
        cameraSize.x = cameraSize.y * Camera.main.aspect;
        screenMin = new Vector3(-(cameraSize.x / 2), cameraSize.y / 2, 0);
        screenMax = new Vector3(cameraSize.x / 2, -(cameraSize.y / 2), 0);
    }

    // Update is called once per frame
    void Update()
    {
        TotalForce = Vector3.zero;

        CalcSteeringForces();

        enemyRB.position = transform.position;

        TotalForce = Vector3.ClampMagnitude(TotalForce, maxForce);

        enemyRB.AddForce(TotalForce);

        if(shootsBullets == true)
        {
            ShootBullets();
        }
    }

    protected abstract void CalcSteeringForces();

    protected abstract void ShootBullets();

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

    protected Vector3 Seek(Enemy target)
    {
        return Seek(target.transform.position);
    }

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

    //when trigger collisions (player bullets) hit an enemy
    //TODO: may move this to individual enemy scripts due to (assumed) hp differences.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks bullet type with tag
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            UnityEngine.Debug.Log("Enemy Hit!");

            //Destroy(gameObject);
        }
    }

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

    public Vector3 WanderInZone(GameObject wanderZone)
    {
        Vector3 targetPos = Vector3.zero;
        Vector3 zonePos = wanderZone.transform.position;
        Vector3 zoneScale = wanderZone.transform.lossyScale;

        targetPos.x = Random.Range(zonePos.x - zoneScale.x / 2, zonePos.x + zoneScale.x / 2);
        targetPos.y = Random.Range(zonePos.y - zoneScale.y / 2, zonePos.y + zoneScale.y / 2);

        return Seek(targetPos);
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
}
