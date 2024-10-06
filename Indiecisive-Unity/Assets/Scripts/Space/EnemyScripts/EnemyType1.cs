using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    protected Vector3 targetPos;

    [SerializeField]
    protected GameObject targetObject;

    [SerializeField]
    protected float boundsWeight;

    protected override void CalcSteeringForces()
    {
        seekPointCooldown -= Time.fixedDeltaTime;
        if (seekPointCooldown <= Random.Range(0f, 1f))
        {
            targetPos = WanderInZone();
            seekPointCooldown = seekPointDelay;
        }
        TotalForce += Seek(targetPos);
        TotalForce += Separate();
        TotalForce += StayInBoundsForce() * boundsWeight;
        //IgnoreCollisionsWithEnemies(enemyRB.GetComponent<Collider2D>());
    }

    protected override void ShootBullets()
    {
        fireCooldown -= Time.fixedDeltaTime;

        if (fireCooldown <= 0)
        {
            //creates a new bullet at the enemy's position TODO: update this based on how it looks with enemy sprite
            var newBullet = Instantiate(bulletPrefab, enemyRB.position, Quaternion.identity);

            //attaches bullet movement script to newly created bullet
            newBullet.AddComponent<NormalBullet>();
            newBullet.AddComponent<BoxCollider2D>();
            newBullet.AddComponent<Rigidbody2D>();
            fireCooldown = fireDelay;
        }
    }
}
