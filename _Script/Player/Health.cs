using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] private EnemyAnimation enemy_Anim;
  [SerializeField]  private NavMeshAgent navAgent;
    [SerializeField] private EnemyController enemy_Controller;

    public float health = 100f;

    public bool is_Player, is_Boar, is_Cannibal;

    [SerializeField] private bool is_Dead;

    private EnemyAudio enemyAudio;

    private PlayerStates player_Stats;
    public GameObject deathFX;

    [SerializeField] Image healthImage;
    public bool shooHealthImage;
    void Awake()
    {
        
        is_Dead = false;
        if (is_Boar || is_Cannibal)
        {
            //if (shooHealthImage)
            //{
            //    if (is_Boar)
            //    {
            //        Debug.Log(1);
            //        healthImage = GameObject.FindGameObjectWithTag("Boar1").GetComponent<Image>();
            //        healthImage.fillAmount = health / 100;
            //    }

            //    if (is_Cannibal)
            //    {
            //        Debug.Log(2);
            //        healthImage = GameObject.FindGameObjectWithTag("Cannibal1").GetComponent<Image>();
            //        healthImage.fillAmount = health / 100;
            //    }
            //}
            enemy_Anim = GetComponent<EnemyAnimation>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            // get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if (is_Player)
        {
            player_Stats = GetComponent<PlayerStates>();
        }

    }

    public void ApplyDamage(float damage)
    {

        // if we died don't execute the rest of the code
        if (is_Dead)
            return;


       
        health -= damage;

        if (!is_Player)
        {
            enemyAudio.Play_injuredSound();
            healthImage.fillAmount = health / 100;
        }
            
        

        if (is_Player)
        {
            // show the stats(display the health UI value)
           player_Stats.Display_HealthStats(health);
        }

        if (is_Boar || is_Cannibal)
        {
          
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
             
                enemy_Controller.chase_Distance = 100f;
            }
        }

        if (health <= 0f)
        {
          
            PlayerDied();

            is_Dead = true;
        }

    } // apply damage

    void PlayerDied()
    {

        if (is_Cannibal)
        {
           
            GetComponent<Animator>().enabled = false;
           // GetComponent<BoxCollider>().isTrigger = false;
         //  GetComponent<Rigidbody>().AddTorque(-transform.forward * 10f);
          
           
            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;
            Instantiate(deathFX, transform.position, Quaternion.identity);
            StartCoroutine(DeadSound());
            gameObject.SetActive(false);
            // EnemyManager spawn more enemies
       //    EnemyManager.instance.EnemyDied(true);
        }

        if (is_Boar)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());

            // EnemyManager spawn more enemies
       //   EnemyManager.instance.EnemyDied(true);
        }

        if (is_Player)
        {

            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            // call enemy manager to stop spawning enemis
        //  EnemyManager.instance.StopSpawning();

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);

        }

        //if (tag == Tags.PLAYER_TAG)
        //{

        //    Invoke("RestartGame", 3f);

        //}
       

    } // player died

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.Play_DeadSound();
    }

}
