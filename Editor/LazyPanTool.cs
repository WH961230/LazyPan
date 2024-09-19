using System;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace LazyPan {
    public class LazyPanTool : EditorWindow {
        public Dictionary<Type, EditorWindow> editorWindows = new Dictionary<Type, EditorWindow>();

        private float areaX = 0;
        private float screenWidth = 1000;
        private float screenHeight = 800;
        private float animToolBarSpeed = 3f;
        private bool isAnimToolBar = false;
        private int currentToolBar = 0;
        private int lastToolBar = 0;
        private int previousToolBar = 0;

        private string[] values = {
            "引导首页 GuideHomePage", "流程 Flow", "实体 Entity", "行为 Behaviour"
        };

        [MenuItem("LazyPan/打开引导面板 _F1")]
        public static void OpenGuide() {
            LazyPanTool window = (LazyPanTool)GetWindow(typeof(LazyPanTool), true, "LAZYPAN 框架", true);
            window.Show();
        }

        private void OnGUI() {
            ToolBar();
            Jump();
        }

        //标签
        private void ToolBar() {
            currentToolBar = GUILayout.Toolbar(currentToolBar, values, GUILayout.Height(30));
            switch (currentToolBar) {
                case 0:
                    if (editorWindows.TryGetValue(typeof(LazyPanGuide), out EditorWindow ret)) {
                        if (ret is LazyPanGuide ep) {
                            if (lastToolBar != currentToolBar) {
                                ep.OnStart();
                                lastToolBar = currentToolBar;
                            }
                            ep.OnCustomGUI(areaX);
                        }
                    } else {
                        editorWindows.Add(typeof(LazyPanGuide), CreateInstance<LazyPanGuide>());
                    }

                    break;
                case 1:
                    if (editorWindows.TryGetValue(typeof(LazyPanFlow), out ret)) {
                        if (ret is LazyPanFlow ep) {
                            if (lastToolBar != currentToolBar) {
                                ep.OnStart();
                                lastToolBar = currentToolBar;
                            }
                            ep.OnCustomGUI(areaX);
                        }
                    } else {
                        editorWindows.Add(typeof(LazyPanFlow), CreateInstance<LazyPanFlow>());
                    }

                    break;
                case 2:
                    if (editorWindows.TryGetValue(typeof(LazyPanEntity), out ret)) {
                        if (ret is LazyPanEntity ep) {
                            if (lastToolBar != currentToolBar) {
                                ep.OnStart();
                                lastToolBar = currentToolBar;
                            }
                            ep.OnCustomGUI(areaX);
                        }
                    } else {
                        editorWindows.Add(typeof(LazyPanEntity), CreateInstance<LazyPanEntity>());
                    }

                    break;
                case 3:
                    if (editorWindows.TryGetValue(typeof(LazyPanBehaviour), out ret)) {
                        if (ret is LazyPanBehaviour ep) {
                            if (lastToolBar != currentToolBar) {
                                ep.OnStart();
                                lastToolBar = currentToolBar;
                            }
                            ep.OnCustomGUI(areaX);
                        }
                    } else {
                        editorWindows.Add(typeof(LazyPanBehaviour), CreateInstance<LazyPanBehaviour>());
                    }

                    break;
            }
        }

        //跳转动画
        private void Jump() {
            //点击新的标签页
            if (previousToolBar != currentToolBar) {
                isAnimToolBar = true;
                areaX = previousToolBar < currentToolBar ? Screen.width : -Screen.width;
                previousToolBar = currentToolBar;
            }

            //动画转换
            if (isAnimToolBar) {
                float sign = -Mathf.Sign(areaX);
                areaX += sign * animToolBarSpeed;
                //动画超过边界
                if (Mathf.Abs(areaX) < 0.05f) {
                    isAnimToolBar = false;
                }

                Repaint();
            }
        }

        public static GUISkin GetGUISkin(string guiskinname) {
            return AssetDatabase.LoadAssetAtPath<GUISkin>(
                $"Packages/evoreek.lazypan/Runtime/Bundles/GUISkin/{guiskinname}.guiskin");
        }

        //压缩文件
        public static void CompressFile(string sourceFolder, string destinationZipFile) {
            try {
                ZipFile.CreateFromDirectory(sourceFolder, destinationZipFile, CompressionLevel.Optimal, false);
                Debug.Log("Folder compressed successfully.");
            } catch (Exception ex) {
                Debug.LogError($"Failed to compress folder: {ex.Message}");
            }
        }

        //解压文件
        public static void DecompressFile(string sourceZipFile, string destinationFolder) {
            try {
                ZipFile.ExtractToDirectory(sourceZipFile, destinationFolder);
                Debug.Log("Folder decompressed successfully.");
            } catch (Exception ex) {
                Debug.LogError($"Failed to decompress folder: {ex.Message}");
            }
        }
    }
}