using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class UIBlinkSelectorMatrix : MonoBehaviour
{
    [Header("Image Layout")]
    public Image image1; // Baris 0
    public Image image2;

    public Image image3; // Baris 1
    public Image image4;
    public Image image5;

    [Header("Blink Settings")]
    public float blinkSpeed = 0.5f;
    public float minAlpha = 0.5f;

    private Image[][] selectionGrid;
    private int rowIndex = 0;
    private int colIndex = 0;
    private Coroutine blinkCoroutine;
    private Image currentlyBlinking;

    void Awake()
    {
        // Baris 0 (2 kolom), Baris 1 (3 kolom)
        selectionGrid = new Image[2][];
        selectionGrid[0] = new Image[] { image1, image2 };
        selectionGrid[1] = new Image[] { image3, image4, image5 };
    }

    void Start()
    {
        ResetAllImagesAlpha();

        currentlyBlinking = selectionGrid[rowIndex][colIndex];
        StartBlink();
    }

    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            ChangeSelection(0, -1);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            ChangeSelection(0, 1);
        }
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            ChangeSelection(-1, 0);
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            ChangeSelection(1, 0);
        }
    }

    void ChangeSelection(int rowDelta, int colDelta)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            SetImageAlpha(currentlyBlinking, 1f);
        }

        // Hitung row baru
        rowIndex = Mathf.Clamp(rowIndex + rowDelta, 0, selectionGrid.Length - 1);

        // Hitung col baru — disesuaikan dengan panjang kolom di row tersebut
        int maxCol = selectionGrid[rowIndex].Length - 1;
        colIndex = Mathf.Clamp(colIndex + colDelta, 0, maxCol);

        // Amankan kalau index terakhir lebih besar dari jumlah kolom baris baru
        if (colIndex > maxCol) colIndex = maxCol;

        currentlyBlinking = selectionGrid[rowIndex][colIndex];
        StartBlink();
    }

    void StartBlink()
    {
        blinkCoroutine = StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        while (true)
        {
            for (float t = 1f; t >= minAlpha; t -= Time.deltaTime * 2)
            {
                SetImageAlpha(currentlyBlinking, t);
                yield return null;
            }

            for (float t = minAlpha; t <= 1f; t += Time.deltaTime * 2)
            {
                SetImageAlpha(currentlyBlinking, t);
                yield return null;
            }
        }
    }

    void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    void ResetAllImagesAlpha()
    {
        foreach (var row in selectionGrid)
        {
            foreach (var img in row)
            {
                SetImageAlpha(img, 1f);
            }
        }
    }
}