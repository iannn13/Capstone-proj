using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float followSpeed = 0.1f;
    public float xOffset = -2.0f; // Negative value to move the camera to the left

    private void LateUpdate()
    {
        // Calculate the target position with an offset to the left
        Vector3 targetPosition = new Vector3(player.position.x + xOffset, player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}
