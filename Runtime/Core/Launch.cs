using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class Launch : MonoBehaviour {
        public static Launch instance;
        public bool OpenConsole;
        public string StageLoadScene;
        [HideInInspector] public Transform UIDontDestroyRoot;
        private void Awake() {
            if (instance == null) {
                instance = this;
                Config.Instance.Init(); 
                Obj.Instance.Init();

                UIDontDestroyRoot = Loader.LoadGo("加载画布", "Global/Global_Loading_UIRoot", null, true).transform;
                UIDontDestroyRoot.gameObject.AddComponent<Stage>();
                UIDontDestroyRoot.gameObject.GetComponent<Canvas>().sortingOrder = 1;
                DontDestroyOnLoad(UIDontDestroyRoot.gameObject);

                DontDestroyOnLoad(gameObject);

                ConsoleEx.Instance.Init(OpenConsole);
                    
                StageLoad(StageLoadScene);
            }
        }

        //加载阶段
        public void StageLoad(string sceneName) {
            Stage stage = UIDontDestroyRoot.gameObject.GetComponent<Stage>();
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