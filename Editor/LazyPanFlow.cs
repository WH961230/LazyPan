using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LazyPan {
    public class LazyPanFlow : EditorWindow {
        private bool isFoldoutTool;
        private bool isFoldoutData;
        private string[][] FlowGenerateStr;
        private LazyPanTool _tool;

        public void OnStart(LazyPanTool tool) {
            _tool = tool;
            ReadCSV.Instance.Read("FlowGenerate", out string content, out string[] lines);
            if (lines != null && lines.Length > 0) {
                FlowGenerateStr = new string[lines.Length - 2][];
                for (int i = 0; i < lines.Length; i++) {
                    if (i > 2) {
                        //遍历第三行到最后一行
                        //遍历每一行数据
                        string[] lineStr = lines[i].Split(",");
                        FlowGenerateStr[i - 3] = new string[lineStr.Length];
                        if (lineStr.Length > 0) {
                            for (int j = 0; j < lineStr.Length; j++) {
                                FlowGenerateStr[i - 3][j] = lineStr[j];
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
            GUILayout.Label("FLOW", style, GUILayout.Height(80));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
            GUILayout.Label("@流程 游戏内主流程", style);
            GUILayout.EndHorizontal();

            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            isFoldoutTool = EditorGUILayout.Foldout(isFoldoutTool, " 自动化工具", true);
            if (isFoldoutTool) {
                GUILayout.BeginHorizontal();
                if(GUILayout.Button("打开流程配置表 Csv", style, GUILayout.Height(80))) {
                    OpenFlowCsv();
                }
                GUILayout.EndHorizontal();
            }

            // 增加间距
            GUILayout.Space(10); // 增加10像素的上下间距
            
            isFoldoutData = EditorGUILayout.Foldout(isFoldoutData, " 预览流程自动化数据", true);
            if (isFoldoutData) {
                ExpandFlowData();
            }

            GUILayout.EndArea();
        }

        private void ExpandFlowData() {
            bool hasContent = false;
            if (FlowGenerateStr != null && FlowGenerateStr.Length > 0) {
                GUILayout.BeginVertical();
                string flowSign = "";
                foreach (var str in FlowGenerateStr) {
                    if (str != null) {
                        if (flowSign != str[0]) {
                            flowSign = str[0];
                            GUILayout.Label("");
                        }

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
                            if (GUILayout.Button(str[i], labelStyle, GUILayout.Width(150))) {
                                _tool.currentToolBar = 2;
                            }
                            hasContent = true;
                        }
                        GUILayout.EndHorizontal();                
                    }
                }
                GUILayout.EndVertical();
            }

            if(!hasContent || FlowGenerateStr == null || FlowGenerateStr.Length == 0) {
                GUILayout.Label("找不到 FlowGenerate.csv 的配置数据！\n请检查是否存在路径或者配置内数据是否为空！");
            }
        }

        private void OpenFlowCsv() {
            string filePath = Application.dataPath + "/StreamingAssets/Csv/FlowGenerate.csv";
            Process.Start(filePath);
        }
    }
}