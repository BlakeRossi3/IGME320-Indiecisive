using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SeekingBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 5.0f;

    [SerializeField]
    protected GameObject targetTransform;

    [SerializeField]
    protected float turnWeight = 10;

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
        rb.velocity = transform.forward * speed * Time.deltaTime;

        var targetRotation = Quaternion.LookRotation(targetTransform.transform.position - transform.position);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnWeight));

        // shoots the bullet straight down
        //transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);
        // try using the player transform for the seek

        // destroy when off screen--translate position to pixels, compare to screen height
        // ***TODO: make this destroy on exit of all sides of screen
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < -Screen.height / 50)
        {
            Destroy(gameObject);
        }
    }
}
