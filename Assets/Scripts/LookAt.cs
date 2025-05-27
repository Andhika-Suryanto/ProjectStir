using UnityEngine;

public class LookAt : MonoBehaviour
{

    [Header("Object to Look At")]
    public Transform target; // Drag the target object here

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

}
