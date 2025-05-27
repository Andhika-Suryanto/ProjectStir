
using UnityEngine;

public class DialogBoxFade : MonoBehaviour
{
    public float fadeDuration = 2.0f;  // Duration of the fade effect
    public SpriteRenderer spriteRenderer;
    private float fadeAmount;


    public void FadeOut()
    {
        StartCoroutine(FadeTo(0.0f));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTo(1.0f));
    }

    private System.Collections.IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = spriteRenderer.color.a;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            spriteRenderer.color = newColor;
            yield return null;
        }

        Color finalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
        spriteRenderer.color = finalColor;
    }
}
