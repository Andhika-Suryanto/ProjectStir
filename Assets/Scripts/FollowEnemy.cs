using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    public Transform target;
    private void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.eulerAngles = new Vector3(90, 270 + target.eulerAngles.y, 0);
    }
}
