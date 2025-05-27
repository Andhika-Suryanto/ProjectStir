using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAssignmentManager : MonoBehaviour
{
    [Header("Car Assignment")]
    public GameObject selectedCar; // Assign this in the Inspector

    [Header("Car Parts (For Manual Dragging)")]
    public RCC_DetachablePart bumperFPart;
    public RCC_DetachablePart bumperRPart;
    public RCC_DetachablePart hood;
    public RCC_DetachablePart trunk;
    public RCC_DetachablePart doorR;
    public RCC_DetachablePart doorL;

    private void Start()
    {
        if (selectedCar == null)
        {
            Debug.LogError("❌ No car assigned in CarAssignmentManager!");
            return;
        }

        AssignCar(selectedCar);
    }

    /// <summary>
    /// Assigns the selected car to relevant scripts: GameManager, Damage.cs, RCC Cameras, RCC Scene Manager, Minimap, and PositionComparator.
    /// </summary>
    public void AssignCar(GameObject car)
    {
        if (car == null)
        {
            Debug.LogError("❌ No car found for assignment!");
            return;
        }

        Debug.Log("✅ Assigning car: " + car.name);

        // ✅ Assign car to GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.car = car.GetComponent<RCC_CarControllerV3>();
            Debug.Log("✅ Assigned car to GameManager.");
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager not found in the scene!");
        }

        // ✅ Assign parts to Damage script (Now from GameManager)
        Damage damageScript = gameManager?.GetComponent<Damage>();

        if (damageScript != null)
        {
            Debug.Log("✅ Found Damage script on GameManager.");

            // **Use Dragged Parts (If Assigned), Otherwise Auto-Find**
            damageScript.bumperFPart = bumperFPart != null ? bumperFPart : FindPart(car, "Bumper_F");
            damageScript.bumperRPart = bumperRPart != null ? bumperRPart : FindPart(car, "Bumper_R");
            damageScript.hood = hood != null ? hood : FindPart(car, "Hood");
            damageScript.trunk = trunk != null ? trunk : FindPart(car, "Trunk");
            damageScript.doorR = doorR != null ? doorR : FindPart(car, "Door_R");
            damageScript.doorL = doorL != null ? doorL : FindPart(car, "Door_L");

            Debug.Log("✅ Assigned car parts to Damage script.");
        }
        else
        {
            Debug.LogWarning("⚠️ Damage script not found on GameManager!");
        }

        // ✅ Assign car to ALL RCC Cameras in the scene
        RCC_Camera[] rccCameras = FindObjectsOfType<RCC_Camera>(); // Get all RCC Cameras
        if (rccCameras.Length > 0)
        {
            foreach (RCC_Camera rccCamera in rccCameras)
            {
                rccCamera.SetTarget(car.GetComponent<RCC_CarControllerV3>());
                Debug.Log($"✅ Assigned car to RCC_Camera: {rccCamera.gameObject.name}");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No RCC_Camera found in the scene!");
        }

        // ✅ Assign car to RCC Scene Manager (if exists)
        if (RCC_SceneManager.Instance != null)
        {
            RCC_SceneManager.Instance.registerLastSpawnedVehicleAsPlayerVehicle = false; // Prevent auto-assigning another car
            RCC_SceneManager.Instance.RegisterPlayer(car.GetComponent<RCC_CarControllerV3>(), true, true);
            Debug.Log("✅ Assigned car to RCC_SceneManager.");
        }
        else
        {
            Debug.LogWarning("⚠️ RCC_SceneManager not found in the scene!");
        }

        // ✅ Assign car to MinimapPointer (if exists)
        MinimapPointerPlayer minimapPointerPlayer = FindObjectOfType<MinimapPointerPlayer>();
        if (minimapPointerPlayer != null)
        {
            minimapPointerPlayer.playerCar = car;
            Debug.Log("✅ Assigned car to MinimapPointerPlayer.");
        }
        else
        {
            Debug.LogWarning("⚠️ MinimapPointerPlayer not found in the scene!");
        }

        // ✅ Assign car to ChangeCameraView (if exists)
        ChangeCameraView changeCameraView = FindObjectOfType<ChangeCameraView>();
        if (changeCameraView != null)
        {
            changeCameraView.car = car.GetComponent<RCC_CarControllerV3>();
            if (rccCameras.Length > 0)
            {
                changeCameraView.Cam = rccCameras[0]; // Assign the first RCC Camera to ChangeCameraView
            }
            Debug.Log("✅ Assigned car to ChangeCameraView.");
        }
        else
        {
            Debug.LogWarning("⚠️ ChangeCameraView not found in the scene!");
        }

        // ✅ Assign car to PositionComparator (for race position tracking)
 GameObject positionComparatorObj = GameObject.Find("===RCC===/RCCCanvas/PanelGameplay/Position");

if (positionComparatorObj != null)
{
    PositionComparator positionComparator = positionComparatorObj.GetComponent<PositionComparator>();

    if (!positionComparatorObj.activeInHierarchy)
    {
        positionComparatorObj.SetActive(true);  // Enable it
    }

    // ✅ Assign the car's waypoint tracker to PositionComparator
    CarDirectionChecker carDirectionChecker = car.GetComponent<CarDirectionChecker>();
    if (carDirectionChecker != null)
    {
        positionComparator.OurWaypoint = carDirectionChecker;
        Debug.Log("✅ Successfully assigned car to PositionComparator.");
    }
    else
    {
        Debug.LogWarning("⚠️ CarDirectionChecker not found on the car!");
    }
}
else
{
    Debug.LogWarning("⚠️ PositionComparator not found at path: ===RCC===/RCCCanvas/PanelGameplay/Position");
}
        FollowTransform followTransform = FindObjectOfType<FollowTransform>();
        if (followTransform != null)
        {
            followTransform.target = car.transform;
            Debug.Log("✅ Assigned car to FollowTransform.");
        }
        else
        {
            Debug.LogWarning("⚠️ FollowTransform not found in the scene!");
        }
        Transform checkpointFolder = GameObject.Find("===CHECKPOINT")?.transform;

    if (checkpointFolder != null)
    {
        LookAt[] lookAtScripts = checkpointFolder.GetComponentsInChildren<LookAt>(true); // Find LookAt scripts in subfolders
        if (lookAtScripts.Length > 0)
        {
            foreach (LookAt lookAtScript in lookAtScripts)
            {
                lookAtScript.target = car.transform; // Assign the player's car as the target
                Debug.Log($"✅ Assigned car to LookAt script in {lookAtScript.gameObject.name}");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No LookAt scripts found in '===CHECKPOINT' folder!");
        }
    }
    else
    {
        Debug.LogWarning("⚠️ Folder '===CHECKPOINT' not found in the scene!");
    }

        Debug.Log("✅ Car assignment completed.");
    }

    /// <summary>
    /// Finds and returns an RCC_DetachablePart by name.
    /// </summary>
    private RCC_DetachablePart FindPart(GameObject car, string partName)
    {
        Transform partTransform = car.transform.Find(partName);
        if (partTransform != null)
        {
            RCC_DetachablePart part = partTransform.GetComponent<RCC_DetachablePart>();
            if (part != null)
            {
                Debug.Log("✅ Found part: " + partName);
            }
            else
            {
                Debug.LogWarning("⚠️ Missing RCC_DetachablePart on: " + partName);
            }
            return part;
        }
        Debug.LogWarning("⚠️ Part not found in hierarchy: " + partName);
        return null;
    }
}
