using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyType3 : Enemy
{
    [SerializeField]
    protected GameObject bulletPrefab; // sprite for bullets

    [SerializeField]
    protected Vector3 targetPos; // randomized position to wander from WanderInZone

    [SerializeField]
    protected Transform targetTransform; // object to seek

    [SerializeField]
    protected float boundsWeight; // how strong the force of the screen bounds is

    [SerializeField]
    protected bool firingEnabled = false;

    [SerializeField]
    protected float seekWeight; // how strong the forces of the points to seek are

    [SerializeField]
    protected float bulletNum; // how many bullets to fire in one spread

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

        if (stayOnScreenCooldown > 0)
        {
            seekPointCooldown -= Time.deltaTime;

            // slightly randomizes the time it takes for enemies to change where they are going
            if (seekPointCooldown <= Random.Range(0f, 1f))
            {
                targetPos = WanderInZone(7.0f, 7.0f, 1.6f, 3.0f);
                seekPointCooldown = seekPointDelay;
                stayOnScreenCooldown--;
            }
            TotalForce += Seek(targetPos) * seekWeight;
            TotalForce += Separate();
            //TotalForce += StayInBoundsForce() * boundsWeight;
            //IgnoreCollisionsWithEnemies(enemyRB.GetComponent<Collider2D>());
        }

        // Leave the screen after a bit so the enemies don't overflow the screen
        else
        {
            Vector3 exitPoint = new Vector3(0.0f, 5.0f, 0.0f);
            TotalForce = Flee(exitPoint);
            maxSpeed += 2.0f * Time.fixedDeltaTime;
            maxForce = 1.0f;
            enemyRB.freezeRotation = false;
            //transform.rotation = Quaternion.identity;
            firingEnabled = false;
            transform.localScale += new Vector3(-0.05f * Time.fixedDeltaTime, -0.05f * Time.fixedDeltaTime, 0.0f);

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
        if (firingEnabled == true)
        {
            fireCooldown -= Time.deltaTime;
        }

        // make sequential radial bullet spread
        if (fireCooldown <= 0)
        {
            float angleStep = 180.0f / bulletNum;
            Vector3 spreadCenter = (player.transform.position - enemyRB.transform.position).normalized;
            for (int i = 0; i < bulletNum; i++)
            {
                // Direction calculations
                float projectileDirX = Mathf.Sin(((angleStep * i * spreadCenter.x) * Mathf.PI) / 180);
                float projectileDirY = Mathf.Cos(((angleStep * i * spreadCenter.y) * Mathf.PI) / 180);

                // Create vectors
                Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);

                //creates a new bullet at the enemy's position TODO: update this based on how it looks with enemy sprite
                var newBullet = ObjectPool.instance.GetPooledObject();
                if (newBullet == null)
                {
                    return;
                }
                newBullet.transform.position = enemyRB.position;
                newBullet.GetComponent<NormalBullet>().FireAngle = projectileVector.normalized;
                newBullet.GetComponent<NormalBullet>().Speed = 2.0f;
                newBullet.SetActive(true);
            }

            fireCooldown = fireDelay;
        }
    }

    protected override void SetHealth()
    {
        currentHP = 4;
    }
}
