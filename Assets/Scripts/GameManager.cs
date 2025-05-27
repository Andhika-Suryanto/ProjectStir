using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RCC_CarControllerV3 car;
    public RCC_Camera rccCamera;

    //[HideInInspector]
    public static GameManager Instance; // Singleton instance

    public TMP_Text expText;
    private float startTime; // Waktu awal permainan

    //[HideInInspector]
    public int expCheckPoint;
    public float expDamage;

    public TMP_Text nameText;
    public string myName;

    public GameObject gameplayPanel;
    public GameObject panelChat;
    public GameObject gameOverPanel;
    public GameObject DashBoardPanel;
    public GameObject buttonrestart;

    public TMP_Text expTextGameOver;
    public TMP_Text damageTextGameOver;

    public AudioSource buttonAudioSource;
    public AudioClip buttonSound;
    private bool isGameOver;    

    public List<RCC_AICarController> CarAI;
    public List<RCC_CarControllerV3> Car;
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

    public void MainMenuScene()
    {
        isGameOver = false;
        SceneManager.LoadScene("Menu");
    }


    private void Start()
    {
        //Debug.Log("DNSAONA");
        buttonAudioSource = GetComponent<AudioSource>();
        expCheckPoint = 0;
        expText.text = expCheckPoint.ToString();
        myName = string.Empty;
        isGameOver = false;
        car.enabled = false;
        rccCamera.enabled = false;
        panelChat.SetActive(false);
        gameOverPanel.SetActive(false);
        DashBoardPanel.SetActive(false);
        
        foreach (var EnemyCar in CarAI)
        {
            EnemyCar.enabled = false;
        }
        foreach(var Car in Car)
        {
            Car.enabled = false;
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
            Debug.Log("button audio source ada");
        }
        buttonAudioSource.PlayOneShot(buttonSound);
    }

    public void setCameraTrue()
    {
        rccCamera.enabled = true;
        DashBoardPanel.SetActive(true);
    }

    public void canStart(float delay = 0f)
    {
        StartCoroutine(startGame(delay));
    }

    IEnumerator startGame(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        car.enabled = true;
        GetComponent<TimerDisplay>().isStart = true;
        //CheckPointManager.getInstance().randomNewCheckPoint(0);
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
        foreach(var EnemyCar in CarAI)
        {
            EnemyCar.enabled = true;
        }
        foreach (var Car in Car)
        {
            Car.enabled = true;
        }
    }
    public void gameOver()
    {
        Debug.Log("Masuk");
        isGameOver = true;
        car.enabled = false;
        rccCamera.enabled = false;
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        DashBoardPanel.SetActive(false);
        buttonrestart.SetActive(false);


        expTextGameOver.text = expCheckPoint.ToString();
        damageTextGameOver.text = GetComponent<Damage>().myDamage.ToString();
        nameText.text = myName + ", Hasil Lo:";

        // SendData();
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
        //bgThankYou.SetActive(true);
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