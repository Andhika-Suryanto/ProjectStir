using UnityEngine;

public class StickToObject : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The object to stick to
    public Vector3 positionOffset = Vector3.zero; // Offset relative to the target
    public Vector3 rotationOffset = Vector3.zero; // Optional rotation offset

    [Header("Update Options")]
    public bool followPosition = true;
    public bool followRotation = false;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("StickToObject: No target assigned!");
            return;
        }

        // Stick to the target position with an offset
        if (followPosition)
        {
            transform.position = target.position + target.TransformDirection(positionOffset);
        }

        // Stick to the target rotation
        if (followRotation)
        {
            transform.rotation = target.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
