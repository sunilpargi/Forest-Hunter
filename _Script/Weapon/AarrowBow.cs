using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AarrowBow : MonoBehaviour
{
    private Rigidbody myBody;

    public float speed = 30f;

    public float deactivate_Timer = 3f;

    public float damage = 10;

   
    public AudioClip launchClip, hitClip;
    AudioSource audioSource;
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
     
    }

    // Use this for initialization
    void Start()
    {
        Invoke("DeactivateGameObject", deactivate_Timer);
    }
       
    public void Launch(Camera mainCamera)
    {
        audioSource.clip = launchClip;
        audioSource.Play();
       // AudioSource.PlayClipAtPoint(launchClip, Camera.main.transform.position);
        myBody.velocity = mainCamera.transform.forward * speed;

        transform.LookAt(transform.position + myBody.velocity);

    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider target)
    {

        // after we touch an enemy deactivate game object
        if (target.tag == Tags.ENEMY_TAG)
        {

             AudioSource.PlayClipAtPoint(hitClip, Camera.main.transform.position);
            target.GetComponent<Health>().ApplyDamage(damage);
            gameObject.SetActive(false);

        }

    }
}
