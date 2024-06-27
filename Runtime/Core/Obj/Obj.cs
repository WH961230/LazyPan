using UnityEngine;

namespace LazyPan {
    public class Obj : Singleton<Obj> {
        public Transform ObjRoot;//根节点

        public void Init() {
            
        }

        public void Preload() {
            ObjRoot = Loader.LoadGo("物体", "Global/Global_ObjRoot", null, true).transform;
        }

        //加载物体
        public Entity LoadEntity(string sign) {
            Entity instanceEntity = new Entity();
            instanceEntity.Init(sign);
            return instanceEntity;
        }

        //销毁实体
        public void UnLoadEntity(Entity entity) {
            if (EntityRegister.HasEntity(entity)) {
                entity.Clear();
            }
        }
    }
}