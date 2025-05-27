using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber; // Nomor checkpoint, misalnya, 1 atau 2
    public GameObject nextCheckpoint; // Checkpoint berikutnya (untuk mengaktifkannya)

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            // Simpan waktu ketika pemain melewati checkpoint
            float checkpointTime = Time.time;

            // Tandai checkpoint sebagai sudah diaktifkan
            activated = true;

            // Menjadikan checkpoint saat ini nonaktif
            DeactivateCheckpoint();

            // Mengaktifkan checkpoint berikutnya (jika ada)
            if (nextCheckpoint != null)
            {
                nextCheckpoint.GetComponent<Checkpoint>().ActivateCheckpoint();
            }

            // Menghitung dan mencetak waktu dalam detik
/*            float elapsedTime = checkpointTime - GameManager.Instance();
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            Debug.Log("Player melewati checkpoint " + checkpointNumber + " pada waktu: " + minutes + " menit " + seconds + " detik");*/
        }
    }

    // Metode untuk mengaktifkan checkpoint ini
    public void ActivateCheckpoint()
    {
        gameObject.SetActive(true);
    }

    // Metode untuk menonaktifkan checkpoint ini
    public void DeactivateCheckpoint()
    {
        gameObject.SetActive(false);
    }

}
