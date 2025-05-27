using System.Collections;
using UnityEngine;

public class CameraLoopManager : MonoBehaviour
{
    public RCC_Camera Cam;
    public RCC_CarControllerV3 car;
    public float switchDelay = 3f;

    private bool looping = true;

    public void StopLooping()
    {
        looping = false;
    }

    public void StartLooping()
    {
        if (!looping)
        {
            looping = true;
            StartCoroutine(LoopCameraModes());
        }
    }

    private void Awake()
    {
        if (Cam && car)
        {
            Cam.useAutoChangeCamera = false;
            Cam.SetTarget(car);
        }

        StartCoroutine(LoopCameraModes());
    }

    IEnumerator LoopCameraModes()
    {
        while (looping)
        {
            Cam.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);
            yield return new WaitForSeconds(switchDelay);

            if (!looping) break;

            Cam.ChangeCamera(RCC_Camera.CameraMode.TOP);
            yield return new WaitForSeconds(switchDelay);

            if (!looping) break;

            Cam.ChangeCamera(RCC_Camera.CameraMode.WHEEL);
            yield return new WaitForSeconds(switchDelay);
        }
    }

    private void Update()
    {
        if (!looping) return;

        // Blok TPS jika tiba-tiba masuk
        if (Cam.cameraMode == RCC_Camera.CameraMode.TPS)
        {
            Cam.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);
        }
    }
}
