using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shooter : MonoBehaviour
{
    [Header("Dont Touch These")]
    public Vector3 startPosition;
    private float timer;
    [Header("Automatically Set In Editor")]
    public movementAndDeath mv;
    public PauseMenu pm;
    public GameObject player;
    [Header("Set In Editor")]
    public Rigidbody2D rb;
    public Transform shooterPos;
    // All the "square" objects are circles but im too lazy to change that lol
    public GameObject square;
    public GameObject square2;
    public GameObject square3;
    public GameObject enemy;
    public GameObject bulletSpawnPos;
    public GameObject bulletPrefab;
    public GameObject ArrowSprite;
    public GameObject back;
    public GameObject front;
    // Distance from which the enemy is starting to shoot at you and is moving towards you (moving towards the front game object)
    public float ndistance;
    // Distance from which the enemy is starting to shoot at you and is standing still
    public float n2distance;
    // Distance from which the enemy is starting to shoot at you and is backing away from you (moving towards the back game object)
    public float n3distance;
    [Header("You Can Edit These")]
    // enemy speed
    public float speed;
    // enemy speed idk i forgo
    public float speederr;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Plauer");
        mv = GameObject.FindGameObjectWithTag("Plauer").GetComponent<movementAndDeath>();
        pm = GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(shooterPos.position, player.transform.position);
        if (distance > ndistance)
        {
            timer = 1.9f;
            ArrowSprite.SetActive(true);
        }
        if (distance < ndistance)
        {
            if (timer > 1.5f)
            {
                ArrowSprite.SetActive(true);
            }
            timer += Time.deltaTime;
            if (timer > 2.5f)
            {
                timer = 0;
                shoot();
                ArrowSprite.SetActive(false);
            }
        }
        
        Vector2 direction = player.transform.position - transform.position;
        Vector2 backdirection = startPosition - transform.position;

        // this is the angle which is always facing the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // this is the opposite angle from the player , not used
        float runangle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        // this is the angle that is always facing the start position of the enemy
        float backangle = Mathf.Atan2(backdirection.y, backdirection.x) * Mathf.Rad2Deg;
        // if it sees the player , but hes too far , it starts moving towards him
        if (mv.die != true && distance < ndistance && distance > n2distance && pm.GameIsPaused == false)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        // if the player is close enought , it stops moving towards it
        else if (mv.die != true && distance < n2distance && distance > n3distance && pm.GameIsPaused == false)
        {
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        // if the player is too close , it starts backing away
        else if (mv.die != true && distance < n3distance && pm.GameIsPaused == false)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, back.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        // if the player is not nearby , it goes back to the start position
        else if (mv.die != true && pm.GameIsPaused == false)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, startPosition, speed * Time.deltaTime * 0.5f);
            transform.rotation = Quaternion.Euler(Vector3.forward * backangle);
        }
        // honestly , idk how to make the circles scale correctly , so i just use a random number until it works, maybe u can figure out
        square.transform.localScale = new Vector2(ndistance * 2.333f, ndistance * 2.333f);
        square2.transform.localScale = new Vector2(n2distance * 2.333f, n2distance * 2.333f);
        square3.transform.localScale = new Vector2(n3distance * 2.33f, n3distance * 2.33f);



        if (pm.GameIsPaused == true)
        {
            rb.velocity = Vector2.zero;
        }
    }



    public void shoot()
    {
        Instantiate(bulletPrefab, bulletSpawnPos.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Plauer") && pm.GameIsPaused == false)
        {
            // you can convert this function to give coins in the 32014 2 shop 
            // PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + 1);
            enemy.SetActive(false);
        }
    }
}
