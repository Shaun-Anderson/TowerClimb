using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

    public int health = 10;
    public SpriteRenderer sRend;
    public Material normalMat;
    public Material hitMat;
    public Sprite openSprite;
    public GameObject coin;
    public bool opened;
    // Use this for initialization
    void Start () {
        sRend = GetComponent<SpriteRenderer>();
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !opened)
        {
            Hit(1);
        }
    }
	
	// Update is called once per frame
	public void Hit (int damage)
    {
        health -= damage;
        CheckHealth();
        StartCoroutine(HIT());
    }

    public void CheckHealth()
    {
        if(health <= 0)
        {
            SpawnPickups();
        }
    }

    public void SpawnPickups()
    {
        for (int i = 0; i < 5; i++)
        {
            sRend.sprite = openSprite;
            GameObject newCoin = Instantiate(coin, transform.position, transform.rotation);
            newCoin.GetComponent<Pickup>().Spawn();
            newCoin.GetComponent<Physics_Object>().velocity.x = Random.Range(-5.0f, 5.0f);
            newCoin.GetComponent<Physics_Object>().velocity.y = Random.Range(4, 10);

            newCoin.GetComponent<Physics_Object>().directionalInput.x = Random.Range(-1, 1);

            opened = true;
        }
    }

    public IEnumerator HIT()
    {
        sRend.material = hitMat;
        yield return new WaitForSeconds(0.1f);
        sRend.material = normalMat;
    }
}
