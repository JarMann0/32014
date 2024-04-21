using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AI : MonoBehaviour
{
    private float distance;
    [Header("Dont Touch These")]
    public Vector3 startPosition;
    public float timer;
    public float timer2;
    public int distancee;
    public bool startTimer;
    public bool startTimer2;
    [Header("Automatically Set In Editor")]
    public GameObject player;
    public PauseMenu pm;
    public movementAndDeath mv;
    public health healt;
    [Header("Set In Editor")]
    public LayerMask layermask;
    // h1 - health visualisation 1 , h2 - health visualisation 2
    public GameObject H1;
    public GameObject H2;
    public GameObject enemy;
    public GameObject rangeOfSightVisualisationCircle;
    public Rigidbody2D rb;
    [Header("You Can Edit These")]
    public int healthh;
    public float speed;
    public float stunDuration;
    public float stunKnockback;
    // distance from which enemy notices you
    public float noticeDistance;

    void Start()
    {
        startPosition = transform.position;
        healt = GameObject.FindGameObjectWithTag("Plauer").GetComponent<health>();
        player = GameObject.FindGameObjectWithTag("Plauer");
        mv = GameObject.FindGameObjectWithTag("Plauer").GetComponent<movementAndDeath>();
        startTimer = false;
        pm = GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>();
    }

    void Update()
    {
        if (startTimer == true && pm.GameIsPaused == false)
        {
            timer += Time.deltaTime;
        }


        if (timer > stunDuration)
        {
            rb.velocity = Vector2.zero;
            startTimer = false;
            timer = 0;
        }
        
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        Vector2 backdirection = startPosition - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float backangle = Mathf.Atan2(backdirection.y, backdirection.x) * Mathf.Rad2Deg;
        //for some reason the raycast wouldnt work at all , even thought it worked fine in the original project , if you manage to fix it use this code instead 
        //if (mv.die != true && distance < noticeDistance && startTimer == false && pm.GameIsPaused == false && ShootRaycast(layermask,distancee) == true)
        if (mv.die != true && distance < noticeDistance && startTimer == false && pm.GameIsPaused == false)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        else if (mv.die != true && startTimer == false && pm.GameIsPaused == false)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, startPosition, speed * Time.deltaTime * 0.5f);
            transform.rotation = Quaternion.Euler(Vector3.forward * backangle);
        }
        rangeOfSightVisualisationCircle.transform.localScale = new Vector2(noticeDistance * 2.2f,noticeDistance* 2.2f);

        if (healthh == 2)
        {
            H1.SetActive(true);
            H2.SetActive(true);
        }
        if (healthh == 1)
        {
            H1.SetActive(true);
            H2.SetActive(false);
        }
        if (healthh == 0)
        {
            enemy.SetActive(false);
        }

        if (startTimer2 == true)
        {
            timer2 += Time.deltaTime;
        }
        if (timer2 > 0.5f)
        {
            startTimer2 = false;
            timer2 = 0;
        }

        if (pm.GameIsPaused == true)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // on collision with player , deducts one hp from player , starts flying backwards , and starting timer turns off movement and player damaging parts of ai
        //checks the objects tag , if the tag isnt player then it doesnt kill  you bruh
        if (collision.gameObject.tag == ("Plauer") && pm.GameIsPaused == false)
        {
            if (healt.currenthealth > 0 && startTimer == false)
            {
                healt.currenthealth--;
                startTimer = true;
                rb.velocity = new Vector2(stunKnockback, stunKnockback);
            }
            
            if (healt.currenthealth == 0)
            {
                mv.die = true;
                mv.rb.velocity = Vector2.zero;
                mv.playtimer = false;
                mv.f = 2;
                mv.e = 2;
                rb.velocity = Vector2.zero;
            }    
        }
        
    }

    public void TakeDamage(int i)
    {
        if (startTimer2 == false && pm.GameIsPaused == false)
        {
            healthh = healthh - i;
            if (healthh == 0)
            {
                // coins from the wawa shop , you can convert the function to give coins in the 32014 2 shop if you have that
                // PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + 1);
                enemy.SetActive(false);
            }
            print("damageDealt");
            startTimer2 = true;
        }
    }
    // look to line 68
    bool ShootRaycast(LayerMask layermask, int distancer)
    {

        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(transform.position, (player.transform.position - enemy.transform.position).normalized, distancer, layermask);
        if (hitInfo.collider != null)
        {
            Debug.DrawRay(transform.position, hitInfo.transform.position - transform.position, Color.green, Time.deltaTime, false);
            print("raycast hit something");
            if (hitInfo.collider.gameObject.tag == "Plauer")
            {
                print("raycast hit player");
                return true;
            }
            else
            {
                print("raycast hit " + hitInfo.collider.gameObject.name);
                return false;
            }
        }
        Debug.DrawRay(transform.position, player.transform.position - transform.position * distancer, Color.red, Time.deltaTime, false);
        print("raycast hit nothing");
        return false;
    }
}
