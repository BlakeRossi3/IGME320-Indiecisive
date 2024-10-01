using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed = 2f;            // Speed of the NPC
    public GameObject[] obstacles;          // Array of obstacles in the scene
    public int count = 0;
    public Vector2 currentDirection;        // Current movement direction
    private Rigidbody2D rb;
    public GameObject menu; // Menu for interacting with ship
    public bool active = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        menu.SetActive(false);
    }

    void Update()
    {
        // Check for collisions with obstacles
        if (count > 75)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                if (IsCollidingWithObstacle(obstacles[i]))
                {
                    switch (i)
                    {
                        case 0:
                            currentDirection = Vector2.down;
                            break;
                        case 1:
                            currentDirection = Vector2.up;
                            break;
                        case 2:
                            currentDirection = Vector2.left ;
                            break;
                        case 3:
                            currentDirection = Vector2.right;
                            break;
                        case 6:
                            moveSpeed = 0;
                            break;
                        default:
                            currentDirection = ChooseNewDirection();
                            break; 
                    }
                    
                }
            }

        }
        // Move the NPC in the current directio
        rb.MovePosition(rb.position + currentDirection * moveSpeed * Time.fixedDeltaTime);
        count++;
    }

    // Function to detect if NPC is colliding with an obstacle
    bool IsCollidingWithObstacle(GameObject obstacle)
    {
        // Get the collider of the obstacle
        Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
        if (obstacleCollider == null) return false;

        // Check if the NPC's collider is overlapping with the obstacle's collider
        Collider2D npcCollider = GetComponent<Collider2D>();
        return npcCollider.IsTouching(obstacleCollider);
    }



    // Utility function to get a random direction
    Vector2 ChooseNewDirection()
    {
        count = 0;

        // List of all possible directions
        List<Vector2> possibleDirections = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        // Remove the current direction from the list of possible directions
        possibleDirections.Remove(currentDirection);

        // Choose a random direction from the remaining three directions
        int randomIndex = Random.Range(0, possibleDirections.Count);

        // Return the new direction
        return possibleDirections[randomIndex];
    }

    public void MenuInteraction()
    {
        if (!active)
        {
            menu.SetActive(true);
            active = true;
        }
        else
        {
            menu.SetActive(false); 
            active = false;
        }
    }

}
