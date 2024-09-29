using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed = 5.0f;

    [SerializeField]
    protected float maxForce = 10.0f;

    [SerializeField]
    protected float sperateRange = 1.0f;

    [SerializeField]
    protected float fireDelay = 5.0f;
    protected float fireCooldown = 1.0f;

    protected Rigidbody2D enemyRB;
    protected Vector3 TotalForce = Vector3.zero;

    public EnemyManager manager;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyRB.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        TotalForce = Vector3.zero;

        CalcSteeringForces();

        enemyRB.position = transform.position;

        TotalForce = Vector3.ClampMagnitude(TotalForce, maxForce);

        enemyRB.AddForce(TotalForce);
    }

    protected abstract void CalcSteeringForces();

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

    protected Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
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

}
