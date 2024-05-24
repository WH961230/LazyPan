using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

public class LazyPanGuide : EditorWindow {
    private bool isFoldout;
    public void OnCustomGUI(float areaX) {
        GUILayout.BeginArea(new Rect(areaX, 60, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();
        GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
        GUILayout.Label("LAZYPAN", style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
        GUILayout.Label("@LazyPan开发组工具 version 0.0.1", style);
        GUILayout.EndHorizontal();

        EditorStyles.foldout.fontSize = 20;
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        isFoldout = EditorGUILayout.Foldout(isFoldout, "LazyPan 环境配置", true);
        if (isFoldout) {
            GUILayout.BeginArea(new Rect(0, 120, Screen.width, Screen.height));

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
            GUILayout.Label("第一步: 点击按钮自动创建框架目录", style);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("点击此处 自动创建框架目录", style)) {
                CreateBaseFilePath();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
            GUILayout.Label("第二步: 点击按钮自动拷贝核心文件", style);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("点击此处 自动拷贝核心文件（拷贝CSV游戏配置 游戏自动化生成的Txt模板 游戏输入系统 等）", style)) {
                MoveCsvFilesToTheTargetDir("Bundles/Configs/Input", "LazyPan/Bundles/Configs/Input");
                MoveCsvFilesToTheTargetDir("Bundles/Configs/Setting", "LazyPan/Bundles/Configs/Setting");
                MoveCsvFilesToTheTargetDir("Bundles/Configs/Txt", "LazyPan/Bundles/Configs/Txt");
                MoveCsvFilesToTheTargetDir("Bundles/Csv/StreamingAssets/Csv", "StreamingAssets/Csv");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
            GUILayout.Label("第三步: 点击按钮自动配置Addressable资源", style);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("点击此处 自动配置Addressable资源", style)) {
                CreateAddressableAsset();
                AutoInstallAddressableData();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        GUILayout.EndArea();
    }

    private void CreateBaseFilePath() {
        string targetBundlesPath = "Assets/LazyPan/Bundles";
        string targetBundlesConfigsPath = "Assets/LazyPan/Bundles/Configs";
        string targetBundlesConfigsInputPath = "Assets/LazyPan/Bundles/Configs/Input";
        string targetBundlesConfigsTxtPath = "Assets/LazyPan/Bundles/Configs/Txt";
        string targetBundlesImagesPath = "Assets/LazyPan/Bundles/Images";
        string targetBundlesConfigsSettingPath = "Assets/LazyPan/Bundles/Configs/Setting";
        string targetBundlesConfigsSettingLocationInformationSettingPath = "Assets/LazyPan/Bundles/Configs/Setting/LocationInformationSetting";
        string targetBundlesMaterialsPath = "Assets/LazyPan/Bundles/Materials";
        string targetBundlesPrefabsPath = "Assets/LazyPan/Bundles/Prefabs";
        string targetBundlesPrefabsGlobalPath = "Assets/LazyPan/Bundles/Prefabs/Global";
        string targetBundlesPrefabsObjPath = "Assets/LazyPan/Bundles/Prefabs/Obj";
        string targetBundlesPrefabsToolPath = "Assets/LazyPan/Bundles/Prefabs/Tool";
        string targetBundlesPrefabsUIPath = "Assets/LazyPan/Bundles/Prefabs/UI";
        string targetScriptsPath = "Assets/LazyPan/Scripts";
        string targetScriptsGamePlayPath = "Assets/LazyPan/Scripts/GamePlay";
        string targetScriptsGamePlayBehaviourPath = "Assets/LazyPan/Scripts/GamePlay/Behaviour";
        string targetScriptsGamePlayBehaviourTemplatePath = "Assets/LazyPan/Scripts/GamePlay/Behaviour/Template";
        string targetScriptsGamePlayConfigPath = "Assets/LazyPan/Scripts/GamePlay/Config";
        string targetScriptsGamePlayDataPath = "Assets/LazyPan/Scripts/GamePlay/Data";
        string targetScriptsGamePlayFlowPath = "Assets/LazyPan/Scripts/GamePlay/Flow";

        if (!Directory.Exists(targetBundlesPath)) { Directory.CreateDirectory(targetBundlesPath); }
        if (!Directory.Exists(targetBundlesConfigsPath)) { Directory.CreateDirectory(targetBundlesConfigsPath); }
        if (!Directory.Exists(targetBundlesConfigsInputPath)) { Directory.CreateDirectory(targetBundlesConfigsInputPath); }
        if (!Directory.Exists(targetBundlesConfigsTxtPath)) { Directory.CreateDirectory(targetBundlesConfigsTxtPath); }
        if (!Directory.Exists(targetBundlesImagesPath)) { Directory.CreateDirectory(targetBundlesImagesPath); }
        if (!Directory.Exists(targetBundlesConfigsSettingPath)) { Directory.CreateDirectory(targetBundlesConfigsSettingPath); }
        if (!Directory.Exists(targetBundlesConfigsSettingLocationInformationSettingPath)) { Directory.CreateDirectory(targetBundlesConfigsSettingLocationInformationSettingPath); }
        if (!Directory.Exists(targetBundlesMaterialsPath)) { Directory.CreateDirectory(targetBundlesMaterialsPath); }
        if (!Directory.Exists(targetBundlesPrefabsPath)) { Directory.CreateDirectory(targetBundlesPrefabsPath); }
        if (!Directory.Exists(targetBundlesPrefabsGlobalPath)) { Directory.CreateDirectory(targetBundlesPrefabsGlobalPath); }
        if (!Directory.Exists(targetBundlesPrefabsObjPath)) { Directory.CreateDirectory(targetBundlesPrefabsObjPath); }
        if (!Directory.Exists(targetBundlesPrefabsToolPath)) { Directory.CreateDirectory(targetBundlesPrefabsToolPath); }
        if (!Directory.Exists(targetBundlesPrefabsUIPath)) { Directory.CreateDirectory(targetBundlesPrefabsUIPath); }
        if (!Directory.Exists(targetScriptsPath)) { Directory.CreateDirectory(targetScriptsPath); }
        if (!Directory.Exists(targetScriptsGamePlayPath)) { Directory.CreateDirectory(targetScriptsGamePlayPath); }
        if (!Directory.Exists(targetScriptsGamePlayBehaviourPath)) { Directory.CreateDirectory(targetScriptsGamePlayBehaviourPath); }
        if (!Directory.Exists(targetScriptsGamePlayBehaviourTemplatePath)) { Directory.CreateDirectory(targetScriptsGamePlayBehaviourTemplatePath); }
        if (!Directory.Exists(targetScriptsGamePlayConfigPath)) { Directory.CreateDirectory(targetScriptsGamePlayConfigPath); }
        if (!Directory.Exists(targetScriptsGamePlayDataPath)) { Directory.CreateDirectory(targetScriptsGamePlayDataPath); }
        if (!Directory.Exists(targetScriptsGamePlayFlowPath)) { Directory.CreateDirectory(targetScriptsGamePlayFlowPath); }
        AssetDatabase.Refresh();
    }

    private void CreateAddressableAsset() {
        AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
            AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
    }

    private void AutoInstallAddressableData() {
        /*游戏总配置*/
        string targetGameSettingPath = $"Packages/evoreek.lazypan/Runtime/Bundles/GameSetting/GameSetting.asset";
        AddAssetToAddressableEntries(targetGameSettingPath);

        /*游戏不同类型资源加载目录*/
        string targetBundlesPrefabsGlobalPath = "Assets/LazyPan/Bundles/Prefabs/Global";
        string targetBundlesPrefabsObjPath = "Assets/LazyPan/Bundles/Prefabs/Obj";
        string targetBundlesPrefabsToolPath = "Assets/LazyPan/Bundles/Prefabs/Tool";
        string targetBundlesPrefabsUIPath = "Assets/LazyPan/Bundles/Prefabs/UI";

        if (Directory.Exists(targetBundlesPrefabsGlobalPath)) { AddAssetToAddressableEntries(targetBundlesPrefabsGlobalPath); }
        if (Directory.Exists(targetBundlesPrefabsObjPath)) { AddAssetToAddressableEntries(targetBundlesPrefabsObjPath); }
        if (Directory.Exists(targetBundlesPrefabsToolPath)) { AddAssetToAddressableEntries(targetBundlesPrefabsToolPath); }
        if (Directory.Exists(targetBundlesPrefabsUIPath)) { AddAssetToAddressableEntries(targetBundlesPrefabsUIPath); }
    }

    private void AddAssetToAddressableEntries(string path) {
        Object dir = AssetDatabase.LoadAssetAtPath<Object>(path);
        string guid = AssetDatabase.AssetPathToGUID(path);
        if (dir != null) {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings != null) {
                var group = settings.DefaultGroup;
                if (group != null) {
                    AddressableAssetEntry entry = group.entries.FirstOrDefault(e => e.guid == guid);
                    if (entry == null) {
                        entry = settings.CreateOrMoveEntry(guid, group, false, false);
                    }
                    entry.address = path;
                }
            }
        }
    }

    private void MoveCsvFilesToTheTargetDir(string inputPath, string outputPath) {
        string sourcePath = $"Packages/evoreek.lazypan/Runtime/{inputPath}"; // 源文件夹路径
        string targetPath = $"Assets/{outputPath}"; // 目标文件夹路径

        if (!Directory.Exists(targetPath)) {
            Directory.CreateDirectory(targetPath);
        }

        foreach (string newPath in Directory.GetFiles(sourcePath, ".", SearchOption.AllDirectories)) {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }

        AssetDatabase.Refresh();
    }
}