using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHandler : MonoBehaviour
{
   
    public GameObject[] myInterface;

    public void ChangeInterface(int index)
    {
        for(int i=0; i<myInterface.Length; i++ )
        {
            myInterface[i].SetActive(false);
        }
        myInterface[index].SetActive(true); 

    }

}
