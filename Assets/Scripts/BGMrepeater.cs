using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMrepeater : MonoBehaviour
{
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
