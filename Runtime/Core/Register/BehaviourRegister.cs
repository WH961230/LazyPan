using System;
using System.Collections.Generic;
using System.Reflection;

namespace LazyPan {
    public class BehaviourRegister {
        private static Dictionary<int, List<Behaviour>> BehaviourDic = new Dictionary<int, List<Behaviour>>();

        #region 增

        //增加注册行为
        public static bool RegisterBehaviour(int id, string sign) {
            //是否有实体
            if (EntityRegister.TryGetEntityByID(id, out Entity entity)) {
                if (BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
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
                        BehaviourDic.TryAdd(id, instanceBehaviours);
                    } catch (Exception e) {
                        LogUtil.LogError(sign);
                        throw;
                    }
                    
                    return true;
                }
            }

            return false;
        }

        //删除注册的行为
        public static bool UnRegisterBehaviour(int id, string sign) {
            int index = GetBehaviourIndex(id, sign);
            //是否有实体
            if (index != -1) {
                if (BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
                    behaviours[index].Clear();
                    behaviours.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }
        
        //删除注册的行为
        public static bool UnRegisterAllBehaviour(int id) {
            if (BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
                if (behaviours != null && behaviours.Count > 0) {
                    int index = behaviours.Count - 1;
                    for (int i = index; i >= 0; i--) {
                        UnRegisterBehaviour(id, behaviours[i].BehaviourSign);
                    }
                }
                return true;
            }

            return false;
        }

        private static int GetBehaviourIndex(int id, string sign) {
            int index = -1;
            //是否有实体
            if (BehaviourDic.TryGetValue(id, out List<Behaviour> behaviours)) {
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

        #endregion
    }
}