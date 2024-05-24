using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class LazyPanFlow : EditorWindow {
    public void OnCustomGUI(float areaX) {
        GUILayout.BeginArea(new Rect(areaX, 30, Screen.width, Screen.height));


        GUILayout.BeginHorizontal();
        GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
        GUILayout.Label("FLOW", style, GUILayout.Height(80));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
        if(GUILayout.Button("打开流程配置表 Csv", style, GUILayout.Height(80))) {
            OpenFlowCsv();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void OpenFlowCsv() {
        string filePath = Application.dataPath + "/StreamingAssets/Csv/FlowGenerate.csv";
        Process.Start(filePath);
    }
}