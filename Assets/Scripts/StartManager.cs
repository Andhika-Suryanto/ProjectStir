using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] gameObjectsActive;
    public GameObject[] gameObjectsInactive;

    void Start()
    {
        Debug.Log("Jalan");
        for (int i = 0; i < gameObjectsActive.Length; i++)
        {
            Debug.Log("Jalan" + i);
            gameObjectsActive[i].SetActive(true);
        }
        for (int i = 0; i < gameObjectsInactive.Length; i++)
        {
            Debug.Log("name:"+gameObjectsInactive[i].name);
            gameObjectsInactive[i].SetActive(false);
        }
    }

}
