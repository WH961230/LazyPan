using UnityEditor;
using UnityEngine;

public class LazyPanGuide : EditorWindow {
    public void OnCustomGUI(float areaX) {
        GUILayout.BeginArea(new Rect(areaX, 30, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();
        GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
        GUILayout.Label("LAZYPAN", style, GUILayout.Height(80));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
        GUILayout.Label("第一步: 点击按钮自动安装环境", style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
        if (GUILayout.Button("点击此处 自动安装环境（自动拷贝 CSV 游戏配置 AB包 等）", style)) {
            Debug.Log("测试点击");
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}