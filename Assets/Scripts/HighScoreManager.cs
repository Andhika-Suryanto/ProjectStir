using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using static HighScoreManager;

/*
 * next thing to do adalah buat listnya 10 doang dan kalo masuk list play animasi or something.
 */
public class HighScoreManager : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighScoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    private Transform yourHighScore;

    public WinHandler WinHandler;
    
    //ini buat ngatur gap antara score
    public float templateheight = 50f;
    public float time = 0f;
    //buat ngatur Total HighScore yang keluar
    public int JumlahHS = 5;
    public string nama;

    [Header("YoursHighScore")]
    public int yourpos;
    public float yourscore;
    public string yourname;
    private void Awake()
    {
        //Debug.Log("Hadir");
        
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        
        entryTemplate.gameObject.SetActive(false);

        ////testing doang
        //Save(3f, "Mikel");
        //Save(6f, "Scotus");
        //Save(5f, "Fajar");
        //Save(1f, "Tipen");

        //masukin ke leaderboard
        time = WinHandler.finishTime;
        nama = WinHandler.nama;

        //kalo menang add
        if (WinHandler.menang == true)
        {
            Save(time, nama);
        }
        //kalo kalah cuma nampilin doang
        DisplayYourHighScore(time, nama);

        #region saveplayerprefs(prasasti)
        ////load disini(playerprefs)
        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //HighScore highscore = JsonUtility.FromJson<HighScore>(jsonString);

        ////show highscore listnya(playerprefs)
        //highscoreEntryTransformList = new List<Transform>();
        //foreach (HighScoreEntry highScoreEntry in highscore.highscoreEntryList)
        //{
        //    CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highscoreEntryTransformList);
        //}
        #endregion

        //load disini(json)
        SaveObject saveObject = Load();
        if(saveObject == null)
        {
            Debug.Log("null");
        }

        //show highscore listnya(json)
        foreach (HighScoreEntry.SaveObject Highscores in saveObject.SaveObjectArray)
        {
            Debug.Log(Highscores.score + Highscores.name);
        }
        if (saveObject != null)
        {
            highscoreEntryTransformList = new List<Transform>();
            foreach (HighScoreEntry.SaveObject highScoreSaveObject in saveObject.SaveObjectArray)
            {
                CreateHighScoreEntryTransform(highScoreSaveObject, entryContainer, highscoreEntryTransformList);
            }
        }
        


        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));


        ////ini buat pas awal doang (udah ga guna)
        //HighScore highscore = new HighScore { highscoreEntryList = highscoreEntryList };
        //string json = JsonUtility.ToJson(highscore);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

    }
   
    public void CreateHighScoreEntryTransform(HighScoreEntry.SaveObject highScoreEntry, Transform Container, List<Transform> transformList)
    {
        //ini buat bikin scorenya
        if(transformList.Count <= (JumlahHS-1))
        {
            Transform entryTransform = Instantiate(entryTemplate, Container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateheight * transformList.Count);

            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "th"; break;

                case 1: rankString = "1st"; break;
                case 2: rankString = "2nd"; break;
                case 3: rankString = "3rd"; break;
            }

            entryTransform.Find("PosText").GetComponent<TMP_Text>().text = rankString;

            float score = highScoreEntry.score;
            entryTransform.Find("TimerText").GetComponent<TMP_Text>().text = FormatTime(score);

            string name = highScoreEntry.name;
            entryTransform.Find("NameText").GetComponent<TMP_Text>().text = name;

            transformList.Add(entryTransform);
        }
        
    }

    public void Save(float score, string name)
    {
        List<HighScoreEntry.SaveObject> SaveObjectList = new List<HighScoreEntry.SaveObject>();
        //masukin semua skor ke save

        //masukin yang lama
        //load
        SaveObject saveObjects = Load();
        //add ke list
        if (saveObjects != null)
        {
            foreach (HighScoreEntry.SaveObject HighScoreSaveObject in saveObjects.SaveObjectArray)
            {
                SaveObjectList.Add(HighScoreSaveObject);
            }
        }

        //masukin yang baru
        //cari tau score + nama
        HighScoreEntry.SaveObject newSaveObject = new HighScoreEntry.SaveObject { score = score, name = name };
        //masukin ke list
        SaveObjectList.Add(newSaveObject);

        //Sorting
        if(saveObjects != null)
        {
            for (int i = 0; i < SaveObjectList.Count; i++)
            {
                for (int j = 0; j < SaveObjectList.Count; j++)
                {
                    if (SaveObjectList[j].score > SaveObjectList[i].score)
                    {
                        //Swap
                        HighScoreEntry.SaveObject temp = SaveObjectList[i];
                        SaveObjectList[i] = SaveObjectList[j];
                        SaveObjectList[j] = temp;
                    }
                }
            }
        }        

        //save
        SaveObject saveObject = new SaveObject { SaveObjectArray = SaveObjectList.ToArray() };
        foreach(HighScoreEntry.SaveObject Highscores in saveObject.SaveObjectArray)
        {
            //Debug.Log(Highscores.score + Highscores.name);
        }
        
        SaveSystem.SaveObject(saveObject);
    }

    public SaveObject Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        return saveObject;
    }

    public class SaveObject
    {
        public HighScoreEntry.SaveObject[] SaveObjectArray;
    }
    public class HighScore
    {
        public List<HighScoreEntry> highscoreEntryList;
    }
    

    //[System.Serializable]
    public class HighScoreEntry
    {
        public float score;
        public string name;

        [System.Serializable]
        public class SaveObject
        {
            public float score;
            public string name;
        }

        public SaveObject Save()
        {
            return new SaveObject
            {
                score = score,
                name = name,
            };
        }
    }

    public string FormatTime(float totalsecond)
    {
        int minute = Mathf.FloorToInt(totalsecond / 60);
        int second = Mathf.FloorToInt(totalsecond % 60);
        int milisecond = Mathf.FloorToInt((totalsecond * 100) % 100);

        return string.Format("{0:D2}:{1:D2}.{2:D2}", minute, second, milisecond);
    }

    public void DisplayYourHighScore(float score, string name)
    {
        yourHighScore = transform.Find("YourHighScore");

        yourHighScore.Find("YourTime").GetComponent<TMP_Text>().text = FormatTime(score);

    }
}
