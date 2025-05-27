using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkImage : MonoBehaviour
{
    private Image image; 
    public float blinkInterval = 0.5f; // Time in seconds between changes
    private bool isFaded = false;

    void Start()
    {
        image = GetComponent<Image>(); // Get the Image component automatically
        if (image == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
            return;
        }

        StartCoroutine(BlinkLoop());
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            SetOpacity(isFaded ? 1f : 0.5f); // Toggle between full and half opacity
            isFaded = !isFaded;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    void SetOpacity(float alpha)
    {
        Color newColor = image.color;
        newColor.a = alpha;
        image.color = newColor;
    }
}

