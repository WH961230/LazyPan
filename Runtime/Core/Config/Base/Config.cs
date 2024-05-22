namespace LazyPan {
    public class Config : Singleton<Config> {
        public void Init() {
            BehaviourConfig.Init();
            SceneConfig.Init();
            ObjConfig.Init();
            UIConfig.Init();
        }
    }
}