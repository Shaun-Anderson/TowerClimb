using UnityEngine;
using System.Collections;

public class Radius : MonoBehaviour {

    public float radi;

    public PlayerHandler pHand;
    public CircleCollider2D pCol;


    void Start()
    {
        //pStats = GetComponentInParent<PlayerStats>();
        pCol = GetComponentInChildren<CircleCollider2D>();
        pHand = GetComponentInParent<PlayerHandler>();
       // radi = pStats.radius;
        pCol.radius = radi;
    }

    void ChangeRadi()
    {
        //radi = pStats.radius;
        pCol.radius = radi;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            if (col.GetComponent<Pickup>() != null)
            {
                if (col.GetComponent<Pickup>().active == false)
                {
                    col.GetComponent<Pickup>().active = true;
                    col.GetComponent<Pickup>().player = transform;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col != null)
        {
            if (col.GetComponent<Pickup>() != null)
            {
                if (col.GetComponent<Pickup>().active == true & col.tag == "Pickup")
                {
                    col.GetComponent<Pickup>().active = false;
                    col.GetComponent<Pickup>().player = null;
                }
            }
        }
    }

}
