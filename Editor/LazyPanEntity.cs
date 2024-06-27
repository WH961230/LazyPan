using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace LazyPan {
    public class LazyPanEntity : EditorWindow {
        private string _instanceFlowName;
        private string _instanceTypeName;
        private string _instanceObjName;
        private string _instanceObjChineseName;
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

            GUILayout.BeginVertical();
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
            if(GUILayout.Button("创建实体物体", style, GUILayout.Height(80))) {
                InstanceCustomObj();
            }
            if(GUILayout.Button("创建实体物体点位配置", style, GUILayout.Height(80))) {
                InstanceCustomLocationSetting();
            }
            GUILayout.EndVertical();
        
            GUILayout.EndArea();
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