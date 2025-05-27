using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ActivableObjectsOnButton : MonoBehaviour
{
    public GameObject[] activeGameObjects;
    public GameObject[] deactiveGameObjects;
    private Button thisButton;

    private void OnEnable()
    {
        thisButton = GetComponent<Button>();

        if (thisButton != null)
        {
            thisButton.onClick.AddListener(DeactivatedGameObjects);
            thisButton.onClick.AddListener(ActivatedGameObjects);
        }
    }

    private void OnDisable()
    {
        if (thisButton != null)
        {
            thisButton.onClick.RemoveListener(DeactivatedGameObjects);
            thisButton.onClick.RemoveListener(ActivatedGameObjects);
        }
    }

    private void Update()
    {
        // Detect Xbox "A" Button Press (Gamepad Button South)
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("🎮 Xbox A Button Pressed!");
            thisButton?.onClick.Invoke();
        }
    }

    void DeactivatedGameObjects()
    {
        foreach (var obj in deactiveGameObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    void ActivatedGameObjects()
    {
        foreach (var obj in activeGameObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
