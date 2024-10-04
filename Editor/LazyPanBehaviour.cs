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
            
            isFoldoutTool = true;
            isFoldoutData = true;
        }

        public void OnCustomGUI(float areaX) {
            GUILayout.BeginArea(new Rect(areaX, 60, Screen.width, Screen.height));
            Title();
            AutoTool();
            PreviewBehaviourConfigData();
            GUILayout.EndArea();
        }

        private void PreviewBehaviourConfigData() {
            isFoldoutData = EditorGUILayout.Foldout(isFoldoutData, " 预览实体配置数据", true);
            Rect rect = GUILayoutUtility.GetLastRect();
            float height = 0;
            if (isFoldoutData) {
                GUILayout.Label("");
                height += GUILayoutUtility.GetLastRect().height;
                ExpandBehaviourData();
                height += GUILayoutUtility.GetLastRect().height;
            } else {
                GUILayout.Space(10);
            }
            
            LazyPanTool.DrawBorder(new Rect(rect.x + 2f, rect.y - 2f, rect.width - 2f, rect.height + height + 5f), Color.white);

            GUILayout.Space(10);
        }

        private void AutoTool() {
            isFoldoutTool = EditorGUILayout.Foldout(isFoldoutTool, " 自动化工具", true);
            Rect rect = GUILayoutUtility.GetLastRect();
            float height = 0;
            if (isFoldoutTool) {
                GUILayout.Label("");
                height += GUILayoutUtility.GetLastRect().height;
                GUILayout.BeginHorizontal();
                GUIStyle style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if(GUILayout.Button("打开行为配置表 Csv", style)) {
                    OpenBehaviourCsv(false);
                }
                if(GUILayout.Button("打开行为自动脚本生成配置表 Csv", style)) {
                    OpenBehaviourCsv(true);
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;
            } else {
                GUILayout.Space(10);
            }
            
            LazyPanTool.DrawBorder(new Rect(rect.x + 2f, rect.y - 2f, rect.width - 2f, rect.height + height + 5f), Color.white);

            GUILayout.Space(10);
        }

        private void Title() {
            GUILayout.BeginHorizontal();
            GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
            GUILayout.Label("BEHAVIOUR", style);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
            GUILayout.Label("@行为 游戏内实体绑定的业务逻辑", style);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
        }

        private void ExpandBehaviourData() {
            if (BehaviourConfigStr != null && BehaviourConfigStr.Length > 0) {
                GUILayout.BeginVertical();
                foreach (var str in BehaviourConfigStr) {
                    if (str != null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        for (int i = 0 ; i < str.Length ; i ++) {
                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                            labelStyle.normal.textColor = Color.green; // 设置字体颜色
                            if (GUILayout.Button(str[i], labelStyle, GUILayout.Width(Screen.width / str.Length - 10))) {
                            }
                        }
                        GUILayout.FlexibleSpace();
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