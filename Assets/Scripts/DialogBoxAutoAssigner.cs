using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBoxAutoAssigner : MonoBehaviour
{
    [Header("UI Elements to Assign (Drag from PanelGameplay)")]
    public GameObject template;
    public Image character;
    public TMP_Text names;
    public TMP_Text chatJPN;
    public TMP_Text chat;
    public DialogBoxTriggerContainer waypointManager;

    private void Start()
    {
        AssignToAllDialogBoxCheckers();
    }

    private void AssignToAllDialogBoxCheckers()
    {
        // Find all DialogBoxChecker components in the scene
        DialogBoxChecker[] dialogBoxes = FindObjectsOfType<DialogBoxChecker>();

        if (dialogBoxes.Length == 0)
        {
            Debug.LogWarning("⚠️ No DialogBoxChecker components found in the scene!");
            return;
        }

        foreach (DialogBoxChecker dialogBox in dialogBoxes)
        {
            // Assign UI elements to each found DialogBoxChecker
            dialogBox.template = template;
            dialogBox.Character = character;
            dialogBox.names = names;
            dialogBox.ChatJPN = chatJPN;
            dialogBox.Chat = chat;

            // Assign Waypoint Manager if it's available
            if (waypointManager != null)
            {
                dialogBox.waypointManager = waypointManager;
            }

            Debug.Log($"✅ Assigned UI elements to DialogBoxChecker on: {dialogBox.gameObject.name}");
        }
    }
}
