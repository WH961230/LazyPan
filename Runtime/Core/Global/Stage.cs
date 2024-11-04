using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LazyPan {
    public class Stage : MonoBehaviour {
        public Comp loadingUIComp; //加载界面
        private StageWork work; //当前作业
        private Queue<StageWork> works = new Queue<StageWork>(); //所有作业

        public void Load(float delayTime, string sceneName) {
            works.Clear();
            if (delayTime != 0) {
                works.Enqueue(new LoadLoadingUI(new LoadLoadingUIParameters() {
                    uiRoot = transform
                }, this));
            }

            works.Enqueue(new LoadScene(new LoadSceneParameters() {
                sceneName = sceneName
            }));
            works.Enqueue(new LoadGlobal(new LoadGlobalParameters() {
                sceneName = sceneName, delayTime = delayTime
            }, this));
        }

        public void Update() {
            //获取当前作业
            if (work == null && works.Count > 0) {
                work = works.Dequeue();
                work?.Start();
            }

            work?.Update();
            if (work != null) {
                if (work.IsDone) {
                    work.Complete();
                    work = null;
                }
            }
        }
    }

    public class LoadGlobalParameters : StageParameters {
        public string sceneName;
        public float delayTime;
        public float progress;
    }

    public class LoadGlobal : StageWork {
        private LoadGlobalParameters Parameters;
        private Stage stage;
        private Game game;
        private Clock clock;
        private float delayDeployTime;

        public LoadGlobal(StageParameters Parameters, Stage stage) : base(Parameters) {
            this.Parameters = (LoadGlobalParameters)Parameters;
            this.stage = stage;
        }

        public override void Start() {
            Parameters.progress = 0;
            delayDeployTime = 0;
        }

        public override void Update() {
            if (SceneManager.GetActiveScene().name == Parameters.sceneName && game == null) {
                game = Loader.LoadGo("全局", "Global/Global", null, true).GetComponent<Game>();
                game.Init();
                clock = ClockUtil.Instance.AlarmAfter(Parameters.delayTime, () => { Parameters.progress = 1f; });
            }

            if (game != null) {
                if (Parameters.progress == 1) {
                    IsDone = true;
                    ClockUtil.Instance.Stop(clock);
                }

                LoadingUI(stage.loadingUIComp, "", Parameters.progress);
            }
        }

        private void LoadingUI(Comp loadingUIComp, string description, float progress) {
            if (loadingUIComp) {
                Slider loadingSlider = Cond.Instance.Get<Slider>(loadingUIComp, "LoadingSlider");
                TextMeshProUGUI loadingText = Cond.Instance.Get<TextMeshProUGUI>(loadingUIComp, "LoadingText");
                if (delayDeployTime < Parameters.delayTime) {
                    delayDeployTime += 1 * Time.deltaTime / Parameters.delayTime;
                    progress = delayDeployTime;
                }

                loadingSlider.value = progress;
                loadingText.text = string.Concat(description, " ", Mathf.Round(loadingSlider.value * 100f), "%");
            }
        }

        public override void Complete() {
            if (stage.loadingUIComp != null) {
                Object.DestroyImmediate(stage.loadingUIComp.gameObject);
            }
        }
    }

    public class LoadSceneParameters : StageParameters {
        public string sceneName;
    }

    public class LoadScene : StageWork {
        LoadSceneParameters Parameters;
        private AsyncOperation operation;

        public LoadScene(StageParameters Parameters) : base(Parameters) {
            this.Parameters = (LoadSceneParameters)Parameters;
            operation = Loader.LoadSceneAsync(this.Parameters.sceneName);
            operation.allowSceneActivation = false;
        }

        public override void Start() {
        }

        public override void Update() {
            if (operation != null && !IsDone) {
                if (operation.progress >= 0.9f) {
                    operation.allowSceneActivation = true;
                    IsDone = true;
                }
            }
        }

        public override void Complete() {
        }
    }

    public class LoadLoadingUIParameters : StageParameters {
        public Transform uiRoot;
    }

    public class LoadLoadingUI : StageWork {
        LoadLoadingUIParameters Parameters;
        Stage stage;

        public LoadLoadingUI(StageParameters Parameters, Stage stage) : base(Parameters) {
            this.Parameters = (LoadLoadingUIParameters)Parameters;
            this.stage = stage;
        }

        public override void Start() {
            stage.loadingUIComp = Loader.LoadComp("加载界面", "UI/UI_Loading", Parameters.uiRoot, true);
        }

        public override void Update() {
            if (stage.loadingUIComp != null) {
                stage.loadingUIComp.gameObject.SetActive(true);
                IsDone = true;
            }
        }

        public override void Complete() {
        }
    }

    public class StageParameters {
        public string Description;
    }

    public abstract class StageWork {
        public bool IsDone;
        public StageParameters Parameters;

        public StageWork(StageParameters Parameters) {
            this.Parameters = Parameters;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void Complete();
    }
}