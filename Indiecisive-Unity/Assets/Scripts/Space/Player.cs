using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player stats TODO: tune these as necessary
    //Do we want damage as a player variable or bullet variable?
    private float moveSpeed = 7f;
    public float currentHealth = 5f;
    private float maxHealth = 5f;

    //variables for handling player game over status
    [HideInInspector]
    public bool isGameOver = false;
    [SerializeField]
    private SpriteRenderer playerSprite;

    //Used for movement and collision
    private Rigidbody2D playerRB;
    private Vector2 moveDirection = Vector2.zero;
    private Vector3 playerPixelPosition;

    //Attach prefab used to instantiate bullets
    public GameObject bulletPrefab;

    //Game window boundaries
    private float screenHeight = Screen.height;
    private float screenWidth = Screen.width;
    private Vector3 screenPixels;
    private Vector3 screenWorld;

    //health display objects
    private GameObject health1;
    private GameObject health2;
    private GameObject health3;
    private GameObject health4;
    private GameObject health5;

    //shield timer handling TODO: tune this
    private float shieldDuration = 1;
    private float shieldTime = 0;
    private float shieldCD = 5;
    private float shieldCDTimer = 0;
    private bool shieldActive = false;

    //list of health display objects
    private List<GameObject> health = new List<GameObject>();

    //Initializes elements
    void Start()
    {
        //connects the rigidbody assigned in the scene editor
        playerRB = GetComponent<Rigidbody2D>();

        //obtains the sprite renderer
        //TODO: this is used as a placeholder way of giving feedback. may remove later.
        playerSprite = GetComponent<SpriteRenderer>();

        //retrieving health display
        health1 = GameObject.Find("health1");
        health2 = GameObject.Find("health2");
        health3 = GameObject.Find("health3");
        health4 = GameObject.Find("health4");
        health5 = GameObject.Find("health5");

        //adds health display to list
        health.Add(health1);
        health.Add(health2);
        health.Add(health3);
        health.Add(health4);
        health.Add(health5);

        //gets the screen size in world points
        screenPixels = new Vector3 (screenWidth, screenHeight, 0);
        screenWorld = Camera.main.ScreenToWorldPoint(screenPixels);
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow player input if not in a game over state
        if (!isGameOver)
        {
            //player input for movement
            playerMovement();

            //handles boundary collision
            checkBounds();

            //basic player fire
            playerFire();

            //player special abilities
            playerSpecial();
        }

        //player feedback for gameover state
        //TODO: placeholder way of handling this. should be updated later on.
        if (isGameOver)
        {
            playerSprite.color = Color.red;

        }
    }

    //player input for movement. uses WASD keys.
    private void playerMovement()
    {
        //Checks for keyboard input, adds direction to Vector2
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection += Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection += Vector2.right;
        }

        //Checks if a key has been released and removes the corresponding vector--allows for smoother diagonal movement
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            moveDirection -= Vector2.up;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            moveDirection -= Vector2.left;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveDirection -= Vector2.down;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            moveDirection -= Vector2.right;
        }

        // If no direction key is held, stop the player
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && 
            !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector2.zero;
        }

        //applies moveDirection to player 
        if (moveDirection != Vector2.zero)
        {
            playerRB.MovePosition(playerRB.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    //automatically moves the player back if they go out of bounds
    //This is a workaround due to needing the ship to have a kinematic rigidbody instead of dynamic (physics issues)
    private void checkBounds()
    {
        if (transform.position.y > screenWorld.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, screenWorld.y, 0);
            transform.position = newPosition;
        }
        if (transform.position.y < - screenWorld.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, -screenWorld.y, 0);
            transform.position = newPosition;
        }
        if (transform.position.x < -screenWorld.x)
        {
            Vector3 newPosition = new Vector3(-screenWorld.x, transform.position.y, 0);
            transform.position = newPosition;
        }
        if (transform.position.x > screenWorld.x)
        {
            Vector3 newPosition = new Vector3(screenWorld.x, transform.position.y, 0);
            transform.position = newPosition;
        }
    }

    //fires basic bullet
    private void playerFire()
    {
        //Checks for if player has pressed the fire button
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //creates a new bullet at the player's position TODO: update this based on how it looks with ship sprite
            var newBullet = Instantiate(bulletPrefab, playerRB.position, Quaternion.identity);

            //attaches components to newly created bullet
            newBullet.AddComponent<PlayerBullet>();
            newBullet.AddComponent<BoxCollider2D>();
            newBullet.AddComponent<Rigidbody2D>();
        }
    }

    //player special abilities
    private void playerSpecial()
    {
        //Press C for shield when cd is zero
        if (Input.GetKeyDown(KeyCode.C) && shieldCDTimer <= 0)
        {
            //activates shield
            shieldActive = true;
            playerSprite.color = Color.blue;
            shieldCDTimer = shieldCD;
        }

        //handles shield timers
        if (shieldActive)
        {
            shieldTime += (1 * Time.deltaTime);

            //deactivates shield when time is up
            if (shieldTime >= shieldDuration)
            {
                shieldActive = false;
                playerSprite.color = Color.white;
                shieldTime = 0;
            }    
        }

        //decreases cooldown timer for shield
        if (shieldCDTimer > 0)
        {
            shieldCDTimer -= ( 1* Time.deltaTime);
            Debug.Log(shieldCDTimer);
        }
    }


    //checks if trigger collisions occur
    private void OnTriggerEnter2D(Collider2D collision)
    {
       //only handle collisions if player has health and no shield up
       if (currentHealth > 0 && !shieldActive)
       {
            //Generic enemy bullet -- 1 dmg
            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                UnityEngine.Debug.Log("Player hit!");

                //Decreases health by 1
                currentHealth -= 1;

            }

            //updates the health display 
            for (int i = 4; i > currentHealth - 1; i--)
            {

                if (i > currentHealth - 1)
                {
                    //hides a heart
                    health[i].SetActive(false);
                }
            }
       }


    }

}
