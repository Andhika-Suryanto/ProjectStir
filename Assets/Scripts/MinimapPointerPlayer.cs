using UnityEngine;

public class MinimapPointerPlayer : MonoBehaviour
{
    [Header("Pointer Settings")]
    [Tooltip("Drag your player car here in the Inspector")]
    public GameObject playerCar;
    
    [Tooltip("Height above the car in world units")]
    public float heightOffset = 5f;
    
    [Header("Rotation Settings")]
    [Tooltip("If true, will only use the car's Y-axis rotation and ignore roll/pitch")]
    public bool ignoreCarTilt = true;
    
    [Tooltip("Offset the rotation by this many degrees")]
    public float rotationOffset = 0f;
    
    [Tooltip("Smooth the pointer rotation to prevent jitter")]
    public bool smoothRotation = true;
    
    [Tooltip("How quickly the pointer rotates to match the car (higher = faster)")]
    [Range(1f, 20f)]
    public float rotationSpeed = 10f;

    private bool warningShown = false;
    private Quaternion targetRotation;

    void Start()
    {
        // Check if a car has been assigned in the Inspector
        if (playerCar == null)
        {
            Debug.LogError("Player car is not assigned to MinimapPointerPlayer on " + gameObject.name + "! Please drag a car into the Inspector.");
            return;
        }

        // Initial position and rotation setup
        UpdatePointerTransform();
    }

    void Update()
    {
        // Check if the reference is still valid
        if (playerCar == null)
        {
            if (!warningShown)
            {
                Debug.LogWarning("Player car reference was lost on " + gameObject.name + ". Please check if the car was destroyed.");
                warningShown = true;
            }
            return;
        }

        // Update position and rotation every frame
        UpdatePointerTransform();
    }

    // Helper method to update position and rotation
    private void UpdatePointerTransform()
    {
        if (playerCar == null)
            return;

        // Keep the pointer following the car but staying at a fixed height
        transform.position = playerCar.transform.position + Vector3.up * heightOffset;

        // Calculate the correct rotation based on car's orientation
        UpdatePointerRotation();
    }

    private void UpdatePointerRotation()
    {
        // Get only the Y-axis rotation from the car
        float carYRotation = playerCar.transform.eulerAngles.y;
        
        // Apply rotation offset
        float finalRotation = carYRotation + rotationOffset;
        
        // Create a rotation that only takes the Y-axis rotation from the car
        // But keeps the pointer flat on the X-axis (pointing 90 degrees down)
        targetRotation = Quaternion.Euler(90f, 0f, -finalRotation);
        
        // Apply rotation immediately or smoothly
        if (smoothRotation)
        {
            // Smoothly interpolate to the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Apply rotation immediately
            transform.rotation = targetRotation;
        }
    }

    // Public method to change the targeted car at runtime if needed
    public void SetTargetCar(GameObject newCar)
    {
        if (newCar != null)
        {
            playerCar = newCar;
            warningShown = false;
            Debug.Log("MinimapPointerPlayer now following: " + newCar.name);
        }
        else
        {
            Debug.LogError("Attempted to set null car reference on MinimapPointerPlayer!");
        }
    }

    // Reset the pointer's rotation to be flat
    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}