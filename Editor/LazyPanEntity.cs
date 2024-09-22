using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace LazyPan {
    public class LazyPanEntity : EditorWindow {
        private bool isFoldoutTool;
        private bool isFoldoutData;
        private bool isFoldoutGenerate;
        private string _instanceFlowName;
        private string _instanceTypeName;
        private string _instanceObjName;
        private string _instanceObjChineseName;
        private string[][] EntityConfigStr;
        private LazyPanTool _tool;
        
        public void OnStart(LazyPanTool tool) {
            _tool = tool;
            ReadCSV.Instance.Read("ObjConfig", out string content, out string[] lines);
            if (lines != null && lines.Length > 0) {
                EntityConfigStr = new string[lines.Length - 2][];
                for (int i = 0; i < lines.Length; i++) {
                    if (i > 2) {
                        //遍历第三行到最后一行
                        //遍历每一行数据
                        string[] lineStr = lines[i].Split(",");
                        EntityConfigStr[i - 3] = new string[lineStr.Length];
                        if (lineStr.Length > 0) {
                            for (int j = 0; j < lineStr.Length; j++) {
                                EntityConfigStr[i - 3][j] = lineStr[j];
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
            GUILayout.Label("ENTITY", style, GUILayout.Height(80));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
            GUILayout.Label("@实体 游戏内最小单位", style);
            GUILayout.EndHorizontal();

            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            isFoldoutTool = EditorGUILayout.Foldout(isFoldoutTool, " 自动化工具", true);
            if (isFoldoutTool) {
                GUILayout.Label("");
                if(GUILayout.Button("打开实体配置表 Csv", style, GUILayout.Height(80))) {
                    GUILayout.BeginHorizontal();
                    OpenEntityCsv();
                    GUILayout.EndHorizontal();
                }
            }
            
            // 增加间距
            GUILayout.Space(10); // 增加10像素的上下间距
            
            isFoldoutData = EditorGUILayout.Foldout(isFoldoutData, " 预览实体配置数据", true);
            if (isFoldoutData) {
                ExpandEntityData();
            }

            // 增加间距
            GUILayout.Space(10); // 增加10像素的上下间距

            isFoldoutGenerate = EditorGUILayout.Foldout(isFoldoutGenerate, " 手动创建预制体工具", true);
            if (isFoldoutGenerate) {
                GUILayout.BeginVertical();
                GUILayout.Label("");
                GUILayout.BeginHorizontal();
                _instanceFlowName = EditorGUILayout.TextField("流程标识(必填)", _instanceFlowName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                _instanceTypeName = EditorGUILayout.TextField("类型标识(必填)", _instanceTypeName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                _instanceObjName = EditorGUILayout.TextField("实体标识(必填)", _instanceObjName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                _instanceObjChineseName = EditorGUILayout.TextField("实体中文名字(必填)", _instanceObjChineseName, GUILayout.Height(20));
                GUILayout.EndHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if(GUILayout.Button("创建实体物体", style)) {
                    InstanceCustomObj();
                }
                if(GUILayout.Button("创建实体物体点位配置", style)) {
                    InstanceCustomLocationSetting();
                }
                GUILayout.EndVertical();
            }
        
            GUILayout.EndArea();
        }

        private void ExpandEntityData() {
            bool hasContent = false;
            if (EntityConfigStr != null && EntityConfigStr.Length > 0) {
                GUILayout.BeginVertical();
                string entitySign = "";
                foreach (var str in EntityConfigStr) {
                    if (str != null) {
                        if (entitySign != str[1]) {
                            entitySign = str[1];
                            GUILayout.Label("");
                        }

                        GUILayout.BeginHorizontal();
                        for (int i = 0 ; i < str.Length ; i ++) {
                            bool hasPrefab = HasPrefabTips(str);
                            Color fontColor;
                            if (i == 1) {
                                fontColor = Color.cyan;
                            } else {
                                fontColor = hasPrefab ? Color.green : Color.red;
                            }
                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                            labelStyle.normal.textColor = fontColor; // 设置字体颜色
                            if (GUILayout.Button(str[i], labelStyle, GUILayout.Width(200))) {
                                UnityEngine.Debug.LogError(str[i]);
                                //点击的实体如果在实体配置存在直接跳转
                                _tool.currentToolBar = 1;
                            }

                            string tooltip = "";
                            // 检测鼠标悬停
                            Rect buttonRect = GUILayoutUtility.GetLastRect();
                            if (buttonRect.Contains(Event.current.mousePosition)) {
                                if (!hasPrefab) {
                                    tooltip = "找不到预制体，请添加: " + str[0];
                                }
                            }

                            // 显示悬浮信息
                            if (!string.IsNullOrEmpty(tooltip)) {
                                Vector2 tooltipPosition = Event.current.mousePosition + new Vector2(10, 10); // 设置悬浮提示位置
                                GUI.Label(new Rect(tooltipPosition.x, tooltipPosition.y, 250, 20), tooltip);
                            }

                            hasContent = true;
                        }
                        GUILayout.EndHorizontal();                
                    }
                }
                GUILayout.EndVertical();
            }

            if(!hasContent || EntityConfigStr == null || EntityConfigStr.Length == 0) {
                GUILayout.Label("找不到 EntityConfig.csv 的配置数据！\n请检查是否存在路径或者配置内数据是否为空！");
            }
        }

        private bool HasPrefabTips(string[] str) {
            string prefabPath = $"Assets/LazyPan/Bundles/Prefabs/Obj/{str[1]}/{str[0]}.prefab";
            return AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null;
        }

        private void ExpandEntityPrefabData() {
            
        }

        private void OpenEntityCsv() {
            string filePath = Application.dataPath + "/StreamingAssets/Csv/ObjConfig.csv";
            Process.Start(filePath);
        }

        private void InstanceCustomObj() {
            if (_instanceObjName == "" || _instanceTypeName == "" || _instanceFlowName == "" || _instanceObjChineseName == "") {
                return;
            }
            string sourcePath = "Packages/evoreek.lazypan/Runtime/Bundles/Prefabs/Obj/Obj_Sample_Sample.prefab"; // 替换为你的预制体源文件路径
            string targetFolderPath = "Assets/LazyPan/Bundles/Prefabs/Obj"; // 替换为你想要拷贝到的目标文件夹路径
            
            // 获取选中的游戏对象
            GameObject selectedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
            if (selectedPrefab != null && PrefabUtility.IsPartOfPrefabAsset(selectedPrefab)) {
                // 确保目标文件夹存在
                if (!Directory.Exists(targetFolderPath)) {
                    Directory.CreateDirectory(targetFolderPath);
                }

                // 获取预制体路径
                string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);
                
                // 拷贝预制体到目标文件夹
                string targetPath = Path.Combine(targetFolderPath, Path.GetFileName(prefabPath));
                AssetDatabase.CopyAsset(prefabPath, targetPath);
                
                // 刷新AssetDatabase
                AssetDatabase.Refresh();
                
                //修改资源的名字为自定义
                AssetDatabase.RenameAsset(targetPath,
                    string.Concat(_instanceFlowName, _instanceFlowName != null ? "/" : "", "Obj_", _instanceTypeName,
                        "_", _instanceObjName));
                AssetDatabase.Refresh();
            }
        }

        private void InstanceCustomLocationSetting() {
            if (_instanceObjName == "" || _instanceTypeName == "" || _instanceFlowName == "" || _instanceObjChineseName == "") {
                return;
            }

            // 创建实例并赋值
            LocationInformationSetting testAsset = CreateInstance<LocationInformationSetting>();
            testAsset.SettingName = _instanceObjChineseName;
            testAsset.locationInformationDatas = new List<LocationInformationData>();
            testAsset.locationInformationDatas.Add(new LocationInformationData());

            // 替换为你希望保存的目录路径，例如 "Assets/MyFolder/"
            string savePath = "Assets/LazyPan/Bundles/Configs/Setting/LocationInformationSetting/";

            // 确保目标文件夹存在，如果不存在则创建
            if (!AssetDatabase.IsValidFolder(savePath)) {
                AssetDatabase.CreateFolder("Assets", "LazyPan/Bundles/Configs/Setting/LocationInformationSetting");
            }

            // 生成一个唯一的文件名
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(savePath + string.Concat("LocationInf_", _instanceFlowName, "_", _instanceObjName, ".asset"));

            // 将实例保存为.asset文件
            AssetDatabase.CreateAsset(testAsset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}