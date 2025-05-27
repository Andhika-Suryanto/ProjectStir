using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDirectionAutoAssigner : MonoBehaviour
{
    [Header("Waypoint Manager (Auto Assign)")]
    public CarDirectionContainer waypointManager; // Assign manually or auto-detect

    [Header("Warning UI Element (Auto Assign)")]
    public GameObject warningText; // Assign if all cars share the same warning UI

    private void Start()
    {
        AssignToAllCarDirectionCheckers();
    }

    private void AssignToAllCarDirectionCheckers()
    {
        CarDirectionChecker[] carDirectionCheckers = FindObjectsOfType<CarDirectionChecker>();

        if (carDirectionCheckers.Length == 0)
        {
            Debug.LogWarning("⚠️ No CarDirectionChecker components found in the scene!");
            return;
        }

        foreach (CarDirectionChecker checker in carDirectionCheckers)
        {
            // Assign Waypoint Manager
            if (waypointManager != null)
            {
                checker.waypointManager = waypointManager;
            }
            else
            {
                CarDirectionContainer autoManager = FindObjectOfType<CarDirectionContainer>();
                if (autoManager != null)
                {
                    checker.waypointManager = autoManager;
                    Debug.Log($"✅ Auto-assigned Waypoint Manager to {checker.gameObject.name}");
                }
                else
                {
                    Debug.LogWarning($"⚠️ No CarDirectionContainer found for {checker.gameObject.name}");
                }
            }

            // Auto-assign warningText if not assigned
            if (checker.warningText == null)
            {
                if (warningText != null)
                {
                    checker.warningText = warningText;
                }
                else
                {
                    GameObject foundWarning = GameObject.Find("WarningText");
                    if (foundWarning != null)
                    {
                        checker.warningText = foundWarning;
                        Debug.Log($"✅ Auto-assigned WarningText to {checker.gameObject.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ No warningText found for {checker.gameObject.name}");
                    }
                }
            }
        }
    }
}
