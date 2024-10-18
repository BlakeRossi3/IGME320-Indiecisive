using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the player moves
    public float interactionRadius = 5f;  // Radius within which the player can interact with the ship
    public TextMeshProUGUI interactionText;  // Reference to the TextMeshPro text element for interaction
    public Vector3 textOffset = new Vector3(0, 1, 0);  // Offset to position the text above the player's head
    public GameObject[] interactables; // Objects the player my interact with

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public string currentDirection;  // Track which direction is currently active
    private string queuedDirection;   // Track which direction is pressed next
    public bool textSwitch;
    int objectNum = -1;

    // Sprites for each direciton and spriteRenderer
    private SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    public bool shovel;
    public int coins;
    public Button myButton;
    public GameObject uiPanel;
    private bool uiToggle =false;



    void Start()
    {
        uiPanel.SetActive(false);
        coins = 0;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentDirection = null;
        queuedDirection = null;
        rb.freezeRotation = true;
        

        // hide interaction text
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.up;
                currentDirection = "up";
                spriteRenderer.sprite = upSprite;
            }
            else
            {
                queuedDirection = "up";
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.left;
                currentDirection = "left";
                spriteRenderer.sprite = leftSprite;
            }
            else
            {
                queuedDirection = "left";
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.down;
                currentDirection = "down";
                spriteRenderer.sprite = downSprite;
            }
            else
            {
                queuedDirection = "down";
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentDirection == null)
            {
                moveInput = Vector2.right;
                currentDirection = "right";
                spriteRenderer.sprite = rightSprite;
            }
            else
            {
                queuedDirection = "right";
            }
        }

        // Handle key releases and switch to queued direction if needed
        if (Input.GetKeyUp(KeyCode.UpArrow) && currentDirection == "up")
        {
            HandleDirectionRelease("up");
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) && currentDirection == "left")
        {
            HandleDirectionRelease("left");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) && currentDirection == "down")
        {
            HandleDirectionRelease("down");
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) && currentDirection == "right")
        {
            HandleDirectionRelease("right");
        }

        // If no direction key is held, stop the player
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;
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
                if (queuedDirection == "up" && Input.GetKey(KeyCode.UpArrow))
                {
                    moveInput = Vector2.up;
                    currentDirection = "up";
                    spriteRenderer.sprite = upSprite;
                }
                else if (queuedDirection == "left" && Input.GetKey(KeyCode.LeftArrow))
                {
                    moveInput = Vector2.left;
                    currentDirection = "left";
                    spriteRenderer.sprite = leftSprite;
                }
                else if (queuedDirection == "down" && Input.GetKey(KeyCode.DownArrow))
                {
                    moveInput = Vector2.down; 
                    currentDirection = "down";
                    spriteRenderer.sprite = downSprite;
                }
                else if (queuedDirection == "right" && Input.GetKey(KeyCode.RightArrow))
                {
                    moveInput = Vector2.right;
                    currentDirection = "right";
                    spriteRenderer.sprite = rightSprite;
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
        
        if (textSwitch == false)
        {
            // Loop through all the potential interactable objects
            for (int i = 0; i < interactables.Length; i++)
            {
                objectNum = i;
                float distanceToObject = Vector2.Distance(transform.position, interactables[i].transform.position);

                // Check if the player is within interaction radius of the object
                if (distanceToObject <= interactionRadius)
                {
                    // Show the interaction prompt and move it above the player's head
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "C to Interact";
                    textSwitch = true;
                   
                    break;                    
                }
            }
        }

        

        float distanceToObject2 = Vector2.Distance(transform.position, interactables[objectNum].transform.position);

        if (Input.GetKeyDown(KeyCode.C) && distanceToObject2 < interactionRadius)
        {
            Interact(objectNum);
        }

        if (distanceToObject2 > interactionRadius)
        {
            // Hide the interaction prompt if the player is too far
            interactionText.gameObject.SetActive(false);
            textSwitch = false;
            NPC npcComponent = interactables[objectNum].GetComponent<NPC>();
            if (npcComponent != null)
            {
                npcComponent.moveSpeed = 2f;


            }
            if(npcComponent.active)
            {
                npcComponent.MenuInteraction();
            }

        }                                
    }

    void Interact(int type)
    {
        // Your interaction logic here
        Debug.Log("Player interacted with the object");
        NPC npcComponent = interactables[type].GetComponent<NPC>();
        if (npcComponent != null)
        {
            npcComponent.moveSpeed = 0;
            
        }
        if (type < 2)
        {
            npcComponent.DialogueOutput(type);
        }
        npcComponent.MenuInteraction();
    }

    public void SideMenuToggle()
    {
        RectTransform rectTransform = myButton.GetComponent<RectTransform>();
        // If the menu isnt already toggled
        if (!uiToggle)
        {
            uiPanel.SetActive(true); // Set panel active
            rectTransform.anchoredPosition += new Vector2(250, 0); // Move right 250
            uiToggle = true;
        }
        else
        {
            uiPanel.SetActive(false);
            rectTransform.anchoredPosition -= new Vector2(250, 0); // Move right 250
            uiToggle=false;
        }
        

    }
}
