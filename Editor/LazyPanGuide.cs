using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LazyPan {
    public class LazyPanGuide : EditorWindow {
        private bool isFoldout = true;
        private LazyPanTool _tool;

        public void OnStart(LazyPanTool tool) {
            _tool = tool;
        }

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
            
            GUILayout.Space(10);

            EditorStyles.foldout.fontSize = 20;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            isFoldout = EditorGUILayout.Foldout(isFoldout, " LazyPan 环境配置", true);
            Rect rect = GUILayoutUtility.GetLastRect();
            float height = 0;
            if (isFoldout) {
                GUILayout.BeginArea(new Rect(0, 120, Screen.width, Screen.height));

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第零步: 点击按钮自动清除即将部署的同名目录谨慎使用！   （目录包含 Assets 下的   1、AddressableAssetsData   2、LazyPan   3、StreamingAssets   4、TextMesh Pro）", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动清除即将部署的同名目录", style)) {
                    DeleteExitDirectory();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第一步: 点击按钮自动创建框架目录", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动创建框架目录", style)) {
                    CreateBaseFilePath();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第二步: 点击按钮自动拷贝核心文件", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动拷贝核心文件（拷贝场景 CSV游戏配置 游戏自动化生成的Txt模板 游戏输入系统 等）", style)) {
                    CopyFilesToDirectory("Bundles/Configs/Input", "LazyPan/Bundles/Configs/Input");
                    CopyFilesToDirectory("Bundles/Configs/Setting", "LazyPan/Bundles/Configs/Setting");
                    CopyFilesToDirectory("Bundles/Configs/Txt", "LazyPan/Bundles/Configs/Txt");
                    CopyFilesToDirectory("Bundles/Prefabs/Global", "LazyPan/Bundles/Prefabs/Global");
                    CopyFilesToDirectory("Bundles/Prefabs/Obj", "LazyPan/Bundles/Prefabs/Obj");
                    CopyFilesToDirectory("Bundles/Prefabs/Tool", "LazyPan/Bundles/Prefabs/Tool");
                    CopyFilesToDirectory("Bundles/Prefabs/UI", "LazyPan/Bundles/Prefabs/UI");
                    CopyFilesToDirectory("Bundles/Scenes", "LazyPan/Bundles/Scenes");
                    CopyFilesToDirectory("Bundles/TextMeshPro", "");
                    CopyFilesToDirectory("Bundles/Csv/StreamingAssets/Csv", "StreamingAssets/Csv");
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第三步: 点击按钮自动配置Addressable资源", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动配置Addressable资源", style)) {
                    CreateAddressableAsset();
                    AutoInstallAddressableData();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第四步: 点击按钮自动装载场景到BuildSettings", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动装载场景到BuildSettings", style)) {
                    MoveSceneToBuildSettings();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第五步: 点击按钮自动创建流程", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动创建框架流程", style)) {
                    AutoGenerateFlow();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第六步: 点击按钮自动生成行为(或模板)", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 自动生成行为", style)) {
                    AutoGenerateBehaviour();
                }
                if (GUILayout.Button("点击此处 自动生成行为模板(需手动修改生成的 XXX_Behaviour_Template 后面的 Template 删除且放到 Behaviour 目录下)", style)) {
                    AutoGenerateBehaviourTemplate();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第七步: 点击按钮加载框架预设的测试行为", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 加载框架预设的测试行为", style)) {
                    AutoDecompressBehaviourFile();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
                GUILayout.Label("第八步: 打开入口场景 Launch", style);
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;

                GUILayout.BeginHorizontal();
                style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
                if (GUILayout.Button("点击此处 打开入口场景Launch 测试并运行", style)) {
                    TestSceneAndPlay();
                }
                GUILayout.EndHorizontal();
                height += GUILayoutUtility.GetLastRect().height;
                height += 90;

                GUILayout.EndArea();
            } else {
                GUILayout.Space(10);
            }
            
            LazyPanTool.DrawBorder(new Rect(rect.x + 2f, rect.y - 2f, rect.width - 2f, rect.height + height + 5f), Color.white);

            GUILayout.EndArea();
        }

        private void DeleteExitDirectory() {
            string pathA = "Assets/AddressableAssetsData";
            string pathB = "Assets/LazyPan";
            string pathC = "Assets/StreamingAssets";
            string pathD = "Assets/TextMesh Pro";
            if (AssetDatabase.IsValidFolder(pathA)) {
                AssetDatabase.DeleteAsset(pathA);
            }
            if (AssetDatabase.IsValidFolder(pathB)) {
                AssetDatabase.DeleteAsset(pathB);
            }
            if (AssetDatabase.IsValidFolder(pathC)) {
                AssetDatabase.DeleteAsset(pathC);
            }
            if (AssetDatabase.IsValidFolder(pathD)) {
                AssetDatabase.DeleteAsset(pathD);
            }
            AssetDatabase.Refresh();
        }

        private void AutoDecompressBehaviourFile() {
            LazyPanTool.DecompressFile($"Packages/evoreek.lazypan/Test/BehaviourZipPack.zip", "Assets/LazyPan/Scripts/GamePlay/Behaviour");
            AssetDatabase.Refresh();
        }

        private void CreateBaseFilePath() {
            string targetBundlesPath = "Assets/LazyPan/Bundles";

            string targetBundlesConfigsPath = "Assets/LazyPan/Bundles/Configs";
            string targetBundlesConfigsInputPath = "Assets/LazyPan/Bundles/Configs/Input";
            string targetBundlesConfigsTxtPath = "Assets/LazyPan/Bundles/Configs/Txt";
            string targetBundlesConfigsSettingPath = "Assets/LazyPan/Bundles/Configs/Setting";
            string targetBundlesConfigsSettingLocationInformationSettingPath =
                "Assets/LazyPan/Bundles/Configs/Setting/LocationInformationSetting";

            string targetBundlesImagesPath = "Assets/LazyPan/Bundles/Images";

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

            if (!Directory.Exists(targetBundlesPath)) {
                Directory.CreateDirectory(targetBundlesPath);
            }

            if (!Directory.Exists(targetBundlesConfigsPath)) {
                Directory.CreateDirectory(targetBundlesConfigsPath);
            }

            if (!Directory.Exists(targetBundlesConfigsInputPath)) {
                Directory.CreateDirectory(targetBundlesConfigsInputPath);
            }

            if (!Directory.Exists(targetBundlesConfigsTxtPath)) {
                Directory.CreateDirectory(targetBundlesConfigsTxtPath);
            }

            if (!Directory.Exists(targetBundlesConfigsSettingPath)) {
                Directory.CreateDirectory(targetBundlesConfigsSettingPath);
            }

            if (!Directory.Exists(targetBundlesConfigsSettingLocationInformationSettingPath)) {
                Directory.CreateDirectory(targetBundlesConfigsSettingLocationInformationSettingPath);
            }

            if (!Directory.Exists(targetBundlesImagesPath)) {
                Directory.CreateDirectory(targetBundlesImagesPath);
            }

            if (!Directory.Exists(targetBundlesMaterialsPath)) {
                Directory.CreateDirectory(targetBundlesMaterialsPath);
            }

            if (!Directory.Exists(targetBundlesPrefabsPath)) {
                Directory.CreateDirectory(targetBundlesPrefabsPath);
            }

            if (!Directory.Exists(targetBundlesPrefabsGlobalPath)) {
                Directory.CreateDirectory(targetBundlesPrefabsGlobalPath);
            }

            if (!Directory.Exists(targetBundlesPrefabsObjPath)) {
                Directory.CreateDirectory(targetBundlesPrefabsObjPath);
            }

            if (!Directory.Exists(targetBundlesPrefabsToolPath)) {
                Directory.CreateDirectory(targetBundlesPrefabsToolPath);
            }

            if (!Directory.Exists(targetBundlesPrefabsUIPath)) {
                Directory.CreateDirectory(targetBundlesPrefabsUIPath);
            }

            if (!Directory.Exists(targetScriptsPath)) {
                Directory.CreateDirectory(targetScriptsPath);
            }

            if (!Directory.Exists(targetScriptsGamePlayPath)) {
                Directory.CreateDirectory(targetScriptsGamePlayPath);
            }

            if (!Directory.Exists(targetScriptsGamePlayBehaviourPath)) {
                Directory.CreateDirectory(targetScriptsGamePlayBehaviourPath);
            }

            if (!Directory.Exists(targetScriptsGamePlayBehaviourTemplatePath)) {
                Directory.CreateDirectory(targetScriptsGamePlayBehaviourTemplatePath);
            }

            if (!Directory.Exists(targetScriptsGamePlayConfigPath)) {
                Directory.CreateDirectory(targetScriptsGamePlayConfigPath);
            }

            if (!Directory.Exists(targetScriptsGamePlayDataPath)) {
                Directory.CreateDirectory(targetScriptsGamePlayDataPath);
            }

            if (!Directory.Exists(targetScriptsGamePlayFlowPath)) {
                Directory.CreateDirectory(targetScriptsGamePlayFlowPath);
            }

            AssetDatabase.Refresh();
        }

        private void CreateAddressableAsset() {
            AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(
                AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
                AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
        }

        private void AutoInstallAddressableData() {
            /*游戏总配置*/
            string targetGameSettingPath = $"Packages/evoreek.lazypan/Runtime/Bundles/GameSetting/GameSetting.asset";
            AddAssetToAddressableEntries(targetGameSettingPath);

            /*游戏配置*/
            string targetBundlesConfigsPath = "Assets/LazyPan/Bundles/Configs";
            if (Directory.Exists(targetBundlesConfigsPath)) {
                AddAssetToAddressableEntries(targetBundlesConfigsPath);
            }

            /*游戏不同类型资源加载目录*/
            string targetBundlesPrefabsGlobalPath = "Assets/LazyPan/Bundles/Prefabs/Global";
            string targetBundlesPrefabsObjPath = "Assets/LazyPan/Bundles/Prefabs/Obj";
            string targetBundlesPrefabsToolPath = "Assets/LazyPan/Bundles/Prefabs/Tool";
            string targetBundlesPrefabsUIPath = "Assets/LazyPan/Bundles/Prefabs/UI";
            string targetBundlesPrefabsImagePath = "Assets/LazyPan/Bundles/Images";

            if (Directory.Exists(targetBundlesPrefabsGlobalPath)) {
                AddAssetToAddressableEntries(targetBundlesPrefabsGlobalPath);
            }

            if (Directory.Exists(targetBundlesPrefabsObjPath)) {
                AddAssetToAddressableEntries(targetBundlesPrefabsObjPath);
            }

            if (Directory.Exists(targetBundlesPrefabsToolPath)) {
                AddAssetToAddressableEntries(targetBundlesPrefabsToolPath);
            }

            if (Directory.Exists(targetBundlesPrefabsUIPath)) {
                AddAssetToAddressableEntries(targetBundlesPrefabsUIPath);
            }

            if (Directory.Exists(targetBundlesPrefabsImagePath)) {
                AddAssetToAddressableEntries(targetBundlesPrefabsImagePath);
            }

            /*输入控制器*/
            string targetInputControlPath = "Assets/LazyPan/Bundles/Configs/Input/LazyPanInputControl.inputactions";
            AddAssetToAddressableEntries(targetInputControlPath);
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

        public void CopyFilesToDirectory(string sourceDirectory, string destinationDirectory) {
            string sourcePath = $"Packages/evoreek.lazypan/Runtime/{sourceDirectory}"; // 源文件夹路径
            string targetPath = $"Assets/{destinationDirectory}"; // 目标文件夹路径
            // 检查源目录是否存在
            if (!Directory.Exists(sourcePath)) {
                Debug.LogError($"Source directory does not exist: {sourcePath}");
                return;
            }

            // 创建目标目录（如果不存在）
            if (!Directory.Exists(targetPath)) {
                Directory.CreateDirectory(targetPath);
            }

            // 复制源目录及其子目录中的所有文件
            CopyDirectory(sourcePath, targetPath);

            Debug.Log($"Files copied from {sourcePath} to {targetPath}");

            AssetDatabase.Refresh();
        }

        private void CopyDirectory(string sourceDir, string destDir) {
            // 创建目标目录
            Directory.CreateDirectory(destDir);

            // 获取源目录中的所有文件
            string[] files = Directory.GetFiles(sourceDir);
            foreach (string file in files) {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destDir, fileName);
                File.Copy(file, destFile, true); // 设置true以覆盖目标目录中的同名文件
            }

            // 获取源目录中的所有子目录
            string[] directories = Directory.GetDirectories(sourceDir);
            foreach (string directory in directories) {
                string directoryName = Path.GetFileName(directory);
                string destDirectory = Path.Combine(destDir, directoryName);
                CopyDirectory(directory, destDirectory); // 递归复制子目录
            }
        }

        public void MoveSceneToBuildSettings() {
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[0]; // 清空 Build Settings 中的场景
            // 获取指定文件夹中的所有场景文件
            string[] sceneFiles =
                Directory.GetFiles("Assets/LazyPan/Bundles/Scenes", "*.unity", SearchOption.AllDirectories);
            List<EditorBuildSettingsScene> newScenes = new List<EditorBuildSettingsScene>();
            // 将所有场景文件添加到 Build Settings 中
            foreach (string sceneFile in sceneFiles) {
                newScenes.Add(new EditorBuildSettingsScene(sceneFile, true));
            }

            // 更新 Build Settings 场景列表
            EditorBuildSettings.scenes = newScenes.ToArray();
        }

        private void AutoGenerateFlow() {
            Generate.GenerateFlow();
        }

        private void AutoGenerateBehaviour() {
            Generate.GenerateBehaviour(false);
        }

        private void AutoGenerateBehaviourTemplate() {
            Generate.GenerateBehaviour(true);
        }

        private void TestSceneAndPlay() {
            EditorSceneManager.OpenScene("Assets/LazyPan/Bundles/Scenes/Launch.unity");
            EditorApplication.isPlaying = true;
        }
    }
}