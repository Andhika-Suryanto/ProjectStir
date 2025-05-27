using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject pressStartText;
    public GameObject gameModePanel;
    public Button[] buttons;
    private int currentIndex = 0;
    private bool inGameModeSelection = false;

    private void Start()
    {
        // Show only the "Press Start" page
        pressStartText.SetActive(true);
        gameModePanel.SetActive(false);
    }

    private void Update()
    {
        if (!inGameModeSelection)
        {
            // Wait for Start Press to go to Page 2
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
            {
                ShowGameModes();
            }
        }
        else
        {
            // Navigate through Game Mode options
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Gamepad.current?.leftShoulder.wasPressedThisFrame == true)
            {
                ChangeSelection(-1);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Gamepad.current?.rightShoulder.wasPressedThisFrame == true)
            {
                ChangeSelection(1);
            }

            // Select game mode
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
            {
                SelectGameMode();
            }
        }
    }

    private void ShowGameModes()
    {
        pressStartText.SetActive(false);
        gameModePanel.SetActive(true);
        inGameModeSelection = true;
        HighlightButton();
    }

    private void ChangeSelection(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        if (currentIndex >= buttons.Length) currentIndex = 0;
        HighlightButton();
    }

    private void HighlightButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Color color = (i == currentIndex) ? Color.green : Color.white;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = color;
        }
    }

    private void SelectGameMode()
    {
        Debug.Log("Selected: " + buttons[currentIndex].name);
        if (buttons[currentIndex].name == "SinglePlayerButton")
        {
            SceneManager.LoadScene(1); // Load Single Player Scene
        }
        else if (buttons[currentIndex].name == "MultiplayerButton")
        {
            SceneManager.LoadScene(2); // Load Multiplayer Scene
        }
        else if (buttons[currentIndex].name == "SettingsButton")
        {
            Debug.Log("Open Settings");
            // Add settings logic
        }
    }
}
