using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [Header("Automatically Set In Editor")]
    private GameObject player;
    private Rigidbody2D rb;
    private PauseMenu pm;
    [Header("Set In Editor")]
    public GameObject me;
    [Header("You Can Edit These")]
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Plauer");

        Vector3 direction = player.transform.position - rb.transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
        pm = GameObject.FindGameObjectWithTag("pause").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == ("Plauer") && pm.GameIsPaused == false)
        {
            collision.gameObject.GetComponent<health>().decreaseHp(1);
        }
        me.SetActive(false);
        if (collision.gameObject.tag == ("Enemy"))
        {
            collision.gameObject.GetComponent<AI>().TakeDamage(1);
            print("bozoHitHisFriend");
        }
        else if (collision.gameObject.tag == ("BuffEnemy"))
        {
            collision.gameObject.GetComponent<BuffAI>().TakeDamage(1);
            print("bozoHitHisFriend");
        }
        else if (collision.gameObject.tag == ("ShooterEnemy"))
        {
            collision.gameObject.SetActive(false);
            print("bozoHitHisFriend");
        }
    }
}
