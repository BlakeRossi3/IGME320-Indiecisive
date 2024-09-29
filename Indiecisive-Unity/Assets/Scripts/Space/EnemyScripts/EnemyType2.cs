using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : Enemy
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
        TotalForce += Flee(targetPos);
        TotalForce += StayInBoundsForce() * boundsWeight;
    }

    protected override void ShootBullets()
    {
        throw new System.NotImplementedException();
    }
}
