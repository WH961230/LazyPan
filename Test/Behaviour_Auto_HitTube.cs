using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_HitTube : Behaviour {
        private Flow_Battle _flowBattle;

        public Behaviour_Auto_HitTube(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowBattle);
            for (int i = 0; i < entity.Comp.Comps.Count; i++) {
                Comp.CompData compComp = entity.Comp.Comps[i];
                if (compComp.Sign == "Trigger") {
                    compComp.Comp.OnTriggerEnterEvent2D.AddListener(OnTriggerEnter2D);
                } else if(compComp.Sign == "PassTrigger") {
                    compComp.Comp.OnTriggerEnterEvent2D.AddListener(OnPassTriggerEnter2D);
                }
            }
        }

        private void OnPassTriggerEnter2D(Collider2D arg0) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), Label.SCORE, out IntData scoreData);
            scoreData.Int += 1;
        }

        private void OnTriggerEnter2D(Collider2D arg0) {
            _flowBattle.Settlement();
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            for (int i = 0; i < entity.Comp.Comps.Count; i++) {
                entity.Comp.Comps[i].Comp.OnTriggerEnterEvent2D.RemoveListener(OnTriggerEnter2D);
            }
        }
    }
}