using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivedObject : MonoBehaviour
{
    public void Deactived(int delay)
    {
        StartCoroutine(DoSomethingAfterDelay(delay));
    }


    private IEnumerator DoSomethingAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }
}
