using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameModeSelector : MonoBehaviour
{
    [Header("Game Mode Selection")]
    public SpriteRenderer[] gameModeSprites;
    public string[] sceneNames;

    [Header("Loading Screen & Camera")]
    public GameObject loadingScreen;
    public Transform cameraTargetPosition;
    public float cameraMoveSpeed = 2f;

    [Header("Car Selection")]
    public GameObject[] carModels;

    [Header("UI & Object Management")]
    public List<GameObject> objectsToDisable;
    public List<GameObject> carSelectionUIs; // ✅ List of car selection UI objects
    public List<GameObject> postCarSelectionUIs; // ✅ List of post car selection UI objects
    public Transform finalCameraPosition;

    private int carIndex = 0;
    private bool isCarSelectionActive = false;
    private int currentIndex = 0;
    private Coroutine blinkCoroutine;
    private SpriteRenderer currentlySelectedSprite;
    private bool isMovingCamera = false;
    private bool isFinalTransition = false;

    private void Start()
    {
        if (gameModeSprites.Length == 0) return;

        ResetAllSpritesAlpha();
        currentIndex = 0;
        currentlySelectedSprite = gameModeSprites[currentIndex];

        StartBlinking();

        // ✅ Hide UI elements at start
        foreach (var ui in carSelectionUIs)
            if (ui != null) ui.SetActive(false);

        foreach (var ui in postCarSelectionUIs)
            if (ui != null) ui.SetActive(false);
    }

    private void Update()
    {
        if (isMovingCamera || isFinalTransition) return;

        if (!isCarSelectionActive)
        {
            HandleGameModeSelection();
        }
        else
        {
            HandleCarSelection();
        }
    }

    private void HandleGameModeSelection()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.left.wasPressedThisFrame == true)
        {
            ChangeSelection(-1);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.right.wasPressedThisFrame == true)
        {
            ChangeSelection(1);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame ||
            Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
        {
            StartCoroutine(MoveCameraToCarSelection());
        }
    }

    private IEnumerator MoveCameraToCarSelection()
    {
        isMovingCamera = true;
        DisableObjects();

        while (Vector3.Distance(Camera.main.transform.position, cameraTargetPosition.position) > 0.1f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTargetPosition.position, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        isCarSelectionActive = true;
        isMovingCamera = false;

        EnableCarSelection();

        // ✅ Show car selection UI after camera stops moving
        foreach (var ui in carSelectionUIs)
            if (ui != null) ui.SetActive(true);
    }

    private void DisableObjects()
    {
        foreach (var obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void EnableCarSelection()
    {
        if (carModels.Length == 0) return;

        foreach (var car in carModels)
        {
            car.SetActive(false);
        }
        carModels[carIndex].SetActive(true);
    }

    private void HandleCarSelection()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.left.wasPressedThisFrame == true)
        {
            ChangeCarSelection(-1);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.right.wasPressedThisFrame == true)
        {
            ChangeCarSelection(1);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame ||
            Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
        {
            PlayerPrefs.SetInt("SelectedCar", carIndex);
            StartCoroutine(HandlePostCarSelection());
        }
    }

    private void ChangeCarSelection(int direction)
    {
        carModels[carIndex].SetActive(false);

        carIndex += direction;
        if (carIndex < 0) carIndex = carModels.Length - 1;
        if (carIndex >= carModels.Length) carIndex = 0;

        carModels[carIndex].SetActive(true);
    }

    private IEnumerator HandlePostCarSelection()
    {
        isFinalTransition = true;

        // ✅ Hide all car selection UI objects
        foreach (var ui in carSelectionUIs)
            if (ui != null) ui.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        // ✅ Show all post-car selection UI objects
        foreach (var ui in postCarSelectionUIs)
            if (ui != null) ui.SetActive(true);

        yield return new WaitForSeconds(1f);

        // ✅ Teleport camera to final position
        if (finalCameraPosition)
        {
            Camera.main.transform.position = finalCameraPosition.position;
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(LoadSelectedScene());
    }
    private IEnumerator LoadSelectedScene()
    {
        if (sceneNames.Length > currentIndex)
        {
            string sceneToLoad = sceneNames[currentIndex];

            if (loadingScreen != null)
            {
                loadingScreen.SetActive(true);
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            if (asyncLoad == null)
            {
                Debug.LogError("🚨 Scene loading failed!");
                yield break;
            }

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogError("🚨 Invalid scene index!");
        }
    }

    private void ChangeSelection(int direction)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (currentlySelectedSprite != null)
        {
            SetSpriteAlpha(currentlySelectedSprite, 1f);
        }

        currentIndex += direction;
        if (currentIndex < 0) currentIndex = gameModeSprites.Length - 1;
        if (currentIndex >= gameModeSprites.Length) currentIndex = 0;

        currentlySelectedSprite = gameModeSprites[currentIndex];

        StartBlinking();
    }

    private void StartBlinking()
    {
        if (currentlySelectedSprite != null)
        {
            blinkCoroutine = StartCoroutine(BlinkingEffect());
        }
    }

    private IEnumerator BlinkingEffect()
    {
        while (true)
        {
            for (float t = 1f; t >= 0.5f; t -= Time.deltaTime * 2)
            {
                SetSpriteAlpha(currentlySelectedSprite, t);
                yield return null;
            }

            for (float t = 0.5f; t <= 1f; t += Time.deltaTime * 2)
            {
                SetSpriteAlpha(currentlySelectedSprite, t);
                yield return null;
            }
        }
    }

    private void SetSpriteAlpha(SpriteRenderer sprite, float alpha)
    {
        if (sprite != null)
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }

    private void ResetAllSpritesAlpha()
    {
        foreach (var sprite in gameModeSprites)
        {
            SetSpriteAlpha(sprite, 1f);
        }
    }
}
