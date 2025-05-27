using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUpdater : MonoBehaviour
{
     public AudioClip[] newClip;
     public AudioSource audioSource;
     
     public void ChangeAudioClip(int clipIndex)
     {
         // Mengganti audio clip dengan clip baru
         audioSource.clip = newClip[clipIndex];
         audioSource.Play();
     }
}
