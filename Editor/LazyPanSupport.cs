using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class LazyPanSupport : EditorWindow {
        private bool isFoldoutList;
        private Texture2D image;
        public void OnStart(LazyPanTool lazyPanTool) {
            image = AssetDatabase.LoadAssetAtPath<Texture2D>($"Packages/evoreek.lazypan/Editor/Image/赞赏码.jpg");
        }

        public void OnCustomGUI(float areaX) {
            GUILayout.BeginArea(new Rect(areaX, 60, Screen.width, Screen.height));

            GUILayout.BeginHorizontal();
            GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
            GUILayout.Label("SUPPORT", style);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
            GUILayout.Label("@支持发条游戏工作室", style);
            GUILayout.EndHorizontal();
            
            EditorStyles.foldout.fontSize = 20;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
            GUILayout.Label("", style);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("❤@  作者B站主页  @❤", style)) {
                Application.OpenURL("https://space.bilibili.com/29326484?spm_id_from=333.1007.0.0");
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("❤@  给作者充个电  @❤", style)) {
                Support window = (Support)GetWindow(typeof(Support), true, "赞赏与支持", true);
                window.SetImage(image);
                window.Show();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("❤@  特别鸣谢  @❤", style)) {
                SupportFans window = (SupportFans)GetWindow(typeof(SupportFans), true, "特别鸣谢", true);
                window.Show();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        public class Support : EditorWindow {
            private Texture2D _image;
            public void SetImage(Texture2D image) {
                _image = image;
            }

            private void OnGUI() {
                if (_image != null) {
                    // 计算窗口和图片大小
                    var imageSize = new Vector2(_image.width, _image.height);
                    var windowSize = new Vector2(imageSize.x, imageSize.y);

                    // 设置窗口大小
                    minSize = windowSize;
                    maxSize = windowSize;

                    // 绘制图片
                    GUILayout.BeginArea(new Rect(0, 0, imageSize.x, imageSize.y));
                    GUILayout.Label(_image);
                    // GUILayout.Label("打赏后私聊作者 GitHubID 写入特别鸣谢！");
                    GUILayout.EndArea();
                }
            }
        }
        
        public class SupportFans : EditorWindow {
            private void OnGUI() {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("特别鸣谢");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("（排名不分前后 感谢小伙伴们对 LazyPan 插件的支持！）");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(20);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("BewanHeyonga")) {
                    Application.OpenURL("https://github.com/BewanHeyonga");
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}