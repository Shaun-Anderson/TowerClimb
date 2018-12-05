/*using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager myTarget = (GameManager)target;

        myTarget.pause = EditorGUILayout.Toggle("Paused", myTarget.pause);
    }
}
*/