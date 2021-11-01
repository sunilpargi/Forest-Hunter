using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 20;
    public float radius = 5f;
    public LayerMask layerMask;

    void Update()
    {

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
          for(int i = 0; i <hits.Length; i++)
            {

                hits[i].gameObject.GetComponent<Health>().ApplyDamage(damage);

            }
          
            gameObject.SetActive(false);

        }

    }
}
