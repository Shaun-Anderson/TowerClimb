using UnityEngine;
using System.Collections;
using System.Linq;
public class FPSCOUNTER : MonoBehaviour
{
    float deltaTime = 0.0f;
    public PlayerHandler pHand;
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();
            GUIStyle _style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.green;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps) \n Distance:{2} ", msec, fps, pHand.distance);
            GUI.Label(rect, text, style);

           /* Rect _rect = new Rect(0, 0, w, h * 2 / 100);
            _style.alignment = TextAnchor.UpperRight;
            _style.fontSize = h * 2 / 100;
            _style.normal.textColor = Color.yellow;
            string _text = string.Format("CUR_STATE:{0} \n CUR_FSMA:{1} \n CUR_DA:{2} \n SEARCH_STATE:{3} \n SEARCH_FSMA:{4} \n SEARCH_DA:{5}", UserDetails.CurrentState, UserDetails.CurrentFSAM, UserDetails.CurrentDA, UserDetails.SearchState, UserDetails.SearchFSAM, UserDetails.SearchDA);
            GUI.Label(_rect, _text, _style);
            */
    }
}