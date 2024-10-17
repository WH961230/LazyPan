using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_Gravity : Behaviour {
        private Transform bodyTran;
        private FloatData velocityYData;
        private FloatData gravityRatioData;
        private RectTransform rectBodyTran;

        public Behaviour_Auto_Gravity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            bodyTran = Cond.Instance.Get<Transform>(entity, Label.BODY);
            rectBodyTran = bodyTran.GetComponent<RectTransform>();
            Cond.Instance.GetData(entity, Label.GRAVITY, out gravityRatioData);
            Cond.Instance.GetData(entity, Label.VELOCITY, out velocityYData);
            Game.instance.OnUpdateEvent.AddListener(OnGravityUpdate);
        }

        private void OnGravityUpdate() {
            if (rectBodyTran != null) {
                velocityYData.Float -= gravityRatioData.Float * Time.deltaTime;
                rectBodyTran.position += new Vector3(0, velocityYData.Float * Time.deltaTime, 0);
            }
        }

        public override void DelayedExecute() {
            
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnGravityUpdate);
        }
    }
}