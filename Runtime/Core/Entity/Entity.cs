using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LazyPan {
    [Serializable]
    public class Entity {
        public int ID;//身份ID
        public string Sign;//标识
        public string Type;//类型

        public GameObject Prefab;//物体
        public Comp Comp;//组件
        public Data Data;//数据
        public ObjConfig ObjConfig;//配置

        public void Init(string sign) {
            //设置ID
            ID = ++EntityRegister.EntityID;
            //设置配置
            ObjConfig objConfig = ObjConfig.Get(sign);
            ObjConfig = objConfig;
            //设置标识
            Sign = objConfig.Sign;
            //设置类型
            Type = objConfig.Type;
            //获取对象池的物体 如数量不足 对象池预加载
            Prefab = Loader.LoadGo(null,
                string.Concat(SceneConfig.Get(SceneManager.GetActiveScene().name).DirPath, objConfig.Sign),
                Obj.Instance.ObjRoot, true);
            //设置Comp
            Comp = Prefab.GetComponent<Comp>();
            //设置Data
            Data = Prefab.GetComponent<Data>();
            //物体名赋值
            Prefab.name = string.Concat("[", ID, "]", objConfig.Name);
            //读取配置位置初始化
            if (!string.IsNullOrEmpty(objConfig.SetUpLocationInformationSign)) {
                LocationInformationSetting setting = Loader.LoadLocationInfSetting(objConfig.SetUpLocationInformationSign);
                LocationInformationData data = null;
                if (setting.locationInformationDatas.Count > 1) {
                    data = setting.locationInformationDatas[UnityEngine.Random.Range(0, setting.locationInformationDatas.Count)];
                } else if (setting.locationInformationDatas.Count == 1) {
                    data = setting.locationInformationDatas[0];
                } else {
                    LogUtil.LogErrorFormat("错误！位置配置{0}信息数据为空！", setting.name);
#if UNITY_EDITOR
                    EditorApplication.isPaused = true;
#endif
                }

                SetBeginLocationInfo(data);
            }
            //注册实体
            EntityRegister.AddEntity(ID, this);
            //注册配置行为
            if (!string.IsNullOrEmpty(objConfig.SetUpBehaviourSign)) {
                string[] behaviourArray = objConfig.SetUpBehaviourSign.Split("|");
                for (int i = 0; i < behaviourArray.Length; i++) {
                    BehaviourRegister.RegisterBehaviour(ID, behaviourArray[i], out Behaviour outBehaviour);
                }
            }
        }

        public void SetBeginLocationInfo(LocationInformationData data) {
            CharacterController characterController = Cond.Instance.Get<CharacterController>(this, Label.CHARACTERCONTROLLER);
            if (characterController != null) {
                characterController.enabled = false;
            }

            if (Cond.Instance.Get<Transform>(this, Label.FOOT) != null) {
                Cond.Instance.Get<Transform>(this, Label.FOOT).position = data.Position;
                Cond.Instance.Get<Transform>(this, Label.FOOT).rotation = Quaternion.Euler(data.Rotation);
            }

            characterController = Cond.Instance.Get<CharacterController>(this, Label.CHARACTERCONTROLLER);
            if (characterController != null) {
                characterController.enabled = true;
            }
        }

        public void Clear() {
            //注销行为
            BehaviourRegister.UnRegisterAllBehaviour(ID);
            //注销实体
            EntityRegister.RemoveEntity(ID);
            //物体销毁
            Object.Destroy(Prefab);
            Prefab = null;
            //销毁配置
            ObjConfig = null;
            //ID重置
            ID = 0;
        }
    }
}