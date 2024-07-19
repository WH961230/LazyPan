namespace LazyPan {
    public class Behaviour {
        public string BehaviourSign;
        public Data BehaviourData;
        public Entity entity;

        protected Behaviour(Entity entity, string behaviourSign) {
            this.entity = entity;
            BehaviourSign = behaviourSign;
            ConsoleEx.Instance.ContentSave("behaviour", $"ID:{entity.ID} 注册行为:{BehaviourConfig.Get(BehaviourSign).Name}");
        }

        protected void SetBehaviourData(Data data) {
            BehaviourData = data;
        }

        public virtual void Clear() {
            ConsoleEx.Instance.ContentSave("behaviour", $"ID:{entity.ID} 注销行为:{BehaviourConfig.Get(BehaviourSign).Name}");
        }
    }
}