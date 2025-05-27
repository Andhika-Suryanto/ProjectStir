using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter : MonoBehaviour
{
    [Tooltip("Optional delay in seconds before reloading the scene")]
    [SerializeField] private float resetDelay = 0.1f;
    
    [Tooltip("Enable/disable scene reset functionality")]
    [SerializeField] private bool resetEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (!resetEnabled)
            return;
            
        // Check for keyboard Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetScene();
            return;
        }
        
        // Check for Xbox controller B button or PS4/PS5 Circle button
        // Both use the same input on different controllers
        if (Input.GetButtonDown("Cancel") || // Default mapping for B/Circle in Unity Input system
            Input.GetKeyDown(KeyCode.JoystickButton1)) // Alternative direct button check
        {
            ResetScene();
            return;
        }
    }
    
    private void ResetScene()
    {
        // Optional: Add any cleanup or save logic here before reset
        
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (resetDelay <= 0)
        {
            // Reset immediately
            SceneManager.LoadScene(currentSceneIndex);
        }
        else
        {
            // Reset after delay
            Invoke("LoadCurrentScene", resetDelay);
        }
    }
    
    private void LoadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    // Optional: Public method to trigger reset from other scripts
    public void TriggerReset()
    {
        if (resetEnabled)
            ResetScene();
    }
    
    // Optional: Method to enable/disable reset functionality
    public void SetResetEnabled(bool enabled)
    {
        resetEnabled = enabled;
    }
}