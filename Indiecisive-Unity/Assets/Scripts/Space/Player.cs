using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class Player : MonoBehaviour
{
    //Player stats TODO: tune these as necessary
    //Do we want damage as a player variable or bullet variable?
    private float moveSpeed = 7f;
    public float currentHealth = 5f;
    private float maxHealth = 5f;

    //TODO: temp variables until eventSystem is fixed
    private float bulletCount = 50; //REMOVE LATER
    public TextMeshProUGUI chargeText; //REMOVE LATER

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

    //Variable for determining active special TODO: retrieve this data from planetside
    [SerializeField]
    private int special = 0;

    //Other variables for handling special TODO: tune this
    //TODO: could read active special in start and adjust values based on that?
    private float specialDuration = 3;
    private float specialTime = 0;
    private float specialCD = 7;
    private float specialCDTimer = 0;
    private bool specialActive = false;

    //Holds a reference to the active special (not instantiated by script)
    private GameObject currentSpecial;

    //Objects that display status for special and shield
    [SerializeField]
    private GameObject specialStatus;
    [SerializeField]
    private GameObject shieldStatus;

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

        //TODO: TEMP TEMP TEMP REMOVE LATER
        chargeText.text = (": " + bulletCount);

        //TODO: check active special and adjust timers/duration as needed

        //Reads what the active special is and assigns the correct item
        switch (special)
        {
            case 0:
                currentSpecial = GameObject.Find("barrierSpecial");
                break;

            case 1:
                currentSpecial = GameObject.Find("beamSpecial");
                break;
        }

        //Hides the special
        currentSpecial.SetActive(false);
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

            //player shield
            playerShield();

            //player special
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
            // BLAKE CODE - adding charge functionality TODO: COMMENTED OUT FOR BUGS RN
            //GameManager gameManager = GameObject.Find("EventSystem").GetComponent<GameManager>();

            //gameManager.Charge -= 100;

            //TODO: update with gameManager data when available
            if (bulletCount > 0)
            {
                //creates a new bullet at the player's position TODO: update this based on how it looks with ship sprite
                var newBullet = Instantiate(bulletPrefab, playerRB.position, Quaternion.identity);

                //attaches components to newly created bullet
                newBullet.AddComponent<PlayerBullet>();
                newBullet.AddComponent<BoxCollider2D>();
                newBullet.AddComponent<Rigidbody2D>();

                //TODO: update this with gamemanager stuff later
                bulletCount--;
                chargeText.text = (": " + bulletCount); 
            }
        }
    }

    //Player special abilities. Uses a switch to determine which is active.
    private void playerSpecial()
    {
        //Special is activated when X is pressed and not on cooldown
        //While the current specials have similar mechanics, this switch is for future proofing with other specials that may act differently.
            switch (special)
            {
                //Damaging barrier around player
                case 0:

                    if (Input.GetKeyDown(KeyCode.X) && specialCDTimer <= 0)
                    {
                        //Makes the special object active
                        currentSpecial.SetActive(true);

                        //Sets use status to active
                        specialActive = true;
                        specialCDTimer = specialCD;

                        //Deactivates "ready" indicator
                        specialStatus.SetActive(false);
                    }

                    //updates object position
                    if (specialActive)
                    {
                        currentSpecial.transform.position = transform.position;
                    }

                    //Handles timers and deactivation
                    objectSpecialTimers();
                break;

                //Straight beam in front of player
                case 1:

                    if (Input.GetKeyDown(KeyCode.X) && specialCDTimer <= 0)
                    {
                        //Makes the special object active
                        currentSpecial.SetActive(true);

                        //Sets use status to active
                        specialActive = true;
                        specialCDTimer = specialCD;
                    }

                    if (specialActive)
                    {
                        currentSpecial.transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                    }

                    //Handles timers and deactivation
                    objectSpecialTimers();

                break;
            }

    }

    //Handles timer and activation for object-based specials
    private void objectSpecialTimers()
    {
        if (specialActive)
        {
            specialTime += (1 * Time.deltaTime);

            //Checks if time is up
            if (specialTime >= specialDuration)
            {
                specialActive = false;
                currentSpecial.SetActive(false);
                specialTime = 0;
            }

        }
        //Decreases cooldown if needed
        if (specialCDTimer > 0)
        {
            specialCDTimer -= 1 * Time.deltaTime;
        }

        //If special is available, activates indicator
        if (specialCDTimer <= 0)
        {
            specialStatus.SetActive(true);
        }
    }

    //player shield--nullifies damage for a set amount of time
    private void playerShield()
    {
        //Press C for shield when cd is zero
        if (Input.GetKeyDown(KeyCode.C) && shieldCDTimer <= 0)
        {
            //activates shield
            shieldActive = true;
            playerSprite.color = Color.blue;
            shieldCDTimer = shieldCD;

            //hides "ready" indicator
            shieldStatus.SetActive(false);
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
        }

        //if shield is available, displays "ready" indicator
        if (shieldCDTimer <= 0)
        {
            shieldStatus.SetActive(true);
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

                // Destroys the bullet that hit the player
                Destroy(collision.gameObject);
            }

            //Pooled enemy bullet -- 1 dmg
            if (collision.gameObject.CompareTag("EnemyBulletPooled"))
            {
                UnityEngine.Debug.Log("Player hit!");

                //Decreases health by 1
                currentHealth -= 1;

                // Deactivates the bullet that hit the player
                collision.gameObject.SetActive(false);
            }

            //Collision with Enemy -- 1 dmg
            if (collision.gameObject.CompareTag("Enemy"))
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
