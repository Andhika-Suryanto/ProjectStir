using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointLooker : MonoBehaviour
{
    private CheckPointManager checkPointManager; // Auto-detected
    private GameObject checkPoint;
    public GameObject checkPointLooker; // 3D Object rotating towards checkpoint
    private UICheckpointLooker uiCheckpointLooker; // Reference to UI script

    void Start()
    {
        // Auto-find GameManager and CheckPointManager
        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            checkPointManager = gameManagerObject.GetComponent<CheckPointManager>();
        }

        if (checkPointManager == null)
        {
            Debug.LogError("CheckPointManager not found! Make sure GameManager has CheckPointManager attached.");
        }

        // Auto-find the UI script
        uiCheckpointLooker = FindObjectOfType<UICheckpointLooker>();
    }

    void Update()
    {
        if (checkPointManager == null || checkPointLooker == null)
            return;

        checkPoint = checkPointManager.getCurrentCheckPoint();
        if (checkPoint == null)
            return;

        // Rotate the checkpoint looker towards the checkpoint
        checkPointLooker.transform.LookAt(checkPoint.transform.position);

        // Get the left/right rotation angle
        float angle = GetSignedRotationAngle();

        // If UI script is found, send the rotation update
        if (uiCheckpointLooker != null)
        {
            uiCheckpointLooker.SetUIRotation(angle);
        }
    }

    private float GetSignedRotationAngle()
    {
        // Direction from looker to checkpoint
        Vector3 directionToCheckpoint = checkPoint.transform.position - checkPointLooker.transform.position;
        directionToCheckpoint.y = 0; // Ignore Y movement for left/right rotation

        // Calculate signed angle for left/right rotation
        return Vector3.SignedAngle(Vector3.forward, directionToCheckpoint.normalized, Vector3.up);
    }
}
