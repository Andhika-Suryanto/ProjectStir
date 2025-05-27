using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSignAutoAssigner : MonoBehaviour
{
    [Header("Assign These References")]
    public TurnSignContainer waypointManager; // Drag the TurnSignContainer from the scene
    public GameObject template; // Drag the UI template GameObject
    public Image turnsign; // Drag the Image component for the turn sign

    private void Awake()
    {
        AssignToTurnSignCheckers();
    }

    private void AssignToTurnSignCheckers()
    {
        // Find all TurnSignChecker scripts in the scene
        TurnSignChecker[] checkers = FindObjectsOfType<TurnSignChecker>();

        if (checkers.Length == 0)
        {
            Debug.LogWarning("⚠️ No TurnSignChecker objects found in the scene!");
            return;
        }

        foreach (TurnSignChecker checker in checkers)
        {
            if (waypointManager != null)
            {
                checker.waypointManager = waypointManager;
                Debug.Log($"✅ Assigned WaypointManager to {checker.gameObject.name}");
            }

            if (template != null)
            {
                checker.template = template;
                Debug.Log($"✅ Assigned Template to {checker.gameObject.name}");
            }

            if (turnsign != null)
            {
                checker.turnsign = turnsign;
                Debug.Log($"✅ Assigned TurnSign Image to {checker.gameObject.name}");
            }
        }

        Debug.Log("✅ Auto-assignment for TurnSignChecker completed!");
    }
}
