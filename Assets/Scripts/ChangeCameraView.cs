using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraView : MonoBehaviour
{
    public RCC_Camera Cam;
    public RCC_CarControllerV3 car;
    //public enum CameraMode { TPS, FPS, WHEEL, FIXED, CINEMATIC, TOP }
    //public CameraMode cameraMode;

    public void Awake()
    {
        Cam.SetTarget(car);
        StartCoroutine(ChangeTarget());
    }

    IEnumerator ChangeTarget()
    {
        yield return new WaitForEndOfFrame();
        Cam.SetTarget(car);
        yield return new WaitForSeconds(0.1f);
        Cam.SetTarget(car);
    }

    public void changePerspective(float time)
    {       
        StartCoroutine(ChangePerspective(time));
    }
    IEnumerator ChangePerspective(float time)
    {
        Cam.useAutoChangeCamera = false;
        Cam.cameraMode = RCC_Camera.CameraMode.WHEEL;
        yield return new WaitForSeconds(time);
        Cam.cameraMode = RCC_Camera.CameraMode.TOP;
        yield return new WaitForSeconds(time);
        Cam.cameraMode = RCC_Camera.CameraMode.TPS;
    }
}
