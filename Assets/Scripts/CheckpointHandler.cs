using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kena!");
            
            CheckPointManager.getInstance().randomNewCheckPoint();
            GameManager.Instance.addExpCheckPoint();
            
        }
    }
}
