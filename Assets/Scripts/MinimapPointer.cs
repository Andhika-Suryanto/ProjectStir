using UnityEngine;

public class MinimapPointer : MonoBehaviour
{
    [Header("Pointer Settings")]
    [Tooltip("Assign the car GameObject in the Inspector or use the Tag system")]
    public GameObject car;
    
    [Tooltip("If true, will find car by tag instead of direct reference")]
    public bool findCarByTag = false;
    
    [Tooltip("Tag to use if findCarByTag is enabled")]
    public string carTag = "Player";
    
    [Tooltip("Height above the car in world units")]
    public float heightOffset = 5f;
    
    // Store the initial car reference to prevent overriding
    private GameObject initialCarReference;
    
    // Track if we've been properly initialized
    private bool initialized = false;

    void Start()
    {
        // Find or validate car reference
        SetupCarReference();
        
        if (initialCarReference != null)
        {
            // Move the pointer above the car
            transform.position = initialCarReference.transform.position + Vector3.up * heightOffset;

            // Make sure the triangle is laying flat
            transform.rotation = Quaternion.Euler(90, 0, 0); // Rotates it to lay flat
            
            initialized = true;
        }
    }
    
    void SetupCarReference()
    {
        // If we already found our car, don't override it
        if (initialCarReference != null)
            return;
            
        // Option 1: Use direct reference
        if (!findCarByTag && car != null)
        {
            initialCarReference = car;
            return;
        }
        
        // Option 2: Use tag system
        if (findCarByTag)
        {
            GameObject taggedCar = GameObject.FindWithTag(carTag);
            if (taggedCar != null)
            {
                initialCarReference = taggedCar;
                return;
            }
        }
        
        // If we reach here, we failed to find a reference
        Debug.LogError("Car reference not found for MinimapPointer on " + gameObject.name + "!");
    }

    void Update()
    {
        // If we haven't been initialized yet, try again
        if (!initialized)
        {
            SetupCarReference();
            
            if (initialCarReference != null)
            {
                initialized = true;
            }
            else
            {
                // Skip this frame if we still don't have a car reference
                return;
            }
        }

        // Keep the pointer following the car but staying at a fixed height
        transform.position = initialCarReference.transform.position + Vector3.up * heightOffset;

        // Rotate to match the car's movement, but keep it flat
        float carRotation = initialCarReference.transform.eulerAngles.y; // Get Y-axis rotation for 3D movement
        transform.rotation = Quaternion.Euler(90, 0, -carRotation);
    }
    
    // Public method to manually set the car if needed (e.g., after spawning)
    public void SetCar(GameObject newCar)
    {
        if (newCar != null)
        {
            initialCarReference = newCar;
            car = newCar; // Update the inspector reference too
            initialized = true;
        }
    }
}