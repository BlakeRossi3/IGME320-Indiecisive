using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    protected GameObject wanderZone;

    [SerializeField]
    protected Vector3 targetPos;

    [SerializeField]
    protected GameObject targetObject;

    [SerializeField]
    protected float boundsWeight;

    protected override void CalcSteeringForces()
    {
        targetPos = WanderInZone(wanderZone);
        TotalForce += Seek(targetPos);
        TotalForce += StayInBoundsForce() * boundsWeight;
    }

    protected override void ShootBullets()
    {
        fireCooldown -= Time.deltaTime;

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
