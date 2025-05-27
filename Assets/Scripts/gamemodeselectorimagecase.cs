using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameModeUISelector : MonoBehaviour
{
    [Header("UI Selector Grid (2 Rows x 3 Columns)")]
    public Image[] gameModeImages; // Total 6 Images
    public string[] sceneNames;
    public int columns = 3; // 3 kolom, otomatis 2 baris

    [Header("Loading Screen & Camera")]
    public GameObject loadingScreen;
    public Transform cameraTargetPosition;
    public float cameraMoveSpeed = 2f;

    [Header("Car Selection")]
    public GameObject[] carModels;

    [Header("UI Management")]
    public List<GameObject> objectsToDisable;
    public List<GameObject> carSelectionUIs;
    public List<GameObject> postCarSelectionUIs;
    public Transform finalCameraPosition;

    private int currentIndex = 0;
    private Coroutine blinkCoroutine;
    private Image currentlySelectedImage;
    private bool isCarSelectionActive = false;
    private int carIndex = 0;
    private bool isMovingCamera = false;
    private bool isFinalTransition = false;

    private void Start()
    {
        if (gameModeImages.Length == 0) return;

        ResetAllImageAlpha();
        currentIndex = 0;
        currentlySelectedImage = gameModeImages[currentIndex];
        StartBlinking();

        foreach (var ui in carSelectionUIs)
            ui?.SetActive(false);

        foreach (var ui in postCarSelectionUIs)
            ui?.SetActive(false);
    }

    private void Update()
    {
        if (isMovingCamera || isFinalTransition) return;

        if (!isCarSelectionActive)
        {
            HandleModeNavigation();
        }
        else
        {
            HandleCarSelection();
        }
    }

    private void HandleModeNavigation()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveSelector(-1, 0);
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveSelector(1, 0);
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            MoveSelector(0, -1);
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            MoveSelector(0, 1);

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(MoveCameraToCarSelection());
        }
    }

    private void MoveSelector(int dx, int dy)
    {
        int rows = Mathf.CeilToInt((float)gameModeImages.Length / columns);
        int row = currentIndex / columns;
        int col = currentIndex % columns;

        row = Mathf.Clamp(row + dy, 0, rows - 1);
        col = Mathf.Clamp(col + dx, 0, columns - 1);

        int newIndex = row * columns + col;
        if (newIndex >= gameModeImages.Length) return;

        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (currentlySelectedImage != null) SetImageAlpha(currentlySelectedImage, 1f);

        currentIndex = newIndex;
        currentlySelectedImage = gameModeImages[currentIndex];
        StartBlinking();
    }

    private IEnumerator MoveCameraToCarSelection()
    {
        isMovingCamera = true;
        foreach (var obj in objectsToDisable) obj?.SetActive(false);

        while (Vector3.Distance(Camera.main.transform.position, cameraTargetPosition.position) > 0.1f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTargetPosition.position, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        isMovingCamera = false;
        isCarSelectionActive = true;
        carModels[carIndex].SetActive(true);

        foreach (var ui in carSelectionUIs)
            ui?.SetActive(true);
    }

    private void HandleCarSelection()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            ChangeCar(-1);
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            ChangeCar(1);

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlayerPrefs.SetInt("SelectedCar", carIndex);
            StartCoroutine(HandlePostCarSelection());
        }
    }

    private void ChangeCar(int dir)
    {
        carModels[carIndex].SetActive(false);
        carIndex = (carIndex + dir + carModels.Length) % carModels.Length;
        carModels[carIndex].SetActive(true);
    }

    private IEnumerator HandlePostCarSelection()
    {
        isFinalTransition = true;

        foreach (var ui in carSelectionUIs)
            ui?.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        foreach (var ui in postCarSelectionUIs)
            ui?.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (finalCameraPosition != null)
            Camera.main.transform.position = finalCameraPosition.position;

        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadSelectedScene());
    }

    private IEnumerator LoadSelectedScene()
    {
        if (sceneNames.Length <= currentIndex)
        {
            Debug.LogError("Scene index invalid!");
            yield break;
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[currentIndex]);
        while (!async.isDone) yield return null;
    }

    private void StartBlinking()
    {
        if (currentlySelectedImage != null)
            blinkCoroutine = StartCoroutine(BlinkingEffect());
    }

    private IEnumerator BlinkingEffect()
    {
        while (true)
        {
            for (float t = 1f; t >= 0.5f; t -= Time.deltaTime * 2)
            {
                SetImageAlpha(currentlySelectedImage, t);
                yield return null;
            }
            for (float t = 0.5f; t <= 1f; t += Time.deltaTime * 2)
            {
                SetImageAlpha(currentlySelectedImage, t);
                yield return null;
            }
        }
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    private void ResetAllImageAlpha()
    {
        foreach (var img in gameModeImages)
            SetImageAlpha(img, 1f);
    }
}