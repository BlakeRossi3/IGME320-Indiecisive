using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class SeekingBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 6.0f;

    [SerializeField]
    protected float acceleration = 1.0f;

    [SerializeField]
    protected float stopSeekRadius = 5.0f;

    [SerializeField]
    protected float seekTime = 5.0f;

    [SerializeField]
    protected Transform targetTransform;

    private Vector3 targetVelocity;
    private SpriteRenderer sprite;

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

        targetTransform = GameObject.Find("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // create and apply the vector that steers the bullet towards the player
        Seek();

        // if the bullet leaves any side of the screen, destroy it
        DestroyOffScreen();
    }

    // moves the bullet towards the players position
    private void Seek()
    {
        if (targetVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, targetVelocity.normalized);
        }

        if (Vector3.Distance(transform.position, targetTransform.position) <= stopSeekRadius)
        {
            // adds the seeking force to the bullet
            rb.AddForce(targetVelocity * Time.deltaTime);

            seekTime -= 0.5f * Time.fixedDeltaTime;
            return;
        }

        // disable seeking and collisions after seekTime hits 0
        if(seekTime <= 0.0f)
        {
            // adds the seeking force to the bullet
            rb.AddForce(targetVelocity * 1.5f * Time.deltaTime);
            sprite.color = Color.gray;
            collider.enabled = false;

            return;
        }

        targetVelocity = (targetTransform.transform.position - transform.position).normalized * speed;

        targetVelocity -= (Vector3)rb.velocity;

        // adds the seeking force to the bullet
        rb.AddForce(targetVelocity * acceleration * Time.deltaTime);

        seekTime -= 0.5f * Time.fixedDeltaTime;
    }

    private void DestroyOffScreen()
    {
        // destroy when off screen--translate position to pixels, compare to screen height and width
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < -Screen.height / 50 ||
            screenPosition.y > Screen.height ||
            screenPosition.x < -Screen.width / 50 ||
            screenPosition.x > Screen.width)
        {
            Destroy(gameObject);
        }
    }
}
