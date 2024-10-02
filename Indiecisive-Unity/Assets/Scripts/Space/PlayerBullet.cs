using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    
    //Bullet stats TODO: tune these as necessary
    private float speed = 3f;
    private Vector3 screenPosition;
    private BoxCollider2D collider;
    private Rigidbody2D rb;

    void Start()
    {
        //Adds a tag to the object (used for collision)
        gameObject.tag = "PlayerBullet";

        //setting the bullet as a trigger allows it to pass through objects but trigger collision detection
        collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;

        //ensures gravity is zero for bullets
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    // Update is called once per frame
    void Update()
    {

        //moves bullet upwards each frame
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

        //gets current position in pixels
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        //if the bullet is offscreen, destroys it
        if (screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }


}
