using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Player stats TODO: tune these as necessary
    private float moveSpeed = 5f;
    private float currentHealth = 5f;
    private float maxHealth = 5f;
    //Do we want damage as a player variable or bullet variable?

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


    //Initializes elements
    void Start()
    {
        //connects the rigidbody assigned in the scene editor
        playerRB = GetComponent<Rigidbody2D>();

        //gets the screen size in world points
        screenPixels = new Vector3 (screenWidth, screenHeight, 0);
        screenWorld = Camera.main.ScreenToWorldPoint(screenPixels);
    }

    // Update is called once per frame
    void Update()
    {
        //player input for movement
        playerMovement();

        //handles boundary collision
        checkBounds();

        //basic player fire
        playerFire();

    }

    //player input for movement. uses WASD keys.
    private void playerMovement()
    {
        //Checks for keyboard input, adds direction to Vector2
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection += Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection += Vector2.right;
        }

        //Checks if a key has been released and removes the corresponding vector--allows for smoother diagonal movement
        if (Input.GetKeyUp(KeyCode.W))
        {
            moveDirection -= Vector2.up;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            moveDirection -= Vector2.left;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            moveDirection -= Vector2.down;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            moveDirection -= Vector2.right;
        }

        // If no direction key is held, stop the player
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
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


    //checks if trigger collisions occur
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            UnityEngine.Debug.Log("Player hit!");
        }
    }

}
