using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public bool on;
    public GameObject[] interactObjs;
    public Sprite onInteractSprite;
    public Sprite onSprite;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player" && !on)
        {
            SwitchOn(col.gameObject);
        }
    }

    void SwitchOn(GameObject col)
    {
        col.GetComponent<PlayerHandler>().ShakeCam(0.01f);
        GetComponent<SpriteRenderer>().sprite = onSprite;
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        for(int i = 0; i < interactObjs.Length; i++)
        {
            interactObjs[i].GetComponent<SpriteRenderer>().sprite = onInteractSprite;
            interactObjs[i].GetComponent<BoxCollider2D>().enabled = false;
            interactObjs[i].GetComponent<AudioSource>().PlayOneShot(interactObjs[i].GetComponent<AudioSource>().clip);
        }
        on = true;
    }
}
