using System;
using System.Collections.Generic;

namespace LazyPan {
    public class MessageRegister : Singleton<MessageRegister> {
        private MsgRegister register = new MsgRegister();

        public void Reg(int id, Action a) {
            register.Register(id, a);
        }

        public void Reg<T>(int id, Action<T> a) {
            register.Register(id, a);
        }

        public void Reg<T1, T2>(int id, Action<T1, T2> a) {
            register.Register(id, a);
        }

        public void UnReg(int id, Action act) {
            register.UnRegister(id, act);
        }

        public void UnReg<T>(int id, Action<T> act) {
            register.UnRegister(id, act);
        }

        public void UnReg<T1, T2>(int id, Action<T1, T2> act) {
            register.UnRegister(id, act);
        }

        public void Dis(int id) {
            register.Dispatcher(id);
        }

        public void Dis<T>(int id, T t) {
            register.Dispatcher(id, t);
        }

        public void Dis<T1, T2>(int id, T1 t1, T2 t2) {
            register.Dispatcher(id, t1, t2);
        }
    }

    public class Body {
        public int id;
        public Delegate handler;

        public Body(int id, Delegate handler) {
            this.id = id;
            this.handler = handler;
        }

        public void Invoke() {
            ((Action)handler).Invoke();
        }

        public void Invoke<T>(T t) {
            ((Action<T>)handler).Invoke(t);
        }

        public void Invoke<T1, T2>(T1 t1, T2 t2) {
            ((Action<T1, T2>)handler).Invoke(t1, t2);
        }

        public void Invoke<T1, T2, T3>(T1 t1, T2 t2, T3 t3) {
            ((Action<T1, T2, T3>)handler).Invoke(t1, t2, t3);
        }
    }

    public class MsgRegister {
        private Dictionary<int, List<Body>> handles = new Dictionary<int, List<Body>>();

        public void Register(int id, Delegate e) {
            List<Body> acts;
            if (!handles.TryGetValue(id, out acts)) {
                acts = new List<Body>();
                handles.Add(id, acts);
            }

            if (SearchWrapperIndex(acts, e) == -1) {
                acts.Add(new Body(id, e));
            }
        }

        private int SearchWrapperIndex(List<Body> wps, Delegate handler) {
            int length = wps.Count;
            for (int i = 0; i < length; ++i) {
                if (wps[i].handler == handler) {
                    return i;
                }
            }

            return -1;
        }

        public void UnRegister(int id, Delegate handler) {
            if (handler == null) {
                return;
            }

            List<Body> acts;
            if (handles.TryGetValue(id, out acts)) {
                int index = SearchWrapperIndex(acts, handler);
                if (index >= 0) {
                    acts.RemoveAt(index);
                    handles[id] = acts;
                }
            }
        }

        public void Dispatcher(int id) {
            if (handles.TryGetValue(id, out var acts)) {
                for (int i = 0; i < acts.Count; i++) {
                    acts[i].Invoke();
                }
            }
        }

        public void Dispatcher<T>(int id, T act) {
            if (handles.TryGetValue(id, out List<Body> acts)) {
                for (int i = 0; i < acts.Count; i++) {
                    acts[i].Invoke(act);
                }
            }
        }

        public void Dispatcher<T1, T2>(int id, T1 act1, T2 act2) {
            if (handles.TryGetValue(id, out List<Body> acts)) {
                for (int i = 0; i < acts.Count; i++) {
                    acts[i].Invoke(act1, act2);
                }
            }
        }
    }

    public class MessageCode {
        public static int TakeOutTheTrash = 36;//倒垃圾
        public static int CatLevelUp = 35;//猫咪升级
        public static int RefreshAllBackpackItems = 32;//显示刷新所有背包的物体

        public static int HungerAllCat = 31;//让所有的猫饥饿

        public static int CleanCatLitterBox = 30;//清理猫砂盆

        public static int CatLitterBoxFullAllCatComfortIncrease = 29;//猫砂盆没有满所有的猫咪舒适值增加
        public static int CatLitterBoxFullAllCatComfortReduced = 28;//猫砂盆满了所有猫咪舒适值降低

        public static int CatLitterBoxSendToPoopCat = 27;//猫砂盆通知拉完了
        public static int CatCallCatLitterBoxStartCatchingPoop = 26;//猫咪通知猫砂盆开始接住便便

        public static int FeedCatWithFood = 24;//喂养猫咪食物

        public static int RefrigeratorSendToCoffeeMaker = 22;//冰箱传给咖啡师
        public static int CoffeeMakerMakeRefrigeratorInRefrigerator = 23;//咖啡师在冰箱加冰

        public static int GrinderSendToCoffeeMaker = 21;//磨豆完给咖啡师
        public static int CoffeeMakerMakeGrinderInGrinder = 20;//咖啡师在磨豆机磨豆

        public static int GiveCoffeeToCustomer = 17;//把咖啡给顾客
        public static int GiveDishToCustomer = 18;//把菜给顾客

        public static int TakeAwayDishFromDishTable = 15;//从菜桌上拿走菜
        public static int TakeAwayCoffeeFromCoffeeTable = 16;//从咖啡桌上拿走咖啡

        public static int RequestDishFromDishTable = 13;//请求从菜桌上拿走菜
        public static int RequestCoffeeFromCoffeeTable = 14;//请求从咖啡桌上拿走咖啡

        public static int PutDishToDishTable = 12;//菜放到菜桌上
        public static int PutCoffeeToCoffeeTable = 11;//咖啡放到咖啡桌上

        public static int KitchenSendDishToChef = 9;//厨房制作完给厨师
        public static int CoffeeMachineSendToCoffeeMaker = 10;//咖啡制作完给咖啡师

        public static int ChefMakeDishInKitchen = 8;//厨师在厨房 制作
        public static int CoffeeMakerMakeCoffeeInCoffeeMachine = 7;//咖啡师在咖啡机 制作

        public static int OrdersBeSentToChef = 3;//订单发放到 初始
        public static int OrdersBeSentToCoffeeMaker = 4;//订单发放到 咖啡师

        public static int KitchenOrdersClose = 5;//厨房订单关闭
        public static int CoffeeOrdersClose = 6;//咖啡订单关闭

        public static int OrderFoods = 0;//陪伴猫咪到位 点食物
        public static int CustomerWalkingOut = 34;//客人开始走出去
        public static int ClearCustomer = 1;//销毁客人
        public static int CreateCustomer = 33;//创建客人

        public static int BeSatisfiedAllNeed = 2;//被满足所有需求

        public static int CustomerSettlement = 19;//顾客结算

        public static int DebugMode = 25;//调试模式
    }
}