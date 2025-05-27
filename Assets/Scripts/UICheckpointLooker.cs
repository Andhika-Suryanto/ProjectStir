using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICheckpointLooker : MonoBehaviour
{
    public RectTransform arrowUI; // The UI arrow inside a Canvas

    public void SetUIRotation(float angle)
    {
        // Rotate the UI arrow (Z-axis in UI space)
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
