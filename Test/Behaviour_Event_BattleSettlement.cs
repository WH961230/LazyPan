using TMPro;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Event_BattleSettlement : Behaviour {
        private Flow_Battle _flowBattle;
        private Comp _settlementComp;
        private TextMeshProUGUI _scoreText;
        private Button _backBtn;

        public Behaviour_Event_BattleSettlement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowBattle);
            _settlementComp = Cond.Instance.Get<Comp>(_flowBattle.GetUI(), Label.SETTLEMENT);
            _settlementComp.gameObject.SetActive(true);

            _scoreText = Cond.Instance.Get<TextMeshProUGUI>(_settlementComp, Label.SCORE);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), Label.SCORE, out IntData scoreData);
            _scoreText.text = scoreData.Int.ToString();

            _backBtn = Cond.Instance.Get<Button>(_settlementComp, Label.BACK);
            ButtonRegister.RemoveAllListener(_backBtn);
            ButtonRegister.AddListener(_backBtn, () => {
                _flowBattle.Next("Begin");
            });
        }

        public override void DelayedExecute() {
            
        }

        public override void Clear() {
            base.Clear();
        }
    }
}