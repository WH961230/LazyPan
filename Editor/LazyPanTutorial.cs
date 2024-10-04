using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class LazyPanTutorial : EditorWindow {
        private Texture2D image;
        public void OnStart(LazyPanTool lazyPanTool) {
            image = AssetDatabase.LoadAssetAtPath<Texture2D>($"Packages/evoreek.lazypan/Editor/Image/FlowIcon.png");
        }

        public void OnCustomGUI(float areaX) {
            GUILayout.BeginArea(new Rect(areaX, 60, Screen.width, Screen.height));

            GUILayout.BeginHorizontal();
            GUIStyle style = LazyPanTool.GetGUISkin("LogoGUISkin").GetStyle("label");
            GUILayout.Label("TUTORIAL", style);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AnnotationGUISkin").GetStyle("label");
            GUILayout.Label("@教程", style);
            GUILayout.EndHorizontal();
            
            EditorStyles.foldout.fontSize = 20;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("TitleGUISkin").GetStyle("label");
            GUILayout.Label("", style);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("打开框架教程", style)) {
                Application.OpenURL("https://space.bilibili.com/29326484?spm_id_from=333.1007.0.0");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("=> Flow 打开流程配置教程", style)) {
                Application.OpenURL("https://www.bilibili.com/video/BV1aZtLe4E3R/");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("=> Entity 打开实体配置教程", style)) {
                Application.OpenURL("https://www.bilibili.com/video/BV1aZtLe4E3R/");
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            style = LazyPanTool.GetGUISkin("AButtonGUISkin").GetStyle("button");
            if (GUILayout.Button("=> Behaviour 打开行为配置教程", style)) {
                Application.OpenURL("https://www.bilibili.com/video/BV1aZtLe4E3R/");
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}