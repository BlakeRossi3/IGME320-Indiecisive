using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpreadBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 4.0f;
    private float initSpeed;

    [SerializeField]
    private float decceleration;

    [SerializeField]
    protected float explosionSize = 6.0f;

    [SerializeField]
    protected GameObject bulletPrefab;

    private Vector3 screenPosition;
    private Rigidbody2D rb;
    private BoxCollider2D collider;

    private List<GameObject> bulletList = new List<GameObject>();

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

        // saves initial speed
        initSpeed = speed;

        // sets a random decceleration for varying heights of explosion
        decceleration = Random.Range(1.5f, 2.5f) * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        // shoots the bullet straight down
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        Explode();

        // destroy when off screen--translate position to pixels, compare to screen height
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    protected void Explode()
    {
        if (speed > 0)
        {
            speed -= decceleration;
        }
        if (speed <= 0)
        {
            SpawnExplosionAtPos(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    protected void SpawnExplosionAtPos(Vector3 position)
    {
        float angleStep = 360.0f / explosionSize;

        for (int i = 0; i < explosionSize; i++)
        {
            // Direction calculations
            float projectileDirX = Mathf.Sin(((angleStep * i) * Mathf.PI) / 180);
            float projectileDirY = Mathf.Cos(((angleStep * i) * Mathf.PI) / 180);

            // Create vectors
            Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);

            var newBullet = ObjectPool.instance.GetPooledObject();
            if (newBullet == null)
            {
                return;
            }
            newBullet.transform.position = position;

            newBullet.GetComponent<NormalBullet>().FireAngle = projectileVector;
            newBullet.GetComponent<NormalBullet>().Speed = initSpeed;
            newBullet.SetActive(true);

            // used to be for deletion, keeping in case we want some other functionality
            //bulletList.Add(newBullet);
        }
    }
}
