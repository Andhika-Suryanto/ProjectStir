using UnityEngine;

public class SmoothCameraTransition : MonoBehaviour
{
    public Transform target;
    public float duration = 1f;
    public bool followRotation = true;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timer = 0f;
    private bool isMoving = false;

    public void StartTransition(Transform targetTransform)
    {
        target = targetTransform;
        startPosition = transform.position;
        startRotation = transform.rotation;
        timer = 0f;
        isMoving = true;
    }

    void LateUpdate()
    {
        if (!isMoving || target == null) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        transform.position = Vector3.Lerp(startPosition, target.position, t);

        if (followRotation)
        {
            transform.rotation = Quaternion.Slerp(startRotation, target.rotation, t);
        }

        if (t >= 1f) isMoving = false;
    }
}
