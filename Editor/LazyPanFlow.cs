using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LazyPan {
    public class LazyPanFlow : EditorWindow {
        private bool isFoldout;
        private string[][] FlowGenerateStr;
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

            isFoldout = EditorGUILayout.Foldout(isFoldout, "预览流程自动化数据", true);
            if (isFoldout) {
                GUILayout.BeginVertical();
                foreach (var str in FlowGenerateStr) {
                    if (str != null) {
                        GUILayout.BeginHorizontal();
                        for (int i = 0 ; i < str.Length ; i ++) {
                            Color fontColor;
                            if (i == 0) {
                                fontColor = Color.cyan;
                            } else {
                                fontColor = str[4] == "Init" ? Color.green : Color.red;
                            }
                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                            labelStyle.normal.textColor = fontColor; // 设置字体颜色
                            GUILayout.Label(str[i], labelStyle, GUILayout.Width(150));
                        }
                        GUILayout.EndHorizontal();                
                    }
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndArea();
        }

        private void OpenFlowCsv() {
            string filePath = Application.dataPath + "/StreamingAssets/Csv/FlowGenerate.csv";
            Process.Start(filePath);
        }

        public void OnStart() {
            string title = "Flow Start \n";
            string log = "";
            ReadCSV.Instance.Read("FlowGenerate", out string content, out string[] lines);
            FlowGenerateStr = new string[lines.Length - 2][];
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    //遍历第三行到最后一行
                    //遍历每一行数据
                    log += lines[i] + "\n";
                    string[] lineStr = lines[i].Split(",");
                    FlowGenerateStr[i - 3] = new string[lineStr.Length];
                    if (lineStr.Length > 0) {
                        for (int j = 0; j < lineStr.Length; j++) {
                            try {
                                FlowGenerateStr[i - 3][j] = lineStr[j];
                            } catch (Exception e) {
                                Debug.LogErrorFormat("{0} {1}", i - 3, j);
                                throw;
                            }
                        }
                    }
                }
            }
            
            UnityEngine.Debug.LogError(log);
        }
    }
}