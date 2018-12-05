using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikemon : MonoBehaviour {

    public bool movingLeft;
    public LayerMask collisionMask;
    public int health;

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_GameManager.NoMove == false)
        {
            if (movingLeft)
            {
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, 0.1f, collisionMask);
                Debug.DrawRay(transform.position, Vector2.right * 0.1f, Color.red);
                if (hit)
                {
                    speed = -speed;
                    Flip();
                    movingLeft = false;
                }
            }
            else
            {
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.1f, collisionMask);
                Debug.DrawRay(transform.position, Vector2.right * 0.1f, Color.red);
                if (hit)
                {
                    speed = -speed;
                    Flip();
                    movingLeft = true;
                }
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<PlayerHandler>().Dashing)
            {
                Hit(col.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Hit(GameObject player)
    {
        GameManager.instance.kills += 1;
        GameManager.instance.CheckMissions();
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        player.GetComponent<PlayerHandler>().jumps = 3;
        player.GetComponent<PlayerHandler>().ShakeCam(0.01f);
        player.GetComponent<PlayerHandler>().Dashing = false;
    }

    protected void Flip()
    {
        Vector2 bodyScale = transform.localScale;
        bodyScale.x *= -1;
        transform.localScale = bodyScale;
    }
}
