using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the player moves
    public float interactionRadius = 2f;  // Radius within which the player can interact with the ship
    public GameObject spaceship;  // Reference to the spaceship
    public TextMeshProUGUI interactionText;  // Reference to the TextMeshPro text element for interaction
    public Vector3 textOffset = new Vector3(0, 1, 0);  // Offset to position the text above the player's head

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private string currentDirection;  // Track which direction is currently active
    private string queuedDirection;   // Track which direction is pressed next

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = null;
        queuedDirection = null;

        // Ensure interactionText is hidden at the start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandleMovementInput();
        HandleInteraction();
    }

    void HandleMovementInput()
    {
        // Handle directional inputs, prioritize first pressed, and queue secondary direction
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.up;
                currentDirection = "up";
            }
            else
            {
                queuedDirection = "up";
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.left;
                currentDirection = "left";
            }
            else
            {
                queuedDirection = "left";
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.down;
                currentDirection = "down";
            }
            else
            {
                queuedDirection = "down";
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.right;
                currentDirection = "right";
            }
            else
            {
                queuedDirection = "right";
            }
        }

        // Handle key releases and switch to queued direction if needed
        if (Input.GetKeyUp(KeyCode.W) && currentDirection == "up")
        {
            HandleDirectionRelease("up");
        }
        else if (Input.GetKeyUp(KeyCode.A) && currentDirection == "left")
        {
            HandleDirectionRelease("left");
        }
        else if (Input.GetKeyUp(KeyCode.S) && currentDirection == "down")
        {
            HandleDirectionRelease("down");
        }
        else if (Input.GetKeyUp(KeyCode.D) && currentDirection == "right")
        {
            HandleDirectionRelease("right");
        }

        // If no direction key is held, stop the player
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            moveInput = Vector2.zero;
            currentDirection = null;
            queuedDirection = null;
        }
    }

    void HandleDirectionRelease(string releasedDirection)
    {
        if (currentDirection == releasedDirection)
        {
            currentDirection = null;

            // If a direction is queued and the corresponding key is still held, switch to that direction
            if (queuedDirection != null)
            {
                if (queuedDirection == "up" && Input.GetKey(KeyCode.W))
                {
                    moveInput = Vector2.up;
                    currentDirection = "up";
                }
                else if (queuedDirection == "left" && Input.GetKey(KeyCode.A))
                {
                    moveInput = Vector2.left;
                    currentDirection = "left";
                }
                else if (queuedDirection == "down" && Input.GetKey(KeyCode.S))
                {
                    moveInput = Vector2.down;
                    currentDirection = "down";
                }
                else if (queuedDirection == "right" && Input.GetKey(KeyCode.D))
                {
                    moveInput = Vector2.right;
                    currentDirection = "right";
                }
            }

            queuedDirection = null; // Clear the queue after processing
        }
    }

    void FixedUpdate()
    {
        // Apply movement based on input and moveSpeed
        if (moveInput != Vector2.zero)  // Only move if there's a direction to move in
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void HandleInteraction()
    {
        // Check if the player is within interaction radius of the spaceship
        if (spaceship != null)
        {
            float distanceToShip = Vector2.Distance(transform.position, spaceship.transform.position);

            if (distanceToShip <= interactionRadius)
            {
                // Show the interaction prompt and move it above the player's head
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "Press E to interact";

                    // Update the position of the text above the player's head
                    Vector3 worldPosition = transform.position + textOffset;
                    worldPosition.z = 1;
                    interactionText.transform.position = worldPosition;
                }

                // Check if the player presses 'E' to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractWithShip();
                }
            }
            else
            {
                // Hide the interaction prompt if the player is too far
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(false);
                }
            }
        }
    }

    void InteractWithShip()
    {
        // Your interaction logic here
        Debug.Log("Player interacted with the spaceship");
    }
}