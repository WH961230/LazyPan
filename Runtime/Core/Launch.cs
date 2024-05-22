using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class Launch : MonoBehaviour {
        public static Launch instance;
        public bool OpenConsole;
        private void Awake() {
            if (instance == null) {
                instance = this;
                Config.Instance.Init(); 
                Obj.Instance.Init();

                Data.Instance.UIDontDestroyRoot = Loader.LoadGo("加载画布", "Global/Global_Loading_UIRoot", null, true).transform;
                Data.Instance.UIDontDestroyRoot.gameObject.AddComponent<Stage>();
                Data.Instance.UIDontDestroyRoot.gameObject.GetComponent<Canvas>().sortingOrder = 1;
                DontDestroyOnLoad(Data.Instance.UIDontDestroyRoot.gameObject);

                DontDestroyOnLoad(gameObject);

                ConsoleEx.Instance.Init(OpenConsole);
                    
                StageLoad("Begin");
            }
        }

        //加载阶段
        public void StageLoad(string sceneName) {
            Data.Instance.OnUpdateEvent.RemoveAllListeners();
            Data.Instance.OnFixedUpdateEvent.RemoveAllListeners();
            Data.Instance.OnLateUpdateEvent.RemoveAllListeners();

            Stage stage = Data.Instance.UIDontDestroyRoot.gameObject.GetComponent<Stage>();
            stage.Load(SceneConfig.Get(sceneName).DelayTime, sceneName);
        }

        //结束游戏
        public void QuitGame() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}