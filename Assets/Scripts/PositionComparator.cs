using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PositionComparator : MonoBehaviour
{
    [Header("Komparasi")]
    public CarDirectionChecker OurWaypoint;
    public List<AICarDirectionChecker> EnemyWaypoints;

    [Header("Reference")]
    public Image posisiHolder;

    [Header("Image Posisi")]
    public Sprite FirstPlace;
    public Sprite SecondPlace;
    public Sprite ThirdPlace;
    public Sprite FourthPlace;
    public Sprite FifthPlace;
    public Sprite SixthPlace;

    public int position;
    public int positiondisplay;
    public bool menang;

    private void Awake()
    {
        
    }
    private void Update()
    {
        CheckforPosition();
    }

    public void CheckforPosition()
    {
        foreach(var waypoint in EnemyWaypoints)
        {
            if(OurWaypoint.currentWaypointIndex <= waypoint.currentWaypointIndex)
            {
                position++;
            }
        }

        switch(position)
        {
            case 0:
                posisiHolder.sprite = FirstPlace;
                menang = true;
                break;
            case 1:
                posisiHolder.sprite = SecondPlace;
                menang = false;
                break;
            case 2:
                posisiHolder.sprite = ThirdPlace;
                menang = false;
                break;
            case 3:
                posisiHolder.sprite = FourthPlace;
                menang = false;
                break;
            case 4:
                posisiHolder.sprite = FifthPlace;
                menang = false;
                break;
            default:
                posisiHolder.sprite = SixthPlace;
                menang = false;
                break;
        }

        if(positiondisplay != position+1)
        {
            positiondisplay = position+1;
        }
        position = 0;
    }

}
