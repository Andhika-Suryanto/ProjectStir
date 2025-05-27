using UnityEngine;

public class CarNameUI : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject car; // Assign the car GameObject in the Inspector
    public float heightOffset = 2f;  // Height above the car in world units
    
    [Header("Rotation Settings")]
    [Tooltip("Whether the text should rotate with the car's left/right movement")]
    public bool allowHorizontalRotation = true;
    [Tooltip("Makes the text always face the camera")]
    public bool faceCamera = true;
    [Tooltip("Offsets the position forward/backward relative to the car")]
    public float forwardOffset = 0f;
    
    private Camera mainCamera;

    void Start()
    {
        if (car == null)
        {
            Debug.LogError("Car is not assigned to CarNameUI!");
            return;
        }

        mainCamera = Camera.main;
        
        // Position the name tag above the car
        UpdatePosition();
        
        // Set initial rotation
        UpdateRotation();
    }

    void Update()
    {
        if (car == null)
            return;

        // Update position every frame
        UpdatePosition();
        
        // Update rotation every frame
        UpdateRotation();
    }
    
    void UpdatePosition()
    {
        // Calculate the position above the car with optional forward offset
        Vector3 forwardDir = car.transform.forward * forwardOffset;
        transform.position = car.transform.position + Vector3.up * heightOffset + forwardDir;
    }
    
    void UpdateRotation()
    {
        if (faceCamera && mainCamera != null)
        {
            // Make the object face the camera, but keep it upright
            Vector3 dirToCamera = mainCamera.transform.position - transform.position;
            dirToCamera.y = 0; // Remove vertical component to keep it upright
            
            if (dirToCamera != Vector3.zero) // Avoid look rotation errors
            {
                transform.rotation = Quaternion.LookRotation(dirToCamera);
            }
        }
        else if (allowHorizontalRotation)
        {
            // Only take Y rotation from the car (horizontal rotation)
            float carYRotation = car.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, carYRotation, 0);
        }
        else
        {
            // Keep it facing forward with no rotation
            transform.rotation = Quaternion.identity;
        }
    }
}