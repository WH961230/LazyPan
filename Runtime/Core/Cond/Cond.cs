﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public class Cond : Singleton<Cond> {
        #region 全局通用实体

        public Entity GetCameraEntity() {
            if (EntityRegister.TryGetRandEntityByType("Camera", out Entity entity)) {
                return entity;
            } else {
                return null;
            }
        }

        public Entity GetPlayerEntity() {
            if (EntityRegister.TryGetRandEntityByType("Player", out Entity entity)) {
                return entity;
            } else {
                return null;
            }
        }

        public bool GetEntityByID(int id, out Entity entity) {
            return EntityRegister.TryGetEntityByID(id, out entity) ;
        }

        #endregion

        #region 查标签组件
        public T Get<T>(Entity entity, string label) where T : Object {
            if (entity == null) { return default; }
#if UNITY_EDITOR
            if (entity.Comp == null) {
                LogUtil.LogErrorFormat("请检查 entity:{0} 没有挂 Comp 组件!", entity.ObjConfig.Sign);
                EditorApplication.isPaused = true;
            }
#endif
            return entity.Comp.Get<T>(label);
        }
        public T Get<T>(Comp comp, string label) where T : Object {
            if (comp == null) { return default; }
            return comp.Get<T>(label);
        }

        #endregion

        #region 查标签数据

        public bool GetData<T>(Entity entity, string label, out T t) {
            if (entity == null) {
                t = default;
                return false;
            }
#if UNITY_EDITOR
            if (entity.Data == null) {
                LogUtil.LogErrorFormat("请检查 entity:{0} 没有挂 Data 组件!", entity.ObjConfig.Sign);
                EditorApplication.isPaused = true;
            }
#endif
            return entity.Data.Get(label, out t);
        }

        public bool SetData<T>(Entity entity, string label, T t) {
            if (entity == null) {
                return false;
            }
#if UNITY_EDITOR
            if (entity.Data == null) {
                LogUtil.LogErrorFormat("请检查 entity:{0} 没有挂 Data 组件!", entity.ObjConfig.Sign);
                EditorApplication.isPaused = true;
            }
#endif
            return entity.Data.Set(label, t);
        }

        #endregion

        #region 查标签事件

        public UnityEvent GetEvent(Entity entity, string label) {
            return entity.Comp.GetEvent(label);
        }
        public UnityEvent GetEvent(Comp comp, string label) {
            return comp.GetEvent(label);
        }

        #endregion
    }
}