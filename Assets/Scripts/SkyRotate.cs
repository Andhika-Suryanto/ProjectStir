using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotate : MonoBehaviour
{
    public float speed;
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time*speed);
    }
}