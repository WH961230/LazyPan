using System;
using System.Collections.Generic;
using System.Reflection;

namespace LazyPan {
    public class BehaviourRegister : Singleton<BehaviourRegister> {
        //实体注册行为
        public bool RegisterBehaviour(int id, string sign) {
            //是否有实体
            if (Data.Instance.TryGetEntityByID(id, out Entity entity)) {
                if (Data.Instance.BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
                    //判断实体已有当前行为
                    foreach (Behaviour tempBehaviour in behaviours) {
                        if (tempBehaviour.BehaviourSign == sign) {
                            return false;
                        }
                    }

                    //创建行为实体
                    try {
                        Type type = Assembly.Load("Assembly-CSharp").GetType(string.Concat("LazyPan.", sign));
                        Behaviour behaviour = (Behaviour) Activator.CreateInstance(type, entity, sign);
                        behaviours.Add(behaviour);
                    } catch (Exception e) {
                        LogUtil.LogError(sign);
                        throw;
                    }

                    return true;
                } else {
                    //创建行为实体
                    try {
                        List<Behaviour> instanceBehaviours = new List<Behaviour>();
                        Type type = Assembly.Load("Assembly-CSharp").GetType(string.Concat("LazyPan.", sign));
                        Behaviour behaviour = (Behaviour) Activator.CreateInstance(type, entity, sign);
                        instanceBehaviours.Add(behaviour);
                        Data.Instance.BehaviourDic.TryAdd(id, instanceBehaviours);
                    } catch (Exception e) {
                        LogUtil.LogError(sign);
                        throw;
                    }
                    
                    return true;
                }
            }

            return false;
        }

        //注销实体注册行为
        public bool UnRegisterBehaviour(int id, string sign) {
            int index = GetBehaviourIndex(id, sign);
            //是否有实体
            if (index != -1) {
                if (Data.Instance.BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
                    behaviours[index].Clear();
                    behaviours.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        private int GetBehaviourIndex(int id, string sign) {
            int index = -1;
            //是否有实体
            if (Data.Instance.BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
                //判断实体已有当前行为
                for (var i = 0; i < behaviours.Count; i++) {
                    if (behaviours[i].BehaviourSign == sign) {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }
    }
}