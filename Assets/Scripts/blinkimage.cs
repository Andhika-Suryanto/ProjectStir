using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class blinkimage : MonoBehaviour
{
    private Image image;
    public float blinkSpeed = 0.3f;
    public float minimumOpacity = 0.5f;

    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
            return;
        }

        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        while (true)
        {
            // Fade to minimum opacity
            yield return StartCoroutine(FadeTo(minimumOpacity, blinkSpeed));
            // Fade back to full opacity
            yield return StartCoroutine(FadeTo(1f, blinkSpeed));
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            yield return null;
        }

        Color finalColor = image.color;
        finalColor.a = targetAlpha;
        image.color = finalColor;
    }
}