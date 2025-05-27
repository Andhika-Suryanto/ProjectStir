using UnityEngine;
using UnityEngine.InputSystem;

public class HideAndShowOnPress : MonoBehaviour
{
    public GameObject objectToHide;  // Object that will be hidden
    public GameObject objectToShow;  // Object that will be shown

    void Update()
    {
        // Check for keyboard keys (Enter, Space) and gamepad Start button
        if (Keyboard.current.enterKey.wasPressedThisFrame || 
            Keyboard.current.spaceKey.wasPressedThisFrame || 
            Gamepad.current?.startButton.wasPressedThisFrame == true)
        {
            PerformAction();
        }
    }

    public void OnClick()
    {
        PerformAction();
    }

    private void PerformAction()
    {
        if (objectToHide != null) objectToHide.SetActive(false);
        if (objectToShow != null) objectToShow.SetActive(true);
    }
}
