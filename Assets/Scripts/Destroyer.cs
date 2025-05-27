using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public void Destroy(float destroyDuration)
    {
        // Contoh: Menghancurkan GameObject ini setelah 5 detik
        Destroy(gameObject, destroyDuration);
    }
}
