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
    protected Transform targetTransform; // object to seek

    [SerializeField]
    protected float boundsWeight; // how strong the force of the screen bounds is

    [SerializeField]
    protected bool firingEnabled = false;

    [SerializeField]
    protected float seekWeight; // how strong the forces of the points to seek are

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
            // Apply steering forces while the enemies are set to stay on screen
            MovementOnScreen();
        }
        else
        {
            // Leave the screen after a bit so the enemies don't overflow the screen
            LeaveScreen();
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
            //var newBullet = Instantiate(bulletPrefab, enemyRB.position, Quaternion.identity);
            var newBullet = ObjectPool.instance.GetPooledObject();
            if(newBullet == null)
            {
                return;
            }
            newBullet.transform.position = enemyRB.position;
            newBullet.GetComponent<NormalBullet>().FireAngle = Vector3.down;
            newBullet.SetActive(true);
            fireCooldown = fireDelay;
        }
    }

    private void MovementOnScreen()
    {
        seekPointCooldown -= Time.deltaTime;

        // slightly randomizes the time it takes for enemies to change where they are going
        if (seekPointCooldown <= Random.Range(0f, 1f))
        {
            targetPos = WanderInZone();
            seekPointCooldown = seekPointDelay;
            stayOnScreenCooldown--;
        }
        TotalForce += Seek(targetPos) * seekWeight;
        TotalForce += Separate();
        //TotalForce += StayInBoundsForce() * boundsWeight;
    }

    private void LeaveScreen()
    {
        Vector3 exitPoint = new Vector3(0.0f, 5.0f, 0.0f);
        TotalForce = Flee(exitPoint);
        maxSpeed += 2.0f * Time.fixedDeltaTime;
        maxForce = 1.5f;
        enemyRB.freezeRotation = false;
        if (TotalForce != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, -TotalForce.normalized);
        }
        firingEnabled = false;
        transform.localScale += new Vector3(-0.08f * Time.fixedDeltaTime, -0.08f * Time.fixedDeltaTime, 0.0f);
        enemyBoxCollider.enabled = false;

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
