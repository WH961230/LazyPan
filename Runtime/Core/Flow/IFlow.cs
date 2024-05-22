namespace LazyPan {
    public interface IFlow {
        public void Init(Flow flow);//初始化
        public void Clear();//清除
        public void ChangeFlow<T>();//改变状态
        public void EndFlow();//结束流程
    }
}