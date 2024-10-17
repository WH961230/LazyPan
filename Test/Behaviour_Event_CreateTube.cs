using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_CreateTube : Behaviour {
        private bool isSettlement;

        public float spawnInterval = 1.5f; // 生成间隔
        public float tubeSpeed = 400f; // 烟囱移动速度
        private float timer;

        private RectTransform _rootRect;
        private StringData signData;
        public List<Entity> tubes = new List<Entity>();

        public Behaviour_Event_CreateTube(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, Label.SIGN, out signData);
            Game.instance.OnUpdateEvent.AddListener(OnTubeUpdate);
        }

        private void OnTubeUpdate() {
            timer += Time.deltaTime;
            if (timer >= spawnInterval) {
                SpawnTube();
                timer = 0f;
            }

            MoveTube();
        }

        private void SpawnTube() {
            Entity tubeEntity = Obj.Instance.LoadEntity(signData.String);
            _rootRect = Cond.Instance.Get<Transform>(tubeEntity, Label.ROOT).GetComponent<RectTransform>();
            _rootRect.localPosition = new Vector3(_rootRect.localPosition.x, Random.Range(-266f, 266f), _rootRect.localPosition.z);
            tubes.Add(tubeEntity);
        }

        void MoveTube() {
            List<int> indicesToRemove = new List<int>();

            for (int i = 0; i < tubes.Count; i++) {
                _rootRect = Cond.Instance.Get<Transform>(tubes[i], Label.ROOT).GetComponent<RectTransform>();
                if (_rootRect.position.x >= 0) {
                    _rootRect.Translate(Vector2.left * tubeSpeed * Time.deltaTime);
                } else {
                    Obj.Instance.UnLoadEntity(tubes[i]);
                    indicesToRemove.Add(i);
                }
            }

            // 从后往前删除元素，避免索引问题
            for (int i = indicesToRemove.Count - 1; i >= 0; i--) {
                tubes.RemoveAt(indicesToRemove[i]);
            }
        }

        public override void DelayedExecute() {
            
        }

        public override void Clear() {
            base.Clear();
            for (int i = 0; i < tubes.Count; i++) {
                Obj.Instance.UnLoadEntity(tubes[i]);
            }
            tubes.Clear();

            Game.instance.OnUpdateEvent.RemoveListener(OnTubeUpdate);
        }
    }
}