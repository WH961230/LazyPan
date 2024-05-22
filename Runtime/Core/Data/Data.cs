using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public partial class Data : Singleton<Data> {
        public bool FirstPlay;//首次游玩
        public Transform UIDontDestroyRoot;
        public Transform UIRoot;//根节点
        public Transform ObjRoot;//根节点

        public UnityEvent OnUpdateEvent = new UnityEvent();
        public UnityEvent OnFixedUpdateEvent = new UnityEvent();
        public UnityEvent OnLateUpdateEvent = new UnityEvent();

        public int EntityID;//为实体分配的ID
        public Dictionary<int, Entity> EntityDic = new Dictionary<int, Entity>();
        public Dictionary<int, List<Behaviour>> BehaviourDic = new Dictionary<int, List<Behaviour>>();

        public bool AddEntity(int id, Entity entity) {
            if (EntityDic.TryAdd(id, entity)) {
                ConsoleEx.Instance.ContentSave("entity", $"ID:{id} 注册实体:{entity.ObjConfig.Name}");
                return true;
            }

            return false;
        }

        public void RemoveEntity(int id) {
            if (EntityDic.ContainsKey(id)) {
                ConsoleEx.Instance.ContentSave("entity", $"ID:{id} 移除实体:{EntityDic[id].ObjConfig.Name}");
                EntityDic.Remove(id);
            }
        }

        //通过 标识 查找实体
        public bool TryGetEntityBySign(string objSign, out Entity entity) {
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (objSign == tmpEntity.ObjConfig.Sign) {
                    entity = tmpEntity;
                    return true;
                }
            }

            entity = default;
            return false;
        }

        //通过 ID 查找实体
        public bool TryGetEntityByID(int id, out Entity entity) {
            if (EntityDic.TryGetValue(id, out entity)) {
                return true;
            }

            return false;
        }

        //通过 类型 查找实体
        public bool TryGetEntityByType(string type, out Entity entity) {
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (tmpEntity.EntityData.BaseRuntimeData != null && type == tmpEntity.EntityData.BaseRuntimeData.Type) {
                    entity = tmpEntity;
                    return true;
                }
            }

            entity = default;
            return false;
        }

        //通过 类型 查找所有实体
        public bool TryGetEntitiesByType(string type, out List<Entity> entity) {
            entity = new List<Entity>();
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (tmpEntity.EntityData.BaseRuntimeData != null && type == tmpEntity.EntityData.BaseRuntimeData.Type) {
                    entity.Add(tmpEntity);
                }
            }

            if (entity.Count > 0) {
                return true;
            }

            entity = default;
            return false;
        }

        //通过组件查找实体
        public bool TryGetEntityByComp(Comp comp, out Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity.Comp == comp) {
                    entity = tempEntity;
                    return true;
                }
            }

            entity = null;
            return false;
        }

        //通过组件查找实体
        public bool TryGetEntityByBodyPrefabID(int id, out Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                Transform bodyTran = Cond.Instance.Get<Transform>(tempEntity, Label.BODY);
                if (bodyTran != null && bodyTran.gameObject.GetInstanceID() == id) {
                    entity = tempEntity;
                    return true;
                }
            }

            entity = null;
            return false;
        }

        //通过实体查找实体
        public bool HasEntity(Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity == entity) {
                    return true;
                }
            }

            return false;
        }

        //获取某一类型的随机实体
        public bool TryGetRandEntityByType(string type, out Entity entity) {
            bool findTypeEntities = TryGetEntitiesByType(type, out List<Entity> entities);
            if (!findTypeEntities) {
                entity = default;
                return false;
            }

            entity = GetRandEntity(entities);
            return true;
        }

        //获取距离内的所有实体
        public bool TryGetEntitiesWithinDistance(string type, Vector3 fromPoint, float distance, out List<Entity> entity) {
            entity = new List<Entity>();
            if (TryGetEntitiesByType(type, out List<Entity> tmpEntities)) {
                foreach (Entity tmpEntity in tmpEntities) {
                    float disSqrt = distance * distance;
                    if ((Cond.Instance.Get<Transform>(tmpEntity, Label.BODY).position - fromPoint).sqrMagnitude < disSqrt) {
                        entity.Add(tmpEntity);
                    }
                }
            }

            if (entity.Count > 0) {
                return true;
            }

            return false;
        }

        //实体列表内获取随机实体
        public Entity GetRandEntity(List<Entity> entities) {
            return entities[Random.Range(0, entities.Count)];
        }
    }
}