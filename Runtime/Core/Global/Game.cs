using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public class Game : MonoBehaviour {
        public static Game instance;
        public UnityEvent OnUpdateEvent = new UnityEvent();
        public UnityEvent OnFixedUpdateEvent = new UnityEvent();
        public UnityEvent OnLateUpdateEvent = new UnityEvent();

        public void Init() {
            instance = this;
            Obj.Instance.Preload();
            UI.Instance.Preload();
            Flo.Instance.Preload();
        }

        private void Update() { OnUpdateEvent.Invoke(); }

        private void FixedUpdate() { OnFixedUpdateEvent.Invoke(); }

        private void LateUpdate() { OnLateUpdateEvent.Invoke(); }
    }
}