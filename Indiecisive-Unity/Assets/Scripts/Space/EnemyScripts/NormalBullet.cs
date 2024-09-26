using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        // shoots the bullet straight down
        transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);

        // TODO: make the bullet follow the rotation of the enemy

        // TODO: destroy when off screen--translate position to pixels, compare to screen height
    }
}
