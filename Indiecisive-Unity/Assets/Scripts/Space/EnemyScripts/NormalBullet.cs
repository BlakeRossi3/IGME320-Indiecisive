using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0.5f;
    private Rigidbody2D rb;
    private BoxCollider2D collider;

    private Vector3 cameraSize;
    private Vector3 screenMax = Vector3.zero;
    private Vector3 screenMin = Vector3.zero;

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

        cameraSize.y = Camera.main.orthographicSize * 2f;
        cameraSize.x = cameraSize.y * Camera.main.aspect;
        screenMin = new Vector3(-(cameraSize.x / 2), cameraSize.y / 2, 0);
        screenMax = new Vector3(cameraSize.x / 2, -(cameraSize.y / 2), 0);
    }

    // Update is called once per frame
    void Update()
    {
        // shoots the bullet straight down
        transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);

        // destroy when off screen--translate position to pixels, compare to screen height
        if (transform.position.x <= screenMin.x ||
            transform.position.x >= screenMax.x ||
            transform.position.y >= screenMin.y ||
            transform.position.y <= screenMax.y)
        {
            Destroy(gameObject);
        }

        // TODO: make the bullet follow the rotation of the enemy
    }
}
