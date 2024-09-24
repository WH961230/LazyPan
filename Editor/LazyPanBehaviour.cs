using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class LazyPanBehaviour : EditorWindow {
        private bool isFoldoutTool;
        private bool isFoldoutData;
        private string[][] BehaviourConfigStr;
        private LazyPanTool _tool;

        public void OnStart(LazyPanTool tool) {
            _tool = tool;

            ReadCSV.Instance.Read("BehaviourConfig", out string content, out string[] lines);
            if (lines != null && lines.Length > 0) {
                BehaviourConfigStr = new string[lines.Length - 3][];
                for (int i = 0; i < lines.Length; i++) {
                    if (i > 2) {
                        string[] lineStr = lines[i].Split(",");
                        BehaviourConfigStr[i - 3] = new string[lineStr.Length];
                        if (lineStr.Length > 0) {
                            for (int j = 0; j < lineStr.Length; j++) {
                                BehaviourConfigStr[i - 3][j] = lineStr[j];
                            }
                        }
                    }
                }
            }
        }

        public void OnCustomGUI(float areaX) {
            GUILayout.BeginArea(new Rect(areaX, 30, Screen.width, Screen.height));

            GUILayout.BeginHorizontal();
            GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
            GUILayout.Label("BEHAVIOUR", style, GUILayout.Height(80));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
            GUILayout.Label("@行为 游戏内实体绑定的业务逻辑", style);
            GUILayout.EndHorizontal();

            isFoldoutTool = EditorGUILayout.Foldout(isFoldoutTool, " 自动化工具", true);
            if (isFoldoutTool) {
                GUILayout.Label("");
                GUILayout.BeginHorizontal();

                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if(GUILayout.Button("打开行为配置表 Csv", style, GUILayout.Height(80))) {
                    OpenBehaviourCsv(false);
                }
                if(GUILayout.Button("打开行为自动脚本生成配置表 Csv", style, GUILayout.Height(80))) {
                    OpenBehaviourCsv(true);
                }
                GUILayout.EndHorizontal();
            }

            // 增加间距
            GUILayout.Space(10); // 增加10像素的上下间距
            
            isFoldoutData = EditorGUILayout.Foldout(isFoldoutData, " 预览实体配置数据", true);
            if (isFoldoutData) {
                ExpandBehaviourData();
            }

            GUILayout.EndArea();
        }

        private void ExpandBehaviourData() {
            if (BehaviourConfigStr != null && BehaviourConfigStr.Length > 0) {
                GUILayout.BeginVertical();
                string behaviourSign = "";
                foreach (var str in BehaviourConfigStr) {
                    if (str != null) {
                        if (behaviourSign != str[1]) {
                            behaviourSign = str[1];
                            GUILayout.Label("");
                        }

                        GUILayout.BeginHorizontal();
                        for (int i = 0 ; i < str.Length ; i ++) {
                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                            labelStyle.normal.textColor = Color.green; // 设置字体颜色
                            if (GUILayout.Button(str[i], labelStyle, GUILayout.Width(200))) {
                            }
                        }
                        GUILayout.EndHorizontal();                
                    }
                }
                GUILayout.EndVertical();
            }
        }

        private void OpenBehaviourCsv(bool isGenerate) {
            string fileName = isGenerate ? "BehaviourGenerate" : "BehaviourConfig";
            string filePath = Application.dataPath + $"/StreamingAssets/Csv/{fileName}.csv";
            Process.Start(filePath);
        }
    }
}