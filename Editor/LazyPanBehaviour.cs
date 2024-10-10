using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class LazyPanBehaviour : EditorWindow {
        private bool isFoldoutTool;
        private bool isFoldoutBehaviour;
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
            isFoldoutBehaviour = true;
        }

        public void OnCustomGUI(float areaX) {
            GUILayout.BeginArea(new Rect(areaX, 60, Screen.width, Screen.height));
            Title();
            AutoTool();
            ManualGenerateBehaviourTool();
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
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUIStyle style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if(GUILayout.Button("打开行为配置表 Csv", style)) {
                    OpenBehaviourCsv(false);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if(GUILayout.Button("打开行为自动脚本生成配置表 Csv", style)) {
                    OpenBehaviourCsv(true);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("点击此处 自动生成行为模板(需手动修改生成的 XXX_Behaviour_Template 后面的 Template 删除且放到 Behaviour 目录下)", style)) {
                    AutoGenerateBehaviourTemplate();
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
                height += GUILayoutUtility.GetLastRect().height;
            } else {
                GUILayout.Space(10);
            }
            
            LazyPanTool.DrawBorder(new Rect(rect.x + 2f, rect.y - 2f, rect.width - 2f, rect.height + height + 5f), Color.white);

            GUILayout.Space(10);
        }

        private void ManualGenerateBehaviourTool() {
            isFoldoutBehaviour = EditorGUILayout.Foldout(isFoldoutBehaviour, " 手动创建行为工具", true);
            Rect rect = GUILayoutUtility.GetLastRect();
            float height = 0;
            if (isFoldoutBehaviour) {
                GUILayout.Label("开发中，敬请期待，实现可以自动配置生成行为模板！");
                height += GUILayoutUtility.GetLastRect().height;
                /*
                GUILayout.BeginVertical();
                GUILayout.Label("");
                GUILayout.BeginHorizontal();
                string _instanceFlowName = "";
                _instanceFlowName = EditorGUILayout.TextField("流程标识(必填)", _instanceFlowName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                string _instanceTypeName = "";
                _instanceTypeName = EditorGUILayout.TextField("类型标识(必填)", _instanceTypeName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                string _instanceObjName = "";
                _instanceObjName = EditorGUILayout.TextField("实体标识(必填)", _instanceObjName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUI.SetNextControlName("objChineseName");
                GUILayout.BeginHorizontal();
                string _instanceObjChineseName = "";
                _instanceObjChineseName = EditorGUILayout.TextField("实体中文名字(必填)", _instanceObjChineseName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUIStyle style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if(GUILayout.Button("创建实体物体", style)) {
                    //InstanceCustomObj();
                }
                if(GUILayout.Button("创建实体物体点位配置", style)) {
                    //InstanceCustomLocationSetting();
                }
                GUILayout.EndVertical();
                height += GUILayoutUtility.GetLastRect().height;
                */
            } else {
                GUILayout.Space(10);
            }
            
            LazyPanTool.DrawBorder(new Rect(rect.x + 2f, rect.y - 2f, rect.width - 2f, rect.height + height + 5f), Color.white);

            GUILayout.Space(10);
        }

        private void AutoGenerateBehaviourTemplate() {
            Generate.GenerateBehaviour(true);
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