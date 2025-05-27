using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager_PS1 : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private Transform yourHighScore;

    public WinHandler WinHandler;

    [Header("Edit LeaderBoard")]
    public float templateheight = 50f;
    public int JumlahHS = 5;

    [Header("Variabel Yang Jadi Perhitungan")]
    public float score = 0f; // jumlah checkpoint
    public float damage = 0f;
    public string nama;

    [Header("YoursHighScore")]
    public int yourpos;
    public float yourscore;
    public string yourname;

    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        // Ambil data dari GameManager
        score = GameManager.Instance.expCheckPoint;
        nama = GameManager.Instance.myName;
        damage = GameManager.Instance.expDamage;

        // Simpan skor terbaru
        Save(score, damage, nama);

        // Ambil data terakhir yang disimpan (bukan top skor)
        var saveObj = Load();
        if (saveObj != null && saveObj.SaveObjectArray.Length > 0)
        {
            var lastInserted = saveObj.SaveObjectArray[saveObj.SaveObjectArray.Length - 1];
            DisplayYourHighScore(lastInserted.score, lastInserted.name);
        }

        // Tampilkan leaderboard top N
        if (saveObj != null)
        {
            highscoreEntryTransformList = new List<Transform>();
            foreach (HighScoreEntry.SaveObject highScoreSaveObject in saveObj.SaveObjectArray)
            {
                CreateHighScoreEntryTransform(highScoreSaveObject, entryContainer, highscoreEntryTransformList);
            }
        }

        // Nonaktifkan arrow pada semua mobil
        DisableAllCarArrows();
    }

    public void CreateHighScoreEntryTransform(HighScoreEntry.SaveObject highScoreEntry, Transform Container, List<Transform> transformList)
    {
        if (transformList.Count <= (JumlahHS - 1))
        {
            Transform entryTransform = Instantiate(entryTemplate, Container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateheight * transformList.Count);

            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default: rankString = rank + "th"; break;
                case 1: rankString = "1st"; break;
                case 2: rankString = "2nd"; break;
                case 3: rankString = "3rd"; break;
            }

            entryTransform.Find("PosText").GetComponent<TMP_Text>().text = rankString;
            entryTransform.Find("TimerText").GetComponent<TMP_Text>().text = highScoreEntry.score.ToString();
            entryTransform.Find("NameText").GetComponent<TMP_Text>().text = highScoreEntry.name;

            transformList.Add(entryTransform);
        }
    }

    public void Save(float score, float damage, string name)
    {
        List<HighScoreEntry.SaveObject> SaveObjectList = new List<HighScoreEntry.SaveObject>();
        SaveObject saveObjects = Load();

        if (saveObjects != null)
        {
            foreach (HighScoreEntry.SaveObject HighScoreSaveObject in saveObjects.SaveObjectArray)
            {
                SaveObjectList.Add(HighScoreSaveObject);
            }
        }

        HighScoreEntry.SaveObject newSaveObject = new HighScoreEntry.SaveObject
        {
            score = score,
            damage = damage,
            name = name
        };
        SaveObjectList.Add(newSaveObject);

        // Sorting leaderboard
        for (int i = 0; i < SaveObjectList.Count; i++)
        {
            for (int j = i + 1; j < SaveObjectList.Count; j++)
            {
                if (SaveObjectList[j].score > SaveObjectList[i].score ||
                    (SaveObjectList[j].score == SaveObjectList[i].score && SaveObjectList[j].damage > SaveObjectList[i].damage))
                {
                    var temp = SaveObjectList[i];
                    SaveObjectList[i] = SaveObjectList[j];
                    SaveObjectList[j] = temp;
                }
            }
        }

        SaveObject saveObject = new SaveObject { SaveObjectArray = SaveObjectList.ToArray() };
        SaveSystem.SaveObject("ps1", saveObject, true);
    }

    public SaveObject Load()
    {
        return SaveSystem.LoadObject<SaveObject>("ps1");
    }

    public class SaveObject
    {
        public HighScoreEntry.SaveObject[] SaveObjectArray;
    }

    public class HighScoreEntry
    {
        public float score;
        public float damage;
        public string name;

        [System.Serializable]
        public class SaveObject
        {
            public float score;
            public float damage;
            public string name;
        }

        public SaveObject Save()
        {
            return new SaveObject
            {
                score = score,
                damage = damage,
                name = name
            };
        }
    }

    public void DisplayYourHighScore(float score, string name)
    {
        yourHighScore = transform.Find("YourHighScore");

        if (yourHighScore != null)
        {
            var scoreText = yourHighScore.Find("YourScore")?.GetComponent<TMP_Text>();
            var nameText = yourHighScore.Find("YourName")?.GetComponent<TMP_Text>();

            if (scoreText != null) scoreText.text = score.ToString();
            if (nameText != null) nameText.text = name;
        }
        else
        {
            Debug.LogWarning("YourHighScore object not found!");
        }
    }

    // 🔻 Nonaktifkan racing_chevron_svg di semua mobil saat leaderboard tampil
    public void DisableAllCarArrows()
    {
        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject car in allCars)
        {
            Transform arrow = car.transform.Find("CarCanvas/racing_chevron_svg");
            if (arrow != null)
            {
                arrow.gameObject.SetActive(false);
            }
        }
    }

    // 🔺 Aktifkan kembali racing_chevron_svg saat restart
    public void EnableAllCarArrows()
    {
        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject car in allCars)
        {
            Transform arrow = car.transform.Find("CarCanvas/racing_chevron_svg");
            if (arrow != null)
            {
                arrow.gameObject.SetActive(true);
            }
        }
    }
}
