using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {
    public bool toMain;
    public Camera cam;
    public Transform player;
    public Transform PlayerPos;
    public Transform CamPos;
    public Image fadeUI;

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D col)
    {
		if(col.tag == "Player")
        {
            player = col.transform;
            PlayerEnter();
        }
	}
	
	// Update is called once per frame
	void PlayerEnter ()
    {
        cam = Camera.main;
        fadeUI = GameManager.instance.UI.transform.Find("Fade").GetComponent<Image>(); ;
        player.GetComponent<PlayerHandler>().slingshotState = SlingshotState.Idle;
        if(!toMain)
        {
            StartCoroutine(Fade());
            cam.GetComponent<SmoothCamera2D>().enabled = false;
        }
        else
        {
            StartCoroutine(Fade());
        }
	}

    IEnumerator Fade()
    {
        _GameManager.NoMove = true;
        fadeUI.gameObject.SetActive(true);
        Time.timeScale = 1;
        float totalChange = 1f - fadeUI.color.a;
        float changePerSecond = totalChange / 0.2f;
        float totalTime = 0;
        while (totalTime < 0.2f)
        {
            totalTime += Time.deltaTime;
            float increment = Time.deltaTime * changePerSecond;
            fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, fadeUI.color.a + increment);

            yield return new WaitForEndOfFrame();
        }

        fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, 1f);

        player.transform.position = PlayerPos.position;
        if (toMain)
        {
            cam.GetComponent<SmoothCamera2D>().enabled = true;
            cam.GetComponent<SmoothCamera2D>().GoToPlayer();
        }
        else
        {
            cam.transform.position = new Vector3(CamPos.position.x, CamPos.position.y, -10);
        }
        yield return new WaitForSeconds(0.2f);


        totalChange = 0f - fadeUI.color.a;
        changePerSecond = totalChange / 0.2f;
        totalTime = 0;
        while (totalTime < 0.2f)
        {
            totalTime += Time.deltaTime;
            float increment = Time.deltaTime * changePerSecond;
            fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, fadeUI.color.a + increment);

            yield return new WaitForEndOfFrame();
        }

        fadeUI.color = new Color(fadeUI.color.r, fadeUI.color.g, fadeUI.color.b, 0f);
        fadeUI.gameObject.SetActive(false);
        _GameManager.NoMove = false;
        yield break;
    }
}
