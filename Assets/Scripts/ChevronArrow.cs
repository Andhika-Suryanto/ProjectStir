// Script to recreate the chevron effect
using UnityEngine;
using UnityEngine.UI;

public class ChevronArrow : MonoBehaviour
{
    public Image[] chevrons; // Assign 3 UI Images
    public float animationSpeed = 1.5f;
    
    void Start()
    {
        StartCoroutine(AnimateChevrons());
    }
    
    System.Collections.IEnumerator AnimateChevrons()
    {
        while (true)
        {
            for (int i = 0; i < chevrons.Length; i++)
            {
                StartCoroutine(PulseChevron(chevrons[i], i * 0.3f));
            }
            yield return new WaitForSeconds(animationSpeed);
        }
    }
    
    System.Collections.IEnumerator PulseChevron(Image chevron, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        float duration = 0.75f;
        float timer = 0;
        Color startColor = chevron.color;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.3f, 1f, Mathf.Sin(timer / duration * Mathf.PI));
            chevron.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        
        chevron.color = new Color(startColor.r, startColor.g, startColor.b, 0.3f);
    }
}