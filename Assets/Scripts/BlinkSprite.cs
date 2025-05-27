using UnityEngine;
using System.Collections;

public class BlinkSprite : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float blinkSpeed = 0.3f;
    public float minimumOpacity = 0.5f;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
            return;
        }

        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        while (true)
        {
            // Fade to 50% opacity
            yield return StartCoroutine(FadeTo(minimumOpacity, blinkSpeed));
            // Fade back to 100% opacity
            yield return StartCoroutine(FadeTo(1f, blinkSpeed));
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = sprite.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, targetAlpha);
    }
}
