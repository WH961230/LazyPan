using System;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class InputRegister : Singleton<InputRegister> {
        public static string Shift = "Player/Shift";
        public static string Motion = "Player/Motion";
        public static string LeftClick = "Global/MouseLeft";
        public static string R = "Player/R";
        public static string MouseRightPress = "Player/MouseRightPress";
        public static string Console = "Global/Console";
        public static string Enter = "Global/Enter";
        public static string ESCAPE = "Global/Escape";
        public static string Tab = "Global/Tab";
        public static string C = "Global/C";
        public static string RightClick = "Global/RightClick";
        public static string Space = "Global/Space";
        private InputActionAsset inputActionAsset;
        private string inputControlKey = "LazyPanInputControl";

        public void Load(string actionName, Action<InputAction.CallbackContext> action) {
            if (inputActionAsset == null) { 
                inputActionAsset = Loader.LoadAsset<InputActionAsset>(AssetType.INPUTACTIONASSET, inputControlKey);
            }
            inputActionAsset.Enable();
            inputActionAsset.FindAction(actionName).started += action;
            inputActionAsset.FindAction(actionName).performed += action;
            inputActionAsset.FindAction(actionName).canceled += action;
        }

        public void UnLoad(string actionName, Action<InputAction.CallbackContext> action) {
            if (inputActionAsset == null) { 
                inputActionAsset = Loader.LoadAsset<InputActionAsset>(AssetType.INPUTACTIONASSET, inputControlKey);
            }
            inputActionAsset.Enable();
            inputActionAsset.FindAction(actionName).started -= action;
            inputActionAsset.FindAction(actionName).performed -= action;
            inputActionAsset.FindAction(actionName).canceled -= action;
        }

        public void Dispose(string actionName) {
            if (inputActionAsset != null) { 
                inputActionAsset.FindAction(actionName).Dispose();
            }
        }

        public void Dispose() {
            if (inputActionAsset != null) {
                inputActionAsset.Disable();
            }
        }
    }
}