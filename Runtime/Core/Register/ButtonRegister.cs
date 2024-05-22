using UnityEngine.Events;
using UnityEngine.UI;

namespace LazyPan {
    public class ButtonRegister {
        public static void AddListener(Button button, UnityAction unityAction) {
            button.onClick.AddListener(unityAction);
        }

        public static void AddListener<T>(Button button, UnityAction<T> unityAction, T t) {
            button.onClick.AddListener(() => unityAction(t));
        }

        public static void AddListener<T1, T2>(Button button, UnityAction<T1, T2> unityAction, T1 t1, T2 t2) {
            button.onClick.AddListener(() => unityAction(t1, t2));
        }
        
        public static void AddListener<T1, T2, T3>(Button button, UnityAction<T1, T2, T3> unityAction, T1 t1, T2 t2, T3 t3) {
            button.onClick.AddListener(() => unityAction(t1, t2, t3));
        }

        public static void RemoveListener(Button button, UnityAction unityAction) {
            button.onClick.RemoveListener(unityAction);
        }

        public static void RemoveListener<T>(Button button, UnityAction<T> unityAction, T t) {
            button.onClick.RemoveListener(() => unityAction(t));
        }

        public static void RemoveListener<T1, T2>(Button button, UnityAction<T1, T2> unityAction, T1 t1, T2 t2) {
            button.onClick.RemoveListener(() => unityAction(t1, t2));
        }

        public static void RemoveListener<T1, T2, T3>(Button button, UnityAction<T1, T2, T3> unityAction, T1 t1, T2 t2, T3 t3) {
            button.onClick.RemoveListener(() => unityAction(t1, t2, t3));
        }

        public static void RemoveAllListener(Button button) {
            button.onClick.RemoveAllListeners();
        }
    }
}