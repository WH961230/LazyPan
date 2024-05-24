using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LazyPan {
#if UNITY_EDITOR
    public class Generate : EditorWindow {
        private string behaviourName;
        private string flowName;

        [MenuItem("Assets/Create/LazyPan/生成游戏行为根据行为配置")]
        public static void GenerateBehaviourByBehaviourConfig() {
            EditorWindow window = GetWindow(typeof(Generate), true, "快速生成游戏行为根据行为配置");
            window.position = new Rect(new Vector2(Screen.width, Screen.height), new Vector2(500, 300));
            window.Show();
            window.Focus();
        }

        [MenuItem("Assets/Create/LazyPan/生成配置脚本")]
        public static void GenerateConfig() {
            Object obj = Selection.objects[0];
            GameSetting gameSetting = Loader.LoadGameSetting();
            ReadCSV.Instance.Read(obj.name, out string content, out string[] lines);
            GenerateConfigScript(obj.name, gameSetting, lines);
        }

        [MenuItem("Assets/Create/LazyPan/生成游戏框架根据流程配置")]
        public static void GenerateFrameByFlowConfig() {
            EditorWindow window = GetWindow(typeof(Generate), true, "快速生成游戏框架根据流程配置");
            window.position = new Rect(new Vector2(Screen.width, Screen.height), new Vector2(500, 300));
            window.Show();
            window.Focus();
        }
        
        private static void GenerateConfigScript(string className, GameSetting gameSetting, string[] lines) {
            string property = "";
            string readContent = "";
            string[] propertyName = lines[0].Split(',');
            string[] typeName = lines[1].Split(',');

            for (int i = 0; i < propertyName.Length; i++) {
                property += string.Concat("\t\t", "public ", typeName[i], " ", propertyName[i], ";", "\n");
            }

            for (int i = 0; i < propertyName.Length; i++) {
                string tmpTypeName = typeName[i];
                if (tmpTypeName == "string") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " = values[", i, "];", "\n");
                } else if (tmpTypeName == "int") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " = int.Parse(values[", i, "])", ";", "\n");
                } else if (tmpTypeName == "float") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " = float.Parse(values[", i, "])", ";", "\n");
                } else if (tmpTypeName == "bool") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " =  int.Parse(values[", i, "]) == 1", ";", "\n");
                }
            }

            string inputPath = string.Concat(Application.dataPath, gameSetting.TxtPath, "GenerateConfigTemplate.txt");
            string outputPath = string.Concat(Application.dataPath, gameSetting.ConfigScriptPath);
            CreateConfigScript(inputPath, outputPath, className, property, readContent, "", "");
        }

        private static bool CreateConfigScript(string inputPath, string outputPath, string className, string property,
            string readContent, string front, string end) {
            if (inputPath.EndsWith(".txt")) {
                var streamReader = new StreamReader(inputPath);
                var log = streamReader.ReadToEnd();
                streamReader.Close();
                log = Regex.Replace(log, "#ClassName#", className);
                log = Regex.Replace(log, "#Property#", property);
                log = Regex.Replace(log, "#ReadContent#", readContent);
                var createPath = $"{outputPath}{front}{className}{end}.cs";
                var streamWriter = new StreamWriter(createPath, false, new UTF8Encoding(true, false));
                streamWriter.Write(log);
                streamWriter.Close();
                AssetDatabase.ImportAsset(createPath);
                AssetDatabase.Refresh();
                return true;
            }

            return false;
        }

        public static bool GenerateScript(string inputPath, string outputPath, string className, string front, string end) {
            if (inputPath.EndsWith(".txt")) {
                var streamReader = new StreamReader(inputPath);
                var log = streamReader.ReadToEnd();
                streamReader.Close();
                log = Regex.Replace(log, "#ClassName#", className);
                var createPath = $"{outputPath}{front}{className}{end}.cs";
                var streamWriter = new StreamWriter(createPath, false, new UTF8Encoding(true, false));
                streamWriter.Write(log);
                streamWriter.Close();
                AssetDatabase.ImportAsset(createPath);
                return true;
            }

            return false;
        }

        public static bool GenerateFlow() {
            /*读取配置*/
            ReadCSV.Instance.Read("FlowGenerate", out string content, out string[] lines);
            List<string> openui = new List<string>();
            List<string> closeui = new List<string>();
            Dictionary<string, List<string>> stage = new Dictionary<string, List<string>>();
            /*生成流程脚本*/
            for (int i = 0; i < lines.Length; i++) {//遍历每一行数据
                if (i > 2) {//遍历第三行到最后一行
                    /*类 找到第一个未创建的流程的代码标识*/
                    string[] values = lines[i].Split(',');
                    /*缓存当前流程标识*/
                    string curFlowSign = values[0];
                    if (values[2] == "open_ui") {
                        openui.Add(values[6]);
                    } else if (values[2] == "close_ui") {
                        closeui.Add(values[6]);
                    } else if (values[2] == "load_entity" || values[2] == "unload_entity") {
                        /*创建与销毁*/
                        string key = values[4];
                        string value = string.Concat(values[2], "|", values[6], "|", values[5]);
                        if (stage.ContainsKey(key)) {//自定义阶段 创建
                            stage[key].Add(value);
                        } else {
                            List<string> instance = new List<string>();
                            instance.Add(value);
                            stage.Add(key, instance);
                        }
                    }

                    /*最后一行 或者 下一行不是之前的流程 则 创建流程类 赋值*/
                    if (i + 1 == lines.Length || (i + 1 != lines.Length && lines[i + 1].Split(',')[0] != curFlowSign)) {
                        /*创建*/
                        string inputPath = "Assets/LazyPan/Bundles/Configs/Txt/GenerateFlowTemplate.txt";
                        var streamReader = new StreamReader(inputPath);
                        var log = streamReader.ReadToEnd();
                        streamReader.Close();

                        /*赋值*/
                        log = Regex.Replace(log, "#流程标识#", values[0]);
                        log = Regex.Replace(log, "#流程名#", values[1]);

                        string openuireplace = "";
                        string uifieldreplace = "";
                        string getuireplace = "";

                        /*界面创建*/
                        foreach (string tmpui in openui) {
                            openuireplace += $"\t\t\t{tmpui} = UI.Instance.Open(\"{tmpui}\");\n";
                            /*界面属性*/
                            uifieldreplace += $"\t\tprivate Comp {tmpui};\n";
                            getuireplace += $"\t\t/*获取UI*/\n\t\tpublic Comp GetUI() {{\n\t\t\treturn {tmpui};\n\t\t}}\n";
                        }

                        /*界面销毁*/
                        string closeuireplace = "";
                        foreach (string tmpui in closeui) {
                            closeuireplace += $"\t\t\tUI.Instance.Close(\"{tmpui}\");\n";
                        }

                        string initloadentityreplace = "";//初始化创建实体替换
                        string unloadentityreplace = "";//清除销毁实体替换
                        string stagereplace = "";//阶段替换
                        string entityfieldreplace = "";//实体属性替换

                        /*实体加载销毁*/
                        foreach (KeyValuePair<string, List<string>> keyValue in stage) {
                            string key = keyValue.Key;

                            if (key == "Init") {
                                foreach (string tmpInit in keyValue.Value) {
                                    string[] valStrs = tmpInit.Split("|");
                                    initloadentityreplace += $"\t\t\t{valStrs[1]} = Obj.Instance.LoadEntity(\"{valStrs[1]}\");\n";
                                    /*实体属性*/
                                    entityfieldreplace += $"\t\tprivate Entity {valStrs[1]};\n";
                                }
                            } else if (key == "Clear") {
                                foreach (string tmpInit in keyValue.Value) {
                                    string[] valStrs = tmpInit.Split("|");
                                    unloadentityreplace += $"\t\t\tObj.Instance.UnLoadEntity({valStrs[1]});\n";
                                }
                            } else {
                                /*创建阶段方法开头*/
                                string methodName = keyValue.Value[0].Split("|")[2];
                                stagereplace += string.Concat("\t\t/*", methodName ,"*/\n\t\tpublic void ", key, "() {\n");

                                /*阶段创建数据*/
                                foreach (string tmpInit in keyValue.Value) {
                                    string[] valStrs = tmpInit.Split("|");
                                    if (valStrs[0] == "load_entity") {
                                        stagereplace += $"\t\t\t{valStrs[1]} = Obj.Instance.LoadEntity(\"{valStrs[1]}\");\n";
                                        /*实体属性*/
                                        entityfieldreplace += $"\t\tprivate Entity {valStrs[1]};\n";
                                    } else if (valStrs[0] == "unload_entity") {
                                        stagereplace += $"\t\t\tObj.Instance.UnLoadEntity({valStrs[1]});\n";
                                    }
                                }

                                /*创建阶段方法结尾*/
                                stagereplace += "\t\t}\n\n";
                            }
                        }

                        log = Regex.Replace(log, "#界面属性#", uifieldreplace);
                        log = Regex.Replace(log, "#实体属性#", entityfieldreplace);

                        log = Regex.Replace(log, "#初始化创建实体#", initloadentityreplace);
                        log = Regex.Replace(log, "#清除销毁实体#", unloadentityreplace);
                        log = Regex.Replace(log, "#创建界面#", openuireplace);
                        log = Regex.Replace(log, "#销毁界面#", closeuireplace);

                        log = Regex.Replace(log, "#获取界面#", getuireplace);
                        log = Regex.Replace(log, "#流程阶段#", stagereplace);

                        /*创建目录*/
                        string path = Path.Combine(Application.dataPath, $"LazyPan/Scripts/GamePlay/Flow/{curFlowSign}");
                        if (!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                        }

                        /*创建脚本*/
                        var createPath = $"{"Assets/LazyPan/Scripts/GamePlay/Flow/"}{curFlowSign}{"/"}{"Flow_"}{curFlowSign}.cs";
                        var streamWriter = new StreamWriter(createPath, false, new UTF8Encoding(true, false));
                        streamWriter.Write(log);
                        streamWriter.Close();
                        AssetDatabase.ImportAsset(createPath);

                        /*数据销毁*/
                        stage.Clear();
                        openui.Clear();
                        closeui.Clear();
                    }
                }
            }

            return true;
        }

        public static void GenerateBehaviour(bool isTemplate) {
            /*读取配置*/
            ReadCSV.Instance.Read("BehaviourGenerate", out string content, out string[] lines);
            LogUtil.Log(content);

            List<string> namespaces = new List<string>();
            Dictionary<string, List<string>> customer = new Dictionary<string, List<string>>();

            /*生成行为脚本*/
            for (int i = 0; i < lines.Length; i++) {
                //遍历每一行数据
                if (i > 2) {
                    //遍历第三行到最后一行
                    /*类 找到第一个未创建的代码标识*/
                    string[] strs = lines[i].Split(',');
                    /*缓存当前标识*/
                    string curBehaviourSign = strs[0];
                    string curBehaviourType = strs[2];
                    string curMethodCode = strs[3];
                    /*缓存内容*/
                    if (curMethodCode == "Namespace") {
                        namespaces.Add(strs[4]);
                    } else if (curMethodCode == "Init") {

                    } else if (curMethodCode == "Clear") {

                    } else {
                        string method = curMethodCode;
                        string methodDescription = strs[4];
                        string methodKey = string.Concat(method, "|", methodDescription);
                        if (customer.ContainsKey(methodKey)) {//自定义存储了该方法
                            customer[methodKey].Add(curMethodCode);
                        } else {
                            List<string> frags = new List<string>();
                            frags.Add(curMethodCode);
                            customer.Add(methodKey, frags);
                        }
                    }
                    if (i + 1 == lines.Length || (i + 1 != lines.Length && lines[i + 1].Split(',')[0] != curBehaviourSign)) {
                        LogUtil.Log("生成脚本");
                        string type = strs[2];
                        string inputPath = $"Assets/LazyPan/Bundles/Configs/Txt/GenerateBehaviourTemplate.txt";
                        var streamReader = new StreamReader(inputPath);
                        var log = streamReader.ReadToEnd();
                        streamReader.Close();

                        string curNamespace = "";
                        foreach (string n in namespaces) {
                            curNamespace += $"{n}\n";
                        }
                        log = Regex.Replace(log, "#命名空间#", curNamespace);
                        log = Regex.Replace(log, "#行为类型#", curBehaviourType);
                        log = Regex.Replace(log, "#行为标识#", isTemplate ? string.Concat(curBehaviourSign, "_Template") : curBehaviourSign);

                        string customerMethod = "";

                        foreach (KeyValuePair<string, List<string>> temp in customer) {
                            /*方法开头*/
                            string[] methodKeyStr = temp.Key.Split("|");
                            customerMethod += string.Concat("\t\t/*", methodKeyStr[1] ,"*/\n\t\tprivate void ", methodKeyStr[0], "() {\n");

                            /*方法结尾*/
                            customerMethod += "\t\t}\n\n";
                        }
                        log = Regex.Replace(log, "#自定义方法#", customerMethod);

                        /*创建脚本*/
                        string createPath = "";
                        if (isTemplate) {
                            createPath = $"Assets/LazyPan/Scripts/GamePlay/Behaviour/Template/Behaviour_{type}_{string.Concat(curBehaviourSign, "_Template")}.cs";
                        } else {
                            createPath = $"Assets/LazyPan/Scripts/GamePlay/Behaviour/Behaviour_{type}_{curBehaviourSign}.cs";
                        }
                        var streamWriter = new StreamWriter(createPath, false, new UTF8Encoding(true, false));
                        streamWriter.Write(log);
                        streamWriter.Close();
                        AssetDatabase.ImportAsset(createPath);

                        customer.Clear();
                        namespaces.Clear();
                    }
                }
            }
        }

        private void OnGUI() {
            if (focusedWindow.titleContent.text == "快速生成行为脚本") {
                behaviourName = EditorGUILayout.TextField("输入行为配置名 ：", behaviourName);
                if (GUILayout.Button("生成行为脚本")) {
                    GenerateScript("Assets/LazyPan/Bundles/Configs/Txt/GenerateBehaviourTemplate.txt",
                        "Assets/LazyPan/Scripts/GamePlay/Behaviour/", behaviourName, "Behaviour_", "");
                }
            } else if (focusedWindow.titleContent.text == "快速生成游戏框架根据流程配置") {
                if (GUILayout.Button("生成")) {
                    GenerateFlow();
                }
            } else if (focusedWindow.titleContent.text == "快速生成游戏行为根据行为配置") {
                if (GUILayout.Button("生成")) {
                    GenerateBehaviour(true);
                }
            }
        }
    }
#endif
}