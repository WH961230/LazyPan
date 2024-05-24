using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class LazyPanEntity : EditorWindow {
    public void OnCustomGUI(float areaX) {
        GUILayout.BeginArea(new Rect(areaX, 30, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();
        GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
        GUILayout.Label("ENTITY", style, GUILayout.Height(80));
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
        if(GUILayout.Button("打开实体配置表 Csv", style, GUILayout.Height(80))) {
            OpenEntityCsv();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.EndArea();
    }

    private void OpenEntityCsv() {
        string filePath = Application.dataPath + "/StreamingAssets/Csv/ObjConfig.csv";
        Process.Start(filePath);
    }
}