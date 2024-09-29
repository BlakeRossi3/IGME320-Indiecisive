using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField]
    protected GameObject normalBulletPrefab;

    [SerializeField]
    protected Vector3 enemyPos;

    protected override void CalcSteeringForces()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if(fireCooldown <= 0)
        {
            //creates a new bullet at the enemy's position TODO: update this based on how it looks with enemy sprite
            var newBullet = Instantiate(normalBulletPrefab, enemyPos, Quaternion.identity);

            //attaches bullet movement script to newly created bullet
            newBullet.AddComponent<NormalBullet>();
            newBullet.AddComponent<BoxCollider2D>();
            newBullet.AddComponent<Rigidbody2D>();
            fireCooldown = fireDelay;
        }
    }
}
