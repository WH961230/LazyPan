using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_BirdFly : Behaviour {
        private FloatData jumpForceData;
        private FloatData velocityYData;
        public Behaviour_Input_BirdFly(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.LeftClick, InputClickEvent);
            Cond.Instance.GetData(entity, Label.Assemble(Label.JUMP, Label.FORCE), out jumpForceData);
            Cond.Instance.GetData(entity, Label.VELOCITY, out velocityYData);
        }

        private void InputClickEvent(InputAction.CallbackContext obj) {
            if (obj.started) {
                Debug.Log("点击");
                velocityYData.Float = jumpForceData.Float;
            }
        }

        public override void DelayedExecute() {
            
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.LeftClick, InputClickEvent);
        }
    }
}