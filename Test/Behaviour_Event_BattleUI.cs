using TMPro;
using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_BattleUI : Behaviour {
        private Flow_Battle _flowBattle;
        private TextMeshProUGUI _scoreText;
        private IntData _scoreData;
        private int score = -1;
        public Behaviour_Event_BattleUI(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            Flo.Instance.GetFlow(out _flowBattle);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), Label.SCORE, out _scoreData);
            _scoreText = Cond.Instance.Get<TextMeshProUGUI>(_flowBattle.GetUI(), Label.SCORE);
        }

        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            if (score != _scoreData.Int) {
                _scoreText.text = _scoreData.Int.ToString();
                score = _scoreData.Int;
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}