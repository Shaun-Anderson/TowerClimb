using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public Transform player;
    public bool active;
    public AudioClip pickupsound;
    public float moveTowardsSpeed;
    public bool pickedUp;
    public bool spawned;

    private SpriteRenderer sRend;
	// Use this for initialization
	void Start () {
        sRend = GetComponent<SpriteRenderer>();
    }
	
    public void Spawn()
    {
        StartCoroutine(SPAWN());
    }
	// Update is called once per frame
	void Update () {
        if (active && spawned)
        {
            GetComponent<Physics_Object>().gravity = 0;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveTowardsSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && pickedUp == false && spawned)
        {
            pickedUp = true;
            GetComponent<AudioSource>().PlayOneShot(pickupsound);
            col.GetComponent<PlayerHandler>().gold += 1;
            col.GetComponent<PlayerHandler>().ChangeCoins();
            Destroy(gameObject, 0.3f);
        }
    }

    public IEnumerator SPAWN()
    {     
        yield return new WaitForSeconds(1);
        spawned = true;
    }

    }
