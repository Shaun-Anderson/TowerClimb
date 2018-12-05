using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour {
    public int targetWidth = 640;
    public int pixelsToUnits = 100;
    public float zoom = 4; //determines amount of zoom capable. Larger number means further zoomed in
    public float smooth = 5; //smooth determines speed of transition between zoomed in and default state
                             // Use this for initialization

    public float up;
    public float down;
    public Camera cam;
    void Start () {
        cam = GetComponent<Camera>();
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
       // cam.orthographicSize = height / pixelsToUnits / 2;
    }
	
	// Update is called once per frame
	void Update () {

       cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, smooth * Time.deltaTime);
    }
}
