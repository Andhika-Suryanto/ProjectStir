using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UnifiedGameSelector : MonoBehaviour
{
    [Header("Mode Selection")]
    public Button[] modeButtons;
    public string[] sceneNames;

    [Header("Car Selection")]
    public Button selectLeftButton;
    public Button selectRightButton;
    public Button backButton;
    public Button startButton;

    [Header("Car and Stats")]
    public GameObject[] carModels;
    public GameObject[] carStats;
    public GameObject carCard;

    [Header("UI & Camera")]
    public GameObject loadingScreen;
    public List<GameObject> objectsToDisable;
    public List<GameObject> carSelectionUIs;
    public List<GameObject> postCarSelectionUIs;
    public Transform cameraToCarSelectPosition;
    public Transform finalCameraPosition;
    public float cameraMoveSpeed = 2f;

    [Header("Cameras")]
    public GameObject rccCameraObject;
    public GameObject carSelectionCameraObject;
    public Transform carSelectionCameraTransform; // Drag transform of CarSelectionCamera here

    private int modeIndex = 0;
    private int carIndex = 0;
    private bool isInCarSelection = false;
    private bool isCameraMoving = false;
    private bool isTransitioning = false;

    private Coroutine blinkCoroutine;
    private Graphic currentBlinkGraphic;

    private void Start()
    {
        isInCarSelection = false;
        modeIndex = 0;
        carIndex = 0;

        foreach (var ui in carSelectionUIs) ui?.SetActive(false);
        foreach (var ui in postCarSelectionUIs) ui?.SetActive(false);
        if (carCard != null) carCard.SetActive(false);

        for (int i = 0; i < carModels.Length; i++)
        {
            carModels[i].SetActive(i == 0);
        }

        for (int i = 0; i < carStats.Length; i++)
        {
            carStats[i].SetActive(false);
        }

        if (rccCameraObject != null) rccCameraObject.SetActive(true);
        if (carSelectionCameraObject != null) carSelectionCameraObject.SetActive(false);

        SelectButton(modeButtons, modeIndex);
    }

    private void Update()
    {
        if (isCameraMoving || isTransitioning) return;

        if (!isInCarSelection)
        {
            HandleModeInput();
        }
        else
        {
            HandleCarSelectionInput();
        }
    }

    void HandleModeInput()
    {
        int direction = 0;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.left.wasPressedThisFrame == true) direction = -1;
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.right.wasPressedThisFrame == true) direction = 1;

        if (direction != 0)
        {
            modeIndex = (modeIndex + direction + modeButtons.Length) % modeButtons.Length;
            SelectButton(modeButtons, modeIndex);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
        {
            StartCoroutine(TransitionToCarSelection());
        }
    }

    IEnumerator TransitionToCarSelection()
    {
        isCameraMoving = true;

        if (rccCameraObject != null) rccCameraObject.SetActive(false);
        if (carSelectionCameraObject != null) carSelectionCameraObject.SetActive(true);

        foreach (var obj in objectsToDisable) obj?.SetActive(false);

        while (Vector3.Distance(Camera.main.transform.position, cameraToCarSelectPosition.position) > 0.05f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraToCarSelectPosition.position, Time.deltaTime * cameraMoveSpeed);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraToCarSelectPosition.rotation, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        isCameraMoving = false;
        isInCarSelection = true;

        foreach (var ui in carSelectionUIs) ui?.SetActive(true);
        if (carCard != null) carCard.SetActive(true);

        SetActiveCarAndStats(carIndex, true);
        SelectSingleButton(selectLeftButton);
    }

    void HandleCarSelectionInput()
    {
        var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (currentSelected != null)
        {
            Button btn = currentSelected.GetComponent<Button>();
            if (btn != null && btn.targetGraphic != null)
            {
                if (btn.targetGraphic != currentBlinkGraphic)
                {
                    if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
                    ResetAllButtonAlphas();
                    currentBlinkGraphic = btn.targetGraphic;
                    blinkCoroutine = StartCoroutine(Blink(currentBlinkGraphic));
                }
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
        {
            var selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                if (selected == selectLeftButton.gameObject)
                {
                    carIndex = (carIndex - 1 + carModels.Length) % carModels.Length;
                    SetActiveCarAndStats(carIndex, true);
                }
                else if (selected == selectRightButton.gameObject)
                {
                    carIndex = (carIndex + 1) % carModels.Length;
                    SetActiveCarAndStats(carIndex, true);
                }
                else if (selected == backButton.gameObject)
                {
                    StartCoroutine(MoveCameraBackToModeSelection());
                }
                else if (selected == startButton.gameObject)
                {
                    PlayerPrefs.SetInt("SelectedCar", carIndex);
                    StartCoroutine(HandlePostCarSelection());
                }
            }
        }
    }

    IEnumerator MoveCameraBackToModeSelection()
    {
        isCameraMoving = true;

        foreach (var ui in carSelectionUIs) ui?.SetActive(false);
        foreach (var ui in postCarSelectionUIs) ui?.SetActive(false);
        if (carCard != null) carCard.SetActive(false);

        // Aktifkan RCC Camera, matikan CarSelection Camera
        if (carSelectionCameraObject != null) carSelectionCameraObject.SetActive(false);
        if (rccCameraObject != null) rccCameraObject.SetActive(true);

        // Salin posisi dari CarSelectionCamera ke Camera.main
        if (carSelectionCameraTransform != null)
        {
            Camera.main.transform.position = carSelectionCameraTransform.position;
            Camera.main.transform.rotation = carSelectionCameraTransform.rotation;
        }

        foreach (var obj in objectsToDisable) obj?.SetActive(true);
        isCameraMoving = false;
        isInCarSelection = false;

        SelectButton(modeButtons, modeIndex);

        yield break; // menghindari CS0161
    }

    IEnumerator HandlePostCarSelection()
    {
        isTransitioning = true;

        foreach (var ui in carSelectionUIs) ui?.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        foreach (var ui in postCarSelectionUIs) ui?.SetActive(true);
        yield return new WaitForSeconds(1f);

        if (finalCameraPosition) Camera.main.transform.position = finalCameraPosition.position;

        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadSceneByMode());
    }

    IEnumerator LoadSceneByMode()
    {
        if (sceneNames.Length > modeIndex)
        {
            loadingScreen?.SetActive(true);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[modeIndex]);
            while (!async.isDone) yield return null;
        }
    }

    void SelectButton(Button[] buttons, int index)
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        ResetAllButtonAlphas(buttons);
        currentBlinkGraphic = buttons[index].targetGraphic;
        blinkCoroutine = StartCoroutine(Blink(currentBlinkGraphic));
        buttons[index].Select();
    }

    void SelectSingleButton(Button button)
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        ResetAllButtonAlphas();
        currentBlinkGraphic = button.targetGraphic;
        blinkCoroutine = StartCoroutine(Blink(currentBlinkGraphic));
        button.Select();
    }

    IEnumerator Blink(Graphic graphic)
    {
        while (true)
        {
            for (float t = 1f; t >= 0.5f; t -= Time.deltaTime * 2)
            {
                SetAlpha(graphic, t);
                yield return null;
            }
            for (float t = 0.5f; t <= 1f; t += Time.deltaTime * 2)
            {
                SetAlpha(graphic, t);
                yield return null;
            }
        }
    }

    void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic == null) return;
        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
    }

    void ResetAllButtonAlphas()
    {
        if (modeButtons != null)
        {
            foreach (var btn in modeButtons)
            {
                SetAlpha(btn.targetGraphic, 1f);
            }
        }
        if (selectLeftButton != null) SetAlpha(selectLeftButton.targetGraphic, 1f);
        if (selectRightButton != null) SetAlpha(selectRightButton.targetGraphic, 1f);
        if (backButton != null) SetAlpha(backButton.targetGraphic, 1f);
        if (startButton != null) SetAlpha(startButton.targetGraphic, 1f);
    }

    void ResetAllButtonAlphas(Button[] buttons)
    {
        if (buttons == null) return;
        foreach (var btn in buttons)
        {
            SetAlpha(btn.targetGraphic, 1f);
        }
    }

    void SetActiveCarAndStats(int index, bool active)
    {
        for (int i = 0; i < carModels.Length; i++)
        {
            carModels[i].SetActive(i == index);
        }
        for (int i = 0; i < carStats.Length; i++)
        {
            carStats[i].SetActive(i == index && active);
        }
    }
}
