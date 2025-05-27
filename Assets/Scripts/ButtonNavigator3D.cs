using UnityEngine;
using UnityEngine.InputSystem; // Needed for new Input System

public class ButtonNavigator3D : MonoBehaviour
{
    public GameObject[] buttons; // Assign buttons in Inspector
    private int currentIndex = 0;
    private Renderer lastSelectedRenderer;

    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;

    private void Start()
    {
        if (buttons.Length > 0)
        {
            HighlightButton(currentIndex);
        }
    }

    private void Update()
    {
        // Keyboard Input
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Gamepad.current?.leftShoulder.wasPressedThisFrame == true)
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Gamepad.current?.rightShoulder.wasPressedThisFrame == true)
        {
            MoveRight();
        }

        // Press Button (Enter or Controller's A Button)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
        {
            SelectButton();
        }
    }

    private void MoveLeft()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        HighlightButton(currentIndex);
    }

    private void MoveRight()
    {
        currentIndex++;
        if (currentIndex >= buttons.Length) currentIndex = 0;
        HighlightButton(currentIndex);
    }

    private void HighlightButton(int index)
    {
        if (lastSelectedRenderer != null)
        {
            lastSelectedRenderer.material.color = normalColor;
        }

        lastSelectedRenderer = buttons[index].GetComponent<Renderer>();
        lastSelectedRenderer.material.color = selectedColor;
    }

    private void SelectButton()
    {
        Debug.Log("Selected: " + buttons[currentIndex].name);
        // Add different functionality based on button name
        switch (buttons[currentIndex].name)
        {
            case "Button_SinglePlayer":
                Debug.Log("Single Player Selected!");
                break;
            case "Button_Multiplayer":
                Debug.Log("Multiplayer Selected!");
                break;
            case "Button_Options":
                Debug.Log("Options Selected!");
                break;
        }
    }
}
