using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 5.0f;

    private Vector3 screenPosition;
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Vector3 fireAngle = Vector2.down;

    public Vector3 FireAngle { get { return fireAngle; } set {  fireAngle = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    

    void Start()
    {
        //Tags the bullet for collision purposes
        gameObject.tag = "EnemyBulletPooled";
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
        transform.Translate(fireAngle * speed * Time.deltaTime);

        // destroy when off screen--translate position to pixels, compare to screen height
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < -Screen.height / 50 ||
            screenPosition.y > Screen.height ||
            screenPosition.x < -Screen.width / 50 ||
            screenPosition.x > Screen.width)
        {
            gameObject.SetActive(false);
        }
    }
}
