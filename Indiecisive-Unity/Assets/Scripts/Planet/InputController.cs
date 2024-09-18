using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the player moves

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from WASD keys or arrow keys
        if (moveInput.y == 0) 
        {
            moveInput.x = Input.GetAxis("Horizontal");
        }
        else if (moveInput.x == 0)
        {
            moveInput.y = Input.GetAxis("Vertical");
        }
    }

    void FixedUpdate()
    {
        // Apply movement based on input and moveSpeed
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
