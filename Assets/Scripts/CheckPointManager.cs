using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SupporterVoice
{
    public string name;
    public string chat;
    public AudioClip sound;
    public Sprite img;
}
public class CheckPointManager : MonoBehaviour
{
    public GameObject[] listCheckPoint;
    [SerializeField]
    private int indexGroup;

    [SerializeField]
    private int index;

    public SupporterVoice[] listsupporter;

    private static CheckPointManager _instance;

    public static CheckPointManager getInstance()
    {
        return CheckPointManager._instance;

    }
    void StartGame()
    {
        indexGroup = 0;
        index = 0;
        _instance = this;
        for(int i = 0; i < listCheckPoint.Length; i++)
        {
            listCheckPoint[i].SetActive(false);
        }
        //StartCoroutine(random());
        randomNewCheckPoint();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    IEnumerator random()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            randomNewCheckPoint();
        }
    }


    public void randomNewCheckPoint(int hardMode = 0)
    {
        int oldIndexGroup = indexGroup;
        do
        {
            if (hardMode == 1)
                indexGroup = Random.Range(0, 3);
            else
            {
                indexGroup = Random.Range(-1, 2);
                indexGroup += oldIndexGroup;
            }
        } while (indexGroup == oldIndexGroup || indexGroup < 0 || indexGroup > 2);

        int newIndex;
        do
        {
            newIndex = Random.Range(0, listCheckPoint.Length);
            //print(oldIndexGroup + " " + newIndex + " " + listCheckPoint[newIndex].transform.tag);
        } while (listCheckPoint[newIndex].transform.CompareTag(getTagName(oldIndexGroup)) == true);

        listCheckPoint[index].SetActive(false);
        listCheckPoint[newIndex].SetActive(true);

        index = newIndex;
    }

    public GameObject getCurrentCheckPoint()
    {
        return listCheckPoint[index];
    }

    public string getCurrentTargetTag()
    {
        return getTagName(index);
    }

    private string getTagName(int index)
    {
        string output;
        switch (index)
        {
            case 0: output = "A"; break;
            case 1: output = "B"; break;
            case 2: output = "C"; break;
            default: output = ""; break;
        }
        return output;
    }
    public SupporterVoice getRandomSuppoter()
    {
        return listsupporter[Random.Range(0, listsupporter.Length)];
    }
}