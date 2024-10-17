using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace LazyPan {
    public class Behaviour_Event_BeginUI : Behaviour {
        private Flow_Begin flow;

        public Behaviour_Event_BeginUI(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out flow);
            Button startGameBtn = Cond.Instance.Get<Button>(flow.GetUI(), Label.Assemble(Label.START, Label.GAME));
            ButtonRegister.RemoveAllListener(startGameBtn);
            ButtonRegister.AddListener(startGameBtn, Next);

            InputRegister.Instance.UnLoad(InputRegister.Space, InputNext);
            InputRegister.Instance.Load(InputRegister.Space, InputNext);
        }

        public override void DelayedExecute() {
            
        }
        
        private void InputNext(InputAction.CallbackContext obj) {
            if (obj.started) {
                Next();
            }
        }

        private void Next() {
            flow.Next("Battle");
        } 

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Space, InputNext);
        }
    }
}