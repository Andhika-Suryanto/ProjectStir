using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerAudioOnButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button btn;

     void Start()
    {
        btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(PlaySound);

    }

    void PlaySound()
    {
        GameManager.Instance.PlayAudioButton();
    }
}
