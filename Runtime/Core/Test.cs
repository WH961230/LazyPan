using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Test : MonoBehaviour {
        private void Start() {
            InputRegister.Instance.Load(InputRegister.Space, Debug);
        }

        private void Debug(InputAction.CallbackContext obj) {
            if (obj.performed) {
                UnityEngine.Debug.Log("测试按下空格");
            }
        }
    }
}