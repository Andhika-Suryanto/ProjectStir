using UnityEngine;

public class DeleteAITaggedObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            Destroy(other.gameObject);
        }
    }
}
