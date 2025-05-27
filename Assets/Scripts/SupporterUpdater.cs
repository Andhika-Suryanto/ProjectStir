using UnityEngine;
using TMPro; // Import namespace TextMeshPro
using UnityEngine.UI;

public class SupporterUpdater : MonoBehaviour
{
    public TMP_Text nama;
    public TMP_Text chat;
    public Image image;

    public AudioClip effectSound;

    private AudioClip clip;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }


    public void SetSupporter(SupporterVoice s)
    {
        nama.text = s.name;
        chat.text = s.chat;
        image.sprite = s.img;
        clip = s.sound;
        source.PlayOneShot(clip);
        source.PlayOneShot(effectSound);
    }
}
