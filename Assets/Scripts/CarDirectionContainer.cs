using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class CarDirectionContainer : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>(); // Dynamic list of waypoints
    

    private void Start()
    {
        Transform[] childWaypoints = GetComponentsInChildren<Transform>();
        waypoints = childWaypoints.ToList();

    }
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Count; i++)
        {
            // Draw a sphere at each waypoint position
            Gizmos.DrawSphere(waypoints[i].position, 0.5f);
            Gizmos.DrawWireSphere(waypoints[i].transform.position, 10f);

            // Draw lines between waypoints
            if (i < waypoints.Count - 1)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
