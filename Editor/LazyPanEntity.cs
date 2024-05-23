using UnityEditor;
using UnityEngine;

public class LazyPanEntity : EditorWindow {
    public void OnCustomGUI(float areaX) {
        GUILayout.BeginArea(new Rect(areaX, 30, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();
        GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
        GUILayout.Label("ENTITY", style, GUILayout.Height(80));
        GUILayout.EndHorizontal();
        
        GUILayout.EndArea();
    }
}