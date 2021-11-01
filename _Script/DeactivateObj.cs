using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine( Deactivate());
    }

   IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
