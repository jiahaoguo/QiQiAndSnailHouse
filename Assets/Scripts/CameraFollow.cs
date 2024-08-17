using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;            // The target that the camera will follow
    public float smoothSpeed = 0.125f;  // The speed with which the camera will catch up to the target
    public Vector3 offset;              // The offset from the target to the camera

    void FixedUpdate()
    {
        // Calculate the desired position, only changing the x and y coordinates for 2D
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = smoothedPosition;
    }
}
