using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ReturnBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 6.0f;

    private Vector3 screenPosition;
    private Rigidbody2D rb;
    private BoxCollider2D collider;

    void Start()
    {
        //Tags the bullet for collision purposes
        gameObject.tag = "EnemyBullet";
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        //setting the bullet as a trigger allows it to pass through objects but trigger collision detection
        collider.isTrigger = true;

        //Sets gravity scale to 0
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // shoots the bullet straight down
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        speed -= 0.005f;

        // destroy when off screen--translate position to pixels, compare to screen height
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
