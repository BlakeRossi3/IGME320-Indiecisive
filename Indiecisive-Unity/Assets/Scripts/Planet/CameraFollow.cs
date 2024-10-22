using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // The player's transform
    public float smoothSpeed = 0.125f;  // The smoothing speed for the camera's movement
    public Vector3 offset;         // The offset from the player to the camera

    void LateUpdate()
    {
        // Calculate the desired position based on the player position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Smooth the position between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Check if the Y position is within the allowed range but the X position is out of bounds
        if (smoothedPosition.y > -15.7f && smoothedPosition.y < 15.3f && !(smoothedPosition.x > -12.6f && smoothedPosition.x < 11.5f))
        {
            // Constrain the Y-axis while keeping the X position constant
            transform.position = new Vector3(transform.position.x, smoothedPosition.y, transform.position.z);
        }
        // Check if the X position is within the allowed range but the Y position is out of bounds
        else if (smoothedPosition.x > -12.6f && smoothedPosition.x < 11.5f && !(smoothedPosition.y > -15.7f && smoothedPosition.y < 15.3f))
        {
            // Constrain the X-axis while keeping the Y position constant
            transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
        }
        else if (!(smoothedPosition.x > -12.6f && smoothedPosition.x < 11.5f) && !(smoothedPosition.y > -15.7f && smoothedPosition.y < 15.3f))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            // Update both X and Y positions if neither are out of bounds
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}
