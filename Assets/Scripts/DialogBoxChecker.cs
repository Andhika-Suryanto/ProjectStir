using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBoxChecker : MonoBehaviour
{
    public DialogBoxTriggerContainer waypointManager; // Reference to the WaypointManager script
    private List<DialogBox> waypoints;
    public int currentWaypointIndex = 0;

    public float delay = 3f;


    [Header("Reference Templatenya")]
    public GameObject template;
    public Image Character;
    public TMP_Text names;
    public TMP_Text ChatJPN;
    public TMP_Text Chat;


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

        DialogBox nextWaypoint = waypoints[currentWaypointIndex];
        Vector3 toWaypoint = nextWaypoint.transform.position - transform.position;
        Vector3 forward = transform.forward;


        // Update the current waypoint index if the car is close enough to the waypoint
        if (Vector3.Distance(transform.position, nextWaypoint.transform.position) < 10f)
        {
            PlayDialogBox(waypoints[currentWaypointIndex]);
            currentWaypointIndex++;
            
            Debug.Log("asda");
        }
    }


    public void PlayDialogBox(DialogBox dialogBox)
    {
        Character.sprite = dialogBox.Char;
        names.text = dialogBox.names;
        Chat.text = dialogBox.Chat;
        ChatJPN.text = dialogBox.ChatJPN;
        template.SetActive(true);
        
        StartCoroutine(MatiinDialogBox(delay));
    }

    IEnumerator MatiinDialogBox(float delay)
    {
  
        yield return new WaitForSeconds(delay);
        template.SetActive(false);
    }
}
