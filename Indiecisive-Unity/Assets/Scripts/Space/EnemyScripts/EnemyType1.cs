using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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

    [SerializeField]
    protected bool firingEnabled = false;

    protected override void CalcSteeringForces()
    {
        // make enable things when the enemy is inside the screen
        if (transform.position.x >= ScreenMin.x &&
           transform.position.x <= ScreenMax.x &&
           transform.position.y <= ScreenMin.y + 0.25f &&
           transform.position.y >= ScreenMax.y)
        {
            firingEnabled = true;
        }
        else
        {
            firingEnabled = false;
            TotalForce += Vector3.down * 0.1f;
        }

        if(stayOnScreenCooldown > 0)
        {
            seekPointCooldown -= Time.deltaTime;

            // slightly randomizes the time it takes for enemies to change where they are going
            if (seekPointCooldown <= Random.Range(0f, 1f))
            {
                targetPos = WanderInZone();
                seekPointCooldown = seekPointDelay;
                stayOnScreenCooldown--;
            }
            TotalForce += Seek(targetPos);
            TotalForce += Separate();
            //TotalForce += StayInBoundsForce() * boundsWeight;
            //IgnoreCollisionsWithEnemies(enemyRB.GetComponent<Collider2D>());
        }

        // Leave the screen after a bit so the enemies don't overflow the screen
        else
        {
            Vector3 exitPoint = new Vector3(0.0f, 5.0f, 0.0f);
            TotalForce = Flee(exitPoint);
            maxSpeed += 0.5f;
            enemyRB.freezeRotation = false;
            firingEnabled = false;
            transform.localScale += new Vector3(-0.0005f, -0.0005f, 0.0f);

            // attempt at a rotation change will look at later
            enemyRB.rotation = Mathf.Atan(Vector3.Normalize(TotalForce).x / Vector3.Normalize(TotalForce).y);

            // once the enemy gets far enough off screen, destroy
            if (transform.position.x < ScreenMin.x * 1.2f ||
               transform.position.x > ScreenMax.x * 1.2f ||
               transform.position.y > ScreenMin.y * 1.2f ||
               transform.position.y < ScreenMax.y * 1.2f)
            {
                Destroy(gameObject);
                manager.Enemies.Remove(this);
            }
        }
    }

    protected override void ShootBullets()
    {
        if(firingEnabled == true)
        {
            fireCooldown -= Time.deltaTime;
        }

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
