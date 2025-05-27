using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraViewCoop : MonoBehaviour
{
    [Header("Player 1 Settings")]
    public RCC_Camera player1Cam;
    public RCC_CarControllerV3 player1Car;

    [Header("Player 2 Settings")]
    public RCC_Camera player2Cam;
    public RCC_CarControllerV3 player2Car;

    private void Awake()
    {
        if (player1Cam != null && player1Car != null)
        {
            player1Cam.SetTarget(player1Car);
            StartCoroutine(ChangeTarget(player1Cam, player1Car));
        }

        if (player2Cam != null && player2Car != null)
        {
            player2Cam.SetTarget(player2Car);
            StartCoroutine(ChangeTarget(player2Cam, player2Car));
        }

        FixAudioListeners();
    }

    IEnumerator ChangeTarget(RCC_Camera cam, RCC_CarControllerV3 car)
    {
        yield return new WaitForEndOfFrame();
        cam.SetTarget(car);
        yield return new WaitForSeconds(0.1f);
        cam.SetTarget(car);
    }

    public void ChangePerspective(float time, int player)
    {
        if (player == 1 && player1Cam != null)
            StartCoroutine(ChangePerspective(player1Cam, time));

        if (player == 2 && player2Cam != null)
            StartCoroutine(ChangePerspective(player2Cam, time));
    }

    IEnumerator ChangePerspective(RCC_Camera cam, float time)
    {
        cam.useAutoChangeCamera = false;
        cam.cameraMode = RCC_Camera.CameraMode.WHEEL;
        yield return new WaitForSeconds(time);
        cam.cameraMode = RCC_Camera.CameraMode.TOP;
        yield return new WaitForSeconds(time);
        cam.cameraMode = RCC_Camera.CameraMode.TPS;
    }

    private void FixAudioListeners()
    {
        if (player1Cam != null)
        {
            AudioListener listener1 = player1Cam.GetComponent<AudioListener>();
            if (listener1 != null) listener1.enabled = false;
        }

        if (player2Cam != null)
        {
            AudioListener listener2 = player2Cam.GetComponent<AudioListener>();
            if (listener2 != null) listener2.enabled = true; // Only enable one AudioListener
        }
    }
}
