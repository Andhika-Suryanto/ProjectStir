using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static HighScoreManager;

public class WinHandler : MonoBehaviour
{
    public GameObject LeaderBoardPanel;
    public HighScoreManager HighScoreManager;
    public PositionComparator PositionComparator;
    public GameManager GameManager;

    public GameObject KaloMenang;
    public GameObject KaloKalah;

    public bool gameHasStarted = false;
    public TMP_InputField InputField;

    public float finishTime;
    public float totalwaktu = 0;
    public string nama = "Stipen";
    public bool menang;
    
    private void Start()
    {
        nama = "Stipen";
        //Debug.Log(totalwaktu);
        //PlayerPrefs.DeleteKey("highscoreTable");
    }

    private void Update()
    {
        nama = InputField.text;
        if(gameHasStarted)
        {
            totalwaktu += Time.deltaTime;
        }
        //if (Input.GetKeyDown(KeyCode.CapsLock))
        //{
        //    Debug.Log("KeDelete");
        
        //}
    }

    public void isTimer(float delay = 0f)
    {
        StartCoroutine(StartTimer(delay));
    }

    IEnumerator StartTimer(float delay = 0f)

    {
        yield return new WaitForSeconds(delay);
        gameHasStarted = true;
    }


    //buat finish line
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            Debug.Log("nabrak");
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Player"))
        {
            finishTime = totalwaktu;
            GameManager.gameOver();
            GameBerakhir();
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    public void GameBerakhir()
    {
        menang = PositionComparator.menang;
        //Time.timeScale = 0;
        if(menang == true)
        {
            KaloMenang.SetActive(true);
            KaloKalah.SetActive(false);
        }
        else
        {
            KaloKalah.SetActive(true);
            KaloMenang.SetActive(false);
        }
    }

    public void SaveScore()
    {
        LeaderBoardPanel.SetActive(true);
    }
}
