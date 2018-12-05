using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDmg : MonoBehaviour {

    public GameObject hit;
    public float Knockback;
    public int health;

    public bool hittable = true;

    public void HIT (GameObject player)
    {
        if(hittable)
        {
            StartCoroutine(Hit(player));
            hittable = false;
        }
    }
	
	// Update is called once per frame
	IEnumerator Hit (GameObject player)
    {
        player.GetComponent<AudioSource>().PlayOneShot(player.GetComponent<AudioSource>().clip);
        player.GetComponent<PlayerHandler>().Movement.velocity = new Vector2((player.transform.position.x - transform.position.x) * Knockback * Time.deltaTime, (player.transform.position.y - transform.position.y) * Knockback* Time.deltaTime);
        player.GetComponent<PlayerHandler>().jumps = 3;
        player.GetComponent<PlayerHandler>().ShakeCam(0.05f);
        health -= 1;
        if(health <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            
            player.GetComponent<PlayerHandler>().Dashing = false;
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            player.GetComponent<PlayerHandler>().Dashing = false;
        }
        hittable = true;
    }
}

