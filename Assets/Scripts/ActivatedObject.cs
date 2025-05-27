using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    public GameObject objecttoActivate;
    public void activated(float delay)
    {
        StartCoroutine(DoSomethingAfterDelay(delay));
    }


    private IEnumerator DoSomethingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (objecttoActivate != null)
        {
            objecttoActivate.SetActive(true);
        }
    }
}
