using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowRotation : MonoBehaviour
{
    public Transform target;
    private void Update()
    {
        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
}
