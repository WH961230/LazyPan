using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_FollowPlayer : Behaviour {
        public Behaviour_Auto_FollowPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            
        }



        public override void Clear() {
            base.Clear();
        }
    }
}