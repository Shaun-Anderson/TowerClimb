using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player" && !col.GetComponent<PlayerHandler>().dmgImmune)
        {
             Hit(col.gameObject);
        }
    }

    // Update is called once per frame
    void Hit(GameObject player)
    {
        player.GetComponent<PlayerHandler>().Movement.velocity = new Vector2((player.transform.position.x - transform.position.x) * 200f * Time.deltaTime, (player.transform.position.y - transform.position.y) * 200f * Time.deltaTime);
        player.GetComponent<PlayerHandler>().jumps = 3;
        player.GetComponent<PlayerHandler>().Hit(1, 0.02f);
        player.GetComponent<PlayerHandler>().Dashing = false;
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
    }
}
