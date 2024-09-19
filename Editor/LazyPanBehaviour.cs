using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class LazyPanBehaviour : EditorWindow {
        public void OnCustomGUI(float areaX) {
            GUILayout.BeginArea(new Rect(areaX, 30, Screen.width, Screen.height));

            GUILayout.BeginHorizontal();
            GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
            GUILayout.Label("BEHAVIOUR", style, GUILayout.Height(80));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if(GUILayout.Button("打开行为配置表 Csv", style, GUILayout.Height(80))) {
                OpenBehaviourCsv(false);
            }

            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if(GUILayout.Button("打开行为自动脚本生成配置表 Csv", style, GUILayout.Height(80))) {
                OpenBehaviourCsv(true);
            }
            GUILayout.EndHorizontal();
        
            GUILayout.EndArea();
        }

        private void OpenBehaviourCsv(bool isGenerate) {
            string fileName = isGenerate ? "BehaviourGenerate" : "BehaviourConfig";
            string filePath = Application.dataPath + $"/StreamingAssets/Csv/{fileName}.csv";
            Process.Start(filePath);
        }

        public void OnStart() {
            UnityEngine.Debug.LogError("Behaviour Start");
        }
    }
}