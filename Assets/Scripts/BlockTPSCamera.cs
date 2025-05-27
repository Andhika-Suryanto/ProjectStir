using UnityEngine;

public class BlockTPSCamera : MonoBehaviour
{
    public RCC_Camera rccCamera;

    private void Update()
    {
        if (rccCamera == null) return;

        // Jika kamera masuk mode TPS, ganti ke CINEMATIC
        if (rccCamera.cameraMode == RCC_Camera.CameraMode.TPS)
        {
            rccCamera.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);
        }
    }
}
