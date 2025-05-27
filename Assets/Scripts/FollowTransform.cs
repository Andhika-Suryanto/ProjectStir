using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowTransform : MonoBehaviour
{
    public Transform target;
    public Vector3 Offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
        
    }
}
