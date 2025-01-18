using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's Transform
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10); // Camera offset position
    [SerializeField] private float smoothSpeed = 0.125f; // Speed of smoothing the camera

    private void LateUpdate()
    {
        if (player == null) return; // Ensure the player is assigned

        // Desired position for the camera
        Vector3 desiredPosition = player.position + offset;

        // Smooth transition between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Optionally, keep the camera looking at the player
        transform.LookAt(player);
    }
}
