using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private float moveSpeed = 2f;            // Speed of the NPC
    public GameObject[] obstacles;          // Array of obstacles in the scene
    private int count = 0;
    public Vector2 currentDirection;        // Current movement direction
    private Rigidbody2D rb;

    void Start()
    {

    }

    void Update()
    {
        // Check for collisions with obstacles
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (IsCollidingWithObstacle(obstacles[i]) && count > 700)
            {
                // If there's a collision with an obstacle, choose a new random direction
                currentDirection = ChooseNewDirection();
                break; // No need to check other obstacles after detecting a collision
            }
        }

        count++;
        // Move the NPC in the current direction
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime);
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
    Vector2   ChooseNewDirection()
    {
        count = 0;

        int randomDir = Random.Range(0, 4);
        switch (randomDir)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            case 3: return Vector2.right;
            default: return Vector2.up;  // Fallback in case something goes wrong
        }


    }
}
