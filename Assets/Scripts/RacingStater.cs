using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingStater : MonoBehaviour
{

    // Deklarasikan variabel GameObject
    public GameObject objToActivate;

// Start is called before the first frame update
    void Start()
    {
        StartRace();
    }

    private void StartRace()
    {

        // Aktifkan GameObject jika ditemukan
        if (objToActivate != null)
        {
            objToActivate.SetActive(true);
        }

    }
}
