using System.Collections;
using System.Collections.Generic;
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
    private float specialDuration = 3;
    private float specialTime = 0;
    private float specialCD = 7;
    private float specialCDTimer = 0;
    private bool specialActive = false;

    //Prefabs for special gameObjects
    public GameObject special0;
    public GameObject special1;

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
        //Vector3 position = new Vector3(-4, -4, 0);
        //chargeText.transform.position = position;
        
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
            playerShield();
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
        //GameObject array for specials generated
        //This is used as a workaround to check if a gameObject exists or not to delete when
        //the special is over, instead of having to check for each special individually.
        GameObject[] specialObject = new GameObject[1];

        //TODO: add this to update bc i forgor to do that with shield initially and was VERY CONFUSED
        //TODO: this needs a key input??? I dunno why I forgot to add one when writing this to begin with tbh
        switch (special)
        {
            //Damaging barrier around player
            case 0:
                //Generates special gameObject 
                var special = Instantiate(special0, playerRB.position, Quaternion.identity);

                //Adds collision tag to gameObject 
                special.gameObject.tag = "PlayerSpecial"; //TODO: seems to be bugging out if I do what I did for bullets. Will check this later, may not have updated tags properly in editor.

                //Sets special status to active
                specialActive = true;
                specialCDTimer = specialCD;
                break;
        }

        //Handles special timers if special is active
        if (specialActive)
        {
            //increments timer up
            specialTime += (1 * Time.deltaTime);

            //checks if time is up for special 
            if (specialTime >= specialDuration)
            {
                //deactivates special
                specialActive = false;

                //deletes special gameObject if applicable 
                if (specialObject[0] != null)
                {
                    Destroy(specialObject[0]);
                }
            }
        }

        //Handles tracking cooldown if special is inactive
        if (specialCDTimer > 0)
        {
            specialCDTimer -= (1 * Time.deltaTime);
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
