using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movementAndDeath : MonoBehaviour
{
    [Header("Dont Touch These")]
    public float movementH;
    public float movementV; 
    public float timer;
    // e is the x size for the bouncing effect , f is the y size for the bouncing effect
    public float e;
    public float f;
    // original size / multiplication factor ( i couldnt come up with a better name )
    public float m;
    //if its false the player is facing right if its true the player is facing left
    public bool playerDirection;
    // false = wawa alive , true = wawa dead
    public bool die;
    // I hosenstly forgot what this is for , it may be a useless variable , or its used by another script , so dont touch
    public Vector2 Distance;
    // used to activate the timer
    public bool playtimer;
    // NotDeadByAMine , makes you not able to move if ur dead (i made it so if you die by a mine , theres a 2 sec animation before dying)
    public bool NDBAM = false;
    [Header("Set In Editor")]
    public GameObject sprite;
    public PauseMenu pm;
    public Rigidbody2D rb;
    public Joystick wawastick;
    [Header("You Can Edit These")]
    public float speed;

    // Start is called before the first frame update

    void Start()
    {
        // IMPORTANT
        // SET IT TO 1 ON MOBILE AND TO 0 ON PC
        // idk how to automate this and im currently sorting code to make it readable , so im not gonna search up tutorials to make it auto set lol
        PlayerPrefs.SetInt("mobile", 0);
        playtimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementH < 1f)
        {
            movementH = 0f;
            this.rb.velocity = new Vector2(0, this.rb.velocity.y);
        }
        if (movementV < 1f)
        {
            movementV = 0f;
            this.rb.velocity = new Vector2(this.rb.velocity.x,0);
        }
        if (die == false && pm.GameIsPaused == false && PlayerPrefs.GetInt("mobile", 1) == 0 && NDBAM == false)
        {
            movementH = Input.GetAxisRaw("Horizontal");
            movementV = Input.GetAxisRaw("Vertical");
        }
        else if (die == false && pm.GameIsPaused == false && PlayerPrefs.GetInt("mobile", 1) == 1 && NDBAM == false)
        {
            movementH = wawastick.Horizontal;
            movementV = wawastick.Vertical;
        }
        else
        {
            movementH = 0;
            movementV = 0;
            rb.velocity = new Vector2(0,0);
        }

        if (Input.GetKeyDown(KeyCode.D)&& pm.GameIsPaused == false)
        {
            playerDirection = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) == true && pm.GameIsPaused == false)
        {
            playerDirection = false;
        }

        if (PlayerPrefs.GetInt("mobile", 0) == 0)
        {
            if (Input.GetKeyDown(KeyCode.A) && pm.GameIsPaused == false)
            {
                playerDirection = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) == true && pm.GameIsPaused == false)
            {
                playerDirection = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && pm.GameIsPaused == false)
            {
                playerDirection = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) == true && pm.GameIsPaused == false)
            {
                playerDirection = false;
            }
        }
        if (PlayerPrefs.GetInt("mobile", 0) == 1)
        {
            if (wawastick.Horizontal < 0 && pm.GameIsPaused == false)
            {
                playerDirection = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && pm.GameIsPaused == false)
            {
                playerDirection = false;
            }
            if (wawastick.Horizontal > 0 && pm.GameIsPaused == false)
            {
                playerDirection = false;
            }
        }

        if (playerDirection == false && pm.GameIsPaused == false)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        if (playtimer == true && pm.GameIsPaused == false)
        {
          timer = timer + Time.deltaTime;
        }
        if (timer > 0.2f)
        {
            timer = 0;
        }

        if (timer <= 0.1f)
        {
            e = m * 0.925f;
            f = m * 1.075f;
        }
        if (timer > 0.1f)
        {
            f = m * 0.925f;
            e = m * 1.075f;
        }

        rb.velocity = new Vector2(movementH * speed,movementV * speed);

        if (movementH != 0 || movementV != 0 && pm.GameIsPaused == false)
        {
            rb.transform.localScale = new Vector2(f, e);
        }
        else if (pm.GameIsPaused == false)
        {
            rb.transform.localScale = new Vector2(m, m);
        }

    }

}
