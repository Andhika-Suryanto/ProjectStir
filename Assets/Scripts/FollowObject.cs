using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject followedObject;
    public float speed = 2.0f;

    [Header("is Follow Y to?")]
    public bool isY = false;

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(
                        followedObject.transform.position.x,
                        isY ? followedObject.transform.position.y : transform.position.y,
                        followedObject.transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }
}

