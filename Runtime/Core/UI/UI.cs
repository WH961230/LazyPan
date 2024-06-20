using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class UI : SingletonMonoBehaviour<UI> {
        private Comp UIComp;
        private Dictionary<string, Comp> uICompAlwaysDics = new Dictionary<string, Comp>();
        private Dictionary<string, Comp> uICompExchangeDics = new Dictionary<string, Comp>();
        private Dictionary<string, Comp> uICompDics = new Dictionary<string, Comp>();

        public void Preload() {
            Data.Instance.UIRoot = Loader.LoadGo("画布", "Global/Global_UIRoot", null, true).transform;
            List<string> keys = UIConfig.GetKeys();
            int length = keys.Count;
            uICompDics.Clear();
            uICompExchangeDics.Clear();
            uICompAlwaysDics.Clear();
            for (int i = 0; i < length; i++) {
                string key = keys[i];
                GameObject uiGo = Loader.LoadGo(UIConfig.Get(key).Description, string.Concat("UI/", key), Data.Instance.UIRoot, false);
                switch (UIConfig.Get(key).Type) {
                    case 0:
                        uICompExchangeDics.Add(key, uiGo.GetComponent<Comp>());
                        break;
                    case 1:
                        uICompAlwaysDics.Add(key, uiGo.GetComponent<Comp>());
                        break;
                }
            
                uICompDics.Add(key, uiGo.GetComponent<Comp>());
            }
        }

        public Comp Open(string name) {
            if (uICompExchangeDics.TryGetValue(name, out Comp uiExchangeComp)) {
                if (UIComp != null) {
                    UIComp.gameObject.SetActive(false);
                }

                UIComp = uiExchangeComp;
                UIComp.gameObject.SetActive(true);
                return UIComp;
            }

            if (uICompAlwaysDics.TryGetValue(name, out Comp uiAlwaysComp)) {
                uiAlwaysComp.gameObject.SetActive(true);
                return uiAlwaysComp;
            }

            return null;
        }

        public Comp Get(string name) {
            if (uICompDics.TryGetValue(name, out Comp comp)) {
                return comp;
            }

            return null;
        }

        public string GetExchangeUIName() {
            if (UIComp != null) {
                return UIComp.gameObject.name;
            }

            return null;
        }

        public bool IsAlwaysUIName(string name) {
            return uICompAlwaysDics.TryGetValue(name, out Comp comp);
        }

        public bool IsExchangeUI() {
            return UIComp != null;
        }

        public void Close() {
            Close(UIComp);
            UIComp = null;
        }

        public void Close(string name) {
            if (uICompAlwaysDics.TryGetValue(name, out Comp uiAlwaysComp)) {
                Close(uiAlwaysComp);
            }
        }

        private void Close(Comp comp) {
            if (comp != null) {
                comp.gameObject.SetActive(false);
            }
        }
    }
}