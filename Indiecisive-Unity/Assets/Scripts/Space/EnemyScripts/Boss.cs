using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Boss : MonoBehaviour
{
    //Firing cooldowns for various bullet types
    float fireCD1 = 0;
    float fireCD2 = 3;
    float fireCD3 = 2;

    //todo: tune this
    float health = 100;

    //Boss parts
    private Rigidbody2D bossRB;
    private SpriteRenderer bossSprite;

    //Bullet types
    [SerializeField]
    private GameObject normalBullet;
    [SerializeField]
    private GameObject spreadBullet;
    [SerializeField]
    private GameObject seekingBullet;

    //Movement direction
    private bool moveLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        bossRB = GetComponent<Rigidbody2D>();
        bossSprite = GetComponent<SpriteRenderer>();
        bossSprite.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        ShootBullets();
        movement();

    }

    private void ShootBullets()
    {
        //reduces cds
        fireCD1 -= Time.deltaTime;
        fireCD2 -= Time.deltaTime;
        fireCD3 -= Time.deltaTime;

        //Normal Bullets
        if (fireCD1 <= 0)
        {
            //creates vector with randomized x value
            Vector3 newVector = new Vector3(Random.Range(bossRB.position.x - 3, bossRB.position.x + 3), bossRB.position.y - 1, 1);
            //creates a new bullet at the enemy's position
            var newBullet = Instantiate(normalBullet, newVector, Quaternion.identity);
            newBullet.AddComponent<NormalBullet>();
            newBullet.AddComponent<BoxCollider2D>();
            newBullet.AddComponent<Rigidbody2D>();
            newBullet.SetActive(true);
            fireCD1 = 0.5f;
        }

        if (fireCD2 <= 0)
        {
            //creates vector with randomized x value
            Vector3 newVector = new Vector3(Random.Range(bossRB.position.x - 3, bossRB.position.x + 3), bossRB.position.y - 1, 1);
            //creates a new bullet at the enemy's position 
            var newBullet = Instantiate(spreadBullet, newVector, Quaternion.identity);
            newBullet.AddComponent<SpreadBullet>();
            newBullet.AddComponent<BoxCollider2D>();
            newBullet.AddComponent<Rigidbody2D>();
            newBullet.SetActive(true);
            fireCD2 = 0.75f;
        }

        if (fireCD3 <= 0)
        {
            //creates vector with randomized x value
            Vector3 newVector = new Vector3(Random.Range(bossRB.position.x - 3, bossRB.position.x + 3), bossRB.position.y - 1, 1);
            //creates a new bullet at the enemy's position 
            var newBullet = Instantiate(seekingBullet, newVector, Quaternion.identity);
            newBullet.AddComponent<SeekingBullet>();
            newBullet.AddComponent<BoxCollider2D>();
            newBullet.AddComponent<Rigidbody2D>();
            newBullet.SetActive(true);
            fireCD3 = 0.5f;
        }


    }

    private void movement()
    {
        if (moveLeft)
        {
            //bossRB.MovePosition(bossRB.position + Vector2.left * 3 * Time.deltaTime);
            Vector3 newPosition = new Vector3(bossRB.position.x - (3 * Time.deltaTime), bossRB.position.y, 1);
            bossRB.position = newPosition;
        }
        else if (!moveLeft)
        {
            //bossRB.MovePosition(bossRB.position + Vector2.left * 3 * Time.deltaTime);
            Vector3 newPosition = new Vector3(bossRB.position.x + (3 * Time.deltaTime), bossRB.position.y, 1);
            bossRB.position = newPosition;
        }

        //changes movement direction if needed 
        if (bossRB.position.x > 6)
        {
            moveLeft = true;
        }
        if (bossRB.position.x < -6)
        {
            moveLeft = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks bullet type with tag
        if (collision.gameObject.CompareTag("PlayerBullet") || collision.gameObject.CompareTag("PlayerSpecial"))
        {
            health -= 1;
            //destroys bullet that hit the enemy
            Destroy(collision.gameObject);

            //destroys the enemy if health is at 0
            if (health <= 0)
            {
                Destroy(gameObject);

                //TODO: you win screen
            }
        }

    }
}
