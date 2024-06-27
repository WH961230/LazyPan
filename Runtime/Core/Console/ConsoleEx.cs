using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class ConsoleEx : Singleton<ConsoleEx> {
        private Dictionary<int, List<string>> contentDic = new Dictionary<int, List<string>>();
        private Comp comp;
        private bool firstSendCode;

        public void Init(bool initOpen) {
#if UNITY_EDITOR
            InputRegister.Instance.Dispose(InputRegister.Console);
            InputRegister.Instance.Load(InputRegister.Console, ConsoleEvent);
            comp =
                Loader.LoadComp("控制台界面", "UI/UI_Console", Launch.instance.UIDontDestroyRoot, initOpen);
            ContentClear();
#endif
        }

        private void ConsoleEvent(InputAction.CallbackContext obj) {
            if (obj.performed) {
                bool hasDebug = comp.gameObject.activeSelf;
                if (hasDebug) {
                    comp.gameObject.SetActive(false);
                } else {
                    comp.gameObject.SetActive(true);
                    firstSendCode = true;
                    //绑定按键
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).text = "";
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).ActivateInputField();
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).onEndEdit.RemoveAllListeners();
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).onEndEdit.AddListener(SendCode);
                }
            }
        }

        //发送命令
        private void SendCode(string content) {
            if (Keyboard.current.enterKey.isPressed) {
                if (!string.IsNullOrEmpty(content)) {
                    Content("you", content);
                    CodeAction(content);
                }
                Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).ActivateInputField();
            }
        }

        //命令的触发表现
        private void CodeAction(string code) {
            if (code == "help") {
                Content("computer",
                    "code: help[帮助]\n" +
                    "clear[清空控制台]\n" +
                    "flow[流程数据]\n" +
                    "behaviour[行为数据]\n" +
                    "entity[实体数据]\n" +
                    "normal[普通数据]\n");
                return;
            }

            if (code == "clear") {
                ContentClear();
                return;
            }

            if (contentDic.TryGetValue(code.GetHashCode(), out List<string> contentVal)) {
                string originalText = Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text;
                string tempContent = "";
                foreach (string content in contentVal) {
                    string[] info = content.Split("@");
                    tempContent += string.Concat(info[0], " : ", info[1], "\n");
                }

                SetText(string.Concat(tempContent, "\n", originalText));
            }
        }

        //新增内容
        public void Content(string who, string content) {
#if UNITY_EDITOR
            string originalText = Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text;
            SetText(string.Concat(who, " : ", content, "\n", originalText));
#endif
        }

        //控制台内容存字典
        public void ContentSave(string hashKey, string content) {
            if (contentDic.TryGetValue(hashKey.GetHashCode(), out List<string> values)) {
                values.Add(string.Concat(System.DateTime.Now, "@", content));
            } else {
                List<string> contentVal = new List<string>();
                contentVal.Add(string.Concat(System.DateTime.Now, "@", content));
                contentDic.Add(hashKey.GetHashCode(), contentVal);
            }
        }

        //清空内容
        private void ContentClear() {
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text = null;
        }

        //设置输入内容
        private void SetText(string content) {
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text = content;
        }
    }
}