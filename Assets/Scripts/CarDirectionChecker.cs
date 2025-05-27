using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarDirectionChecker : MonoBehaviour
{
    public CarDirectionContainer waypointManager; // Reference to the WaypointManager script
    private List<Transform> waypoints;
    public int currentWaypointIndex = 0;
    public float directionThreshold = 45f; // Angle threshold to determine wrong direction
    public GameObject warningText; // UI Text for warning message

    private void Start()
    {
        if (waypointManager != null)
        {
            waypoints = waypointManager.waypoints;
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        CheckDirection();
    }

    void CheckDirection()
    {
        if (currentWaypointIndex >= waypoints.Count)
            return;

        Transform nextWaypoint = waypoints[currentWaypointIndex];
        Vector3 toWaypoint = nextWaypoint.position - transform.position;
        Vector3 forward = transform.forward;

        // Visualize directions
        //Debug.DrawLine(transform.position, transform.position + forward * 5, Color.blue);
        //Debug.DrawLine(transform.position, nextWaypoint.position, Color.red);

        // Calculate the angle between the car's forward direction and the direction to the waypoint
        float angle = Vector3.Angle(forward, toWaypoint);

        // If the angle exceeds the threshold, display a warning
        if (angle > directionThreshold)
        {
            Debug.DrawLine(transform.position, nextWaypoint.position, Color.red);
            warningText.SetActive(true);
        }
        else
        {
            Debug.DrawLine(transform.position, nextWaypoint.position, Color.blue);
            warningText.SetActive(false); // Clear the warning
        }

        // Update the current waypoint index if the car is close enough to the waypoint
        if (Vector3.Distance(transform.position, nextWaypoint.position) < 10f)
        {
            currentWaypointIndex++;
        }
    }
}
