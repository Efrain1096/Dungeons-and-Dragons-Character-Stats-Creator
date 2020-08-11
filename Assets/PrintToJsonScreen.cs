using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintToJsonScreen : MonoBehaviour
{
    public string text = "the quick brown fox jumps over the lazy dog";

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 12;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        GUI.Label(rect, text, style);
    }
}
