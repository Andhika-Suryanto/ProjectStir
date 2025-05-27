using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPToHealthBarScaler : MonoBehaviour
{
    [Header("References")]
    public TMP_Text healthText;               // TMP that holds HP as number (0–100)
    public RectTransform healthBarUI;         // UI RectTransform to resize & reposition

    [Header("Health Bar Settings")]
    public float fullWidth = 200f;            // X scale when HP = 100
    public float baseXPosition = 0f;          // Default X position when full HP

    [Header("Fixed Position Lock (Local)")]
    public float fixedY = 0f;
    public float fixedZ = 0f;

    private int lastHP = -1;

    void Update()
    {
        if (healthText == null || healthBarUI == null)
            return;

        if (int.TryParse(healthText.text, out int currentHP))
        {
            currentHP = Mathf.Clamp(currentHP, 0, 100); // Just in case

            if (currentHP != lastHP)
            {
                float newWidth = fullWidth * (currentHP / 100f);
                float offsetX = (fullWidth - newWidth) / 2f; // To keep left side anchored

                // Resize width
                Vector2 size = healthBarUI.sizeDelta;
                size.x = newWidth;
                healthBarUI.sizeDelta = size;

                // Adjust position
                Vector3 newPos = healthBarUI.localPosition;
                newPos.x = baseXPosition - offsetX;
                newPos.y = fixedY;
                newPos.z = fixedZ;
                healthBarUI.localPosition = newPos;

                lastHP = currentHP;
            }
        }
    }
}
