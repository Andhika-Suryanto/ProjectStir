using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSignContainer : MonoBehaviour
{
    public List<TurnSign> waypoints = new List<TurnSign>(); // Dynamic list of waypoints


    private void Start()
    {
        TurnSign[] childWaypoints = GetComponentsInChildren<TurnSign>();
        waypoints = childWaypoints.ToList();

    }
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < waypoints.Count; i++)
        {
            // Draw a sphere at each waypoint position
            Gizmos.DrawSphere(waypoints[i].transform.position, 0.5f);
            Gizmos.DrawWireSphere(waypoints[i].transform.position, 10f);

            // Draw lines between waypoints
            if (i < waypoints.Count - 1)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }
        }
    }
}
