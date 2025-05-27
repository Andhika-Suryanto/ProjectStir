using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSignChecker : MonoBehaviour
{
    public TurnSignContainer waypointManager; // Reference to the WaypointManager script
    private List<TurnSign> waypoints;
    public int currentWaypointIndex = 0;

    public float delay = 3f;


    [Header("Reference Templatenya")]
    public GameObject template;
    public Image turnsign;


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

        TurnSign nextWaypoint = waypoints[currentWaypointIndex];
        Vector3 toWaypoint = nextWaypoint.transform.position - transform.position;
        Vector3 forward = transform.forward;


        // Update the current waypoint index if the car is close enough to the waypoint
        if (Vector3.Distance(transform.position, nextWaypoint.transform.position) < 10f)
        {
            PlayTurnSign(waypoints[currentWaypointIndex]);
            currentWaypointIndex++;

            Debug.Log("asda");
        }
    }


    public void PlayTurnSign(TurnSign TurnSign)
    {
        turnsign.sprite = TurnSign.gambar;
        template.SetActive(true);

        StartCoroutine(MatiinDialogBox(delay));
    }

    IEnumerator MatiinDialogBox(float delay)
    {
        float jittertime = delay/12;
        for(int i = 0; i < 6; i++)
        { 
            turnsign.enabled = false;
            yield return new WaitForSeconds(jittertime);
            turnsign.enabled = true;
            yield return new WaitForSeconds(jittertime);

            Debug.Log("trigger?");
        }
        template.SetActive(false);
    }
}
