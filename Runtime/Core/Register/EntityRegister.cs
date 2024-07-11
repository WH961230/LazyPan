using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class EntityRegister {
        public static int EntityID;//为实体分配的ID
        public static Dictionary<int, Entity> EntityDic = new Dictionary<int, Entity>();

        #region 增删改查

        //增
        public static bool AddEntity(int id, Entity entity) {
            if (EntityDic.TryAdd(id, entity)) {
                ConsoleEx.Instance.ContentSave("entity", $"ID:{id} 注册实体:{entity.ObjConfig.Name}");
                return true;
            }

            return false;
        }

        //删
        public static void RemoveEntity(int id) {
            if (EntityDic.ContainsKey(id)) {
                ConsoleEx.Instance.ContentSave("entity", $"ID:{id} 移除实体:{EntityDic[id].ObjConfig.Name}");
                EntityDic.Remove(id);
            }
        }
        
        //查ID
        public static bool TryGetEntityByID(int id, out Entity entity) {
            if (EntityDic.TryGetValue(id, out entity)) {
                return true;
            }

            return false;
        }
        
        //查标识
        public static bool TryGetEntityBySign(string objSign, out Entity entity) {
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (objSign == tmpEntity.ObjConfig.Sign) {
                    entity = tmpEntity;
                    return true;
                }
            }

            entity = default;
            return false;
        }
        
        //查类型
        public static bool TryGetEntitiesByType(string type, out List<Entity> entity) {
            entity = new List<Entity>();
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (type == tmpEntity.Type) {
                    entity.Add(tmpEntity);
                }
            }

            if (entity.Count > 0) {
                return true;
            }

            entity = default;
            return false;
        }

        //查组件
        public static bool TryGetEntityByComp(Comp comp, out Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity.Comp == comp) {
                    entity = tempEntity;
                    return true;
                }
            }

            entity = null;
            return false;
        }
        
        //查BodyInstanceID
        public static bool TryGetEntityByBodyPrefabID(int id, out Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                Transform bodyTran = Cond.Instance.Get<Transform>(tempEntity, Label.BODY);
                if (bodyTran != null && bodyTran.gameObject != null && bodyTran.gameObject.GetInstanceID() == id) {
                    entity = tempEntity;
                    return true;
                }
            }

            entity = null;
            return false;
        }
        
        //查某类型的随机一个实体
        public static bool TryGetRandEntityByType(string type, out Entity entity) {
            bool findTypeEntities = TryGetEntitiesByType(type, out List<Entity> entities);
            if (!findTypeEntities) {
                entity = default;
                return false;
            }

            entity = GetRandEntity(entities);
            return true;
        }
        
        //查距离内的所有指定类型的实体
        public static bool TryGetEntitiesWithinDistance(string type, Vector3 fromPoint, float distance, out List<Entity> entity) {
            entity = new List<Entity>();
            if (TryGetEntitiesByType(type, out List<Entity> tmpEntities)) {
                foreach (Entity tmpEntity in tmpEntities) {
                    float disSqrt = distance * distance;
                    Transform body = Cond.Instance.Get<Transform>(tmpEntity, Label.BODY);
                    if (body != null) {
                        if ((body.position - fromPoint).sqrMagnitude < disSqrt) {
                            entity.Add(tmpEntity);
                        }
                    }
                }
            }

            if (entity.Count > 0) {
                return true;
            }

            return false;
        }

        //查实体列表内获取随机实体
        private static Entity GetRandEntity(List<Entity> entities) {
            return entities[Random.Range(0, entities.Count)];
        }

        //是否有该实体
        public static bool HasEntity(Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity == entity) {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}