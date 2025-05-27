using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputFieldHandler : MonoBehaviour
{
    public TMP_InputField tmpInputField; // Assign your TMP_InputField in the Unity Inspector.
    public List<TMP_Text> displayText; // Assign your TMP_Text UI elements in the Unity Inspector.
    public Button btnName;
    public string defaultPlayerName = "Hashiriya"; // Default name when input is empty

    public void checkEmpty()
    {
        if (tmpInputField.text == "")
        {
            btnName.interactable = false;
        }
        else
        {
            btnName.interactable = true;
        }
    }

    public void OnButtonClick()
    {
        // Get text from TMP_InputField, use default name if empty
        string inputText = string.IsNullOrEmpty(tmpInputField.text) ? defaultPlayerName : tmpInputField.text;
        GameManager.Instance.myName = inputText;
        
        // Replace all instances of <PlayerName> in all displayText elements
        foreach (TMP_Text textElement in displayText)
        {
            if (textElement != null)
            {
                string updatedText = textElement.text.Replace("<PlayerName>", inputText);
                textElement.text = updatedText;
            }
        }
    }
}