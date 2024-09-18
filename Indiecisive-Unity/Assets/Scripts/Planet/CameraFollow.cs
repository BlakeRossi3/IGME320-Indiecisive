using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;         

    void LateUpdate()
    {
        // Calculate the desired position based on the player position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Smooth position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Keep Z position constant
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
