using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SaturationChanger : MonoBehaviour
{
    
    [SerializeField] Volume volume; // Reference to your Volume component
    public float duration = 5f; // Duration for the transition
    public float startValue;
    public float endValue;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        volume = GetComponent<Volume>();

    }

    public void ChangeSaturation()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            StartCoroutine(LerpSaturation(colorAdjustments, startValue, endValue, duration));
        }
    }

    IEnumerator LerpSaturation(ColorAdjustments colorAdj, float startValue, float endValue, float duration)
    {
        float time = 0;
        while(time < duration)
        {
            colorAdj.saturation.value = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        colorAdj.saturation.value = endValue;
    }
}
