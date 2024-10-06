using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField]
    protected GameObject bulletPrefab; // sprite for bullets

    [SerializeField]
    protected Vector3 targetPos; // randomized position to wander from WanderInZone

    [SerializeField]
    protected GameObject targetObject; // object to seek

    [SerializeField]
    protected float boundsWeight; // how strong the force of the screen bounds is

    protected override void CalcSteeringForces()
    {
        seekPointCooldown -= Time.fixedDeltaTime;

        // slightly randomizes the time it takes for enemies to change where they are going
        if (seekPointCooldown <= Random.Range(0f, 5f))
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
