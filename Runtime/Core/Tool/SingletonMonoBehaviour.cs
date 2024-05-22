using UnityEngine;

namespace LazyPan {
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        static T instance = null;

        public static T Instance {
            get {
                if (instance != null) {
                    return instance;
                }

                instance = new GameObject(typeof(T).Name).AddComponent<T>();
                if (Application.isPlaying) {
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        private void Awake() {
            instance = this as T;
        }
    }
}