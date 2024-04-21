using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class explode : MonoBehaviour
{
    public movementAndDeath mv;
    public health healt;
    public bool isPlayer = true;
    public GameObject player;
    public float audioo;
    public GameObject explosion;
    public AudioSource source;
    public AudioSource source2;

    private void Start()
    {
        audioo = PlayerPrefs.GetFloat("volume");
        healt = GameObject.FindGameObjectWithTag("Plauer").GetComponent<health>();
        mv = GameObject.FindGameObjectWithTag("Plauer").GetComponent<movementAndDeath>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ActiveMine"))
        {
                Explode();
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    private void Explode()
    {
        // Play explosion sound
        source2.Play();
        Invoke("e", 0.1f);
        // Make an explosion
        

        // Optional, add visual effect 

        if (!isPlayer)
        {// Destroy the enemy
            e();
            Destroy(player);
        }

        else if (isPlayer)
        {
            // Delay before loading the scene
            Invoke("LoadSceneAfterDelay", 2.2f);
            healt.currenthealth--;
            healt.currenthealth--;
            healt.currenthealth--;
            mv.NDBAM = true;
            mv.sprite.SetActive(false);
        }
    }

    private void LoadSceneAfterDelay()
    {
        mv.die = true;
        mv.playtimer = false;
        mv.f = 2;
        mv.e = 2;
        mv.rb.velocity = Vector2.zero;
    }
    private void e()
    {
        source.Play();
    }

}
