using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoopGameManager : MonoBehaviour
{
    public RCC_CarControllerV3 player1Car;
    public RCC_Camera player1Camera;

    public RCC_CarControllerV3 player2Car;
    public RCC_Camera player2Camera;

    public static CoopGameManager Instance; // Singleton instance

    public TMP_Text expText;
    private float startTime; // Start time of the game

    public int expCheckPoint;
    public float expDamage;

    public TMP_Text nameText;
    public string myName;

    public GameObject gameplayPanel;
    public GameObject panelChat;
    public GameObject gameOverPanel;
    public GameObject DashBoardPanel;

    public TMP_Text expTextGameOver;
    public TMP_Text damageTextGameOver;

    public AudioSource buttonAudioSource;
    public AudioClip buttonSound;
    private bool isGameOver;    

    public List<RCC_AICarController> CarAI;
    public List<RCC_CarControllerV3> Cars;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartScene()
    {
        isGameOver = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
        expCheckPoint = 0;
        expText.text = expCheckPoint.ToString();
        myName = string.Empty;
        isGameOver = false;

        if (player1Car) player1Car.enabled = false;
        if (player2Car) player2Car.enabled = false;

        if (player1Camera) player1Camera.enabled = false;
        if (player2Camera) player2Camera.enabled = false;

        panelChat.SetActive(false);
        gameOverPanel.SetActive(false);
        DashBoardPanel.SetActive(false);

        foreach (var EnemyCar in CarAI)
        {
            EnemyCar.enabled = false;
        }
        foreach (var car in Cars)
        {
            car.enabled = false;
        }

        SetupSplitScreen();
    }

    private void SetupSplitScreen()
    {
        if (player1Camera)
        {
            player1Camera.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1); // Left half for Player 1
        }

        if (player2Camera)
        {
            player2Camera.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1); // Right half for Player 2
        }
    }

    private void Update()
    {
        expDamage = GetComponent<Damage>().myDamage;
    }

    public void PlayAudioButton()
    {
        if (buttonAudioSource)
        {
            Debug.Log("Button audio source exists.");
        }
        buttonAudioSource.PlayOneShot(buttonSound);
    }

    public void setCameraTrue()
    {
        if (player1Camera) player1Camera.enabled = true;
        if (player2Camera) player2Camera.enabled = true;
        
        DashBoardPanel.SetActive(true);
    }

    public void canStart(float delay = 0f)
    {
        StartCoroutine(startGame(delay));
    }

    IEnumerator startGame(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        
        if (player1Car) player1Car.enabled = true;
        if (player2Car) player2Car.enabled = true;

        GetComponent<TimerDisplay>().isStart = true;
    }

    public void addExpCheckPoint(int value = 1)
    {
        expCheckPoint += value;
        expText.text = expCheckPoint.ToString();
        playSupporter();
    }

    public void playSupporter()
    {
        SupporterVoice s = CheckPointManager.getInstance().getRandomSuppoter();
        SupporterUpdater t = GetComponent<SupporterUpdater>();
        t.SetSupporter(s);
        panelChat.SetActive(true);
        Invoke("closePanel", 2f);
    }

    public void closePanel()
    {
        panelChat.SetActive(false);
    }

    public void setName()
    {
        myName = GetComponent<InputFieldHandler>().tmpInputField.text;
        nameText.text = myName;
    }

    public void EnableAI(float delay)
    {
        StartCoroutine(AIstarted(delay));
    }

    IEnumerator AIstarted(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var EnemyCar in CarAI)
        {
            EnemyCar.enabled = true;
        }
        foreach (var car in Cars)
        {
            car.enabled = true;
        }
    }

    public void gameOver()
    {
        Debug.Log("Game Over Triggered");
        isGameOver = true;

        if (player1Car) player1Car.enabled = false;
        if (player2Car) player2Car.enabled = false;

        if (player1Camera) player1Camera.enabled = false;
        if (player2Camera) player2Camera.enabled = false;

        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        expTextGameOver.text = expCheckPoint.ToString();
        damageTextGameOver.text = GetComponent<Damage>().myDamage.ToString();
        nameText.text = myName + ", Your Result:";
    }

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    #region SendToGoogleForm

    [Header("GoogleForm")]
    public string entryCodeName;
    public string entryCodeExp;
    public string entryCodeDemage;
    public string urlForm;
    
    public void SendData()
    {
        StartCoroutine(sendingData());
    }

    IEnumerator sendingData()
    {
        int exp = expCheckPoint;
        int damage = GetComponent<Damage>().myDamage;

        WWWForm form = new WWWForm();
        form.AddField(entryCodeName, myName);
        form.AddField(entryCodeExp, exp.ToString());
        form.AddField(entryCodeDemage, damage.ToString());

        UnityWebRequest www = UnityWebRequest.Post(urlForm, form);
        yield return www.SendWebRequest();
    }

    #endregion
}
