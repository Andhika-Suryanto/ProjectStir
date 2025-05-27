using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogBoxTriggerContainer : MonoBehaviour
{
    public List<DialogBox> waypoints = new List<DialogBox>(); // Dynamic list of waypoints

    private void Start()
    {
        DialogBox[] childWaypoints = GetComponentsInChildren<DialogBox>();
        waypoints = childWaypoints.ToList();

    }
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Gizmos.color = Color.blue;

        for (int i = 0; i < waypoints.Count; i++)
        {
            // Draw a sphere at each waypoint position
            Gizmos.DrawSphere(waypoints[i].transform.position, 0.5f);
            Gizmos.DrawWireSphere(waypoints[i].transform.position, 10f);

            // Draw lines between waypoints
            if (i < waypoints.Count - 1)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i+1].transform.position);
            }
        }
    }
}
