using System;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class InputRegister : Singleton<InputRegister> {
        public static string Shift = "Player/Shift";
        public static string Motion = "Player/Motion";
        public static string LeftClick = "Player/MouseLeft";
        public static string R = "Player/R";
        public static string MouseRightPress = "Player/MouseRightPress";
        public static string Console = "Global/Console";
        public static string Enter = "Global/Enter";
        public static string ESCAPE = "Global/Escape";
        public static string Tab = "Global/Tab";
        public static string C = "Global/C";
        public static string RightClick = "Global/RightClick";
        public static string Space = "Global/Space";
        private LazyPanInputControl inputControl;

        public void Load(string actionName, Action<InputAction.CallbackContext> action) {
            if (inputControl == null) { 
                inputControl = new LazyPanInputControl();
            }
            inputControl.Enable();
            inputControl.FindAction(actionName).started += action;
            inputControl.FindAction(actionName).performed += action;
            inputControl.FindAction(actionName).canceled += action;
        }

        public void UnLoad(string actionName, Action<InputAction.CallbackContext> action) {
            if (inputControl == null) { 
                inputControl = new LazyPanInputControl();
            }
            inputControl.Enable();
            inputControl.FindAction(actionName).started -= action;
            inputControl.FindAction(actionName).performed -= action;
            inputControl.FindAction(actionName).canceled -= action;
        }

        public void Dispose(string actionName) {
            if (inputControl == null) { 
                inputControl = new LazyPanInputControl();
            }
            inputControl.Enable();
            inputControl.FindAction(actionName).Dispose();
        }

        public void Dispose() {
            if (inputControl == null) {
                inputControl = new LazyPanInputControl();
            }
            inputControl.Enable();
            inputControl.Dispose();
        }
    }
}