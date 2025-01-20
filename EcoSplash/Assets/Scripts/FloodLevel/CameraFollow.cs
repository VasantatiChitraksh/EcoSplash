using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Player's transform
    [SerializeField] private Vector3 offset;           // Offset from the player

    [SerializeField] private Vector2 xBounds;          // Minimum and maximum X values
    [SerializeField] private Vector2 zBounds;          // Minimum and maximum Z values
    [SerializeField] private float minHeight = 5f;     // Minimum height for the camera

    void LateUpdate()
    {
        // Calculate the new camera position based on the player's position
        Vector3 newPosition = playerTransform.position + offset;

        // Clamp the position within the defined bounds
        newPosition.x = Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y);
        newPosition.z = Mathf.Clamp(newPosition.z, zBounds.x, zBounds.y);

        // Ensure the camera's height does not go below the minHeight
        newPosition.y = Mathf.Max(playerTransform.position.y + offset.y, minHeight);

        // Update the camera position
        transform.position = newPosition;
    }
}
