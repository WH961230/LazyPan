namespace LazyPan {
    public partial class Label {
        //全局
        public static string CAMERA = "Camera"; //相机
        public static string ANIMATOR = "Animator"; //动画播放器
        public static string CHARACTERCONTROLLER = "CharacterController"; //角色控制器
        public static string PLAYABLEDIRECTOR = "PlayableDirector"; //时间轴播放器
        public static string TIMELINEASSET = "TimelineAsset"; //时间轴资源
        public static string TRAILRENDERER = "TrailRenderer"; //拖尾
        public static string EVENT = "Event"; //事件
        public static string TRIGGER = "Trigger"; //触发器
        public static string COLLIDER = "Collider"; //碰撞体
        public static string MATERIAL = "Material"; //材质
        public static string RENDERER = "Renderer"; //渲染
        public static string COMP = "Comp"; //组件
        public static string TMPINPUTFIELD = "TMPInputField"; //输入
        public static string TEXT = "Text"; //文本
        public static string BUTTON = "Button"; //按钮
        public static string SCALE = "Scale"; //缩放
        public static string BODY = "Body"; //身体位置
        public static string FOOT = "Foot"; //脚底位置
        public static string CODE = "Code"; //代码
        public static string CONTENT = "Content"; //内容
        public static string NEXT = "Next";
        public static string MOTION = "Motion";
        public static string BUFF = "Buff"; //霸服
        public static string MENU = "Menu"; //菜单
        public static string COUNTDOWN = "CountDown"; //倒计时
        public static string CLEAR = "Clear"; //结算
        public static string A = "A"; //固定数量临时标签
        public static string B = "B"; //固定数量临时标签
        public static string C = "C"; //固定数量临时标签
        public static string D = "D";
        public static string E = "E";
        public static string F = "F";
        public static string G = "G";
        public static string H = "H";
        public static string MAP = "Map";//地图
        public static string PROGRESS = "Progress";//进度
        public static string STORE = "Store";//商店
        public static string STICKER = "Sticker";//贴纸
        public static string SELECT = "Select";//选择
        public static string ITEM = "Item";//物品
        public static string PARENT = "Parent";//父物体
        public static string NAME = "Name";//名字
        public static string ICON = "Icon";//图标
        public static string CLICK = "Click";//点击
        public static string SURE = "Sure";//确定
        public static string BACK = "Back";//返回
        public static string CAT = "Cat";//猫咪
        public static string DISPLAY = "Display";//展示
        public static string CANCEL = "Cancel";//取消
        public static string TIP = "Tip";//提示
        public static string ALL = "All";//所有
        public static string AGENT = "Agent";//寻路
        public static string COFFEE = "Coffee";//咖啡
        public static string DISH = "Dish";//菜
        public static string BACKPACK = "Backpack";//背包
        public static string DEBUG = "Debug";//调试
        public static string NUM = "Num";//数量
        public static string MONEY = "Money";//金币
        public static string ALWAYS = "Always";//常驻
        public static string PRICE = "Price";//价格
        public static string SETTLEMENT = "Settlement";//结算
        public static string LIST = "List";//列表
        public static string INFO = "Info";//信息
        public static string UPGRADE = "Upgrade";//升级
        public static string SLIDER = "Slider";//滑条
        public static string ACCOMPANY = "Accompany";//陪伴
        public static string CANVAS = "Canvas";//图版
        public static string DELIVERY = "Delivery";//传递
        public static string KEY = "Key";//按键
        public static string ROOT = "Root";//根
        public static string FRAME = "Frame";//边框
        public static string LEVEL = "Level";//等级
        public static string HOVER = "Hover";//覆盖
        public static string RATING = "Rating";//评级
        public static string MACHINE = "Machine";//设备
        public static string BOW = "Bow";//碗
        public static string GRID = "Grid";//格子
        public static string PAGE = "Page";//页面
        public static string LEFT = "Left";//左
        public static string RIGHT = "Right";//右
        public static string BOTTOM = "Bottom";//下方
        public static string HOLD = "Hold";//持有数量
        public static string BUY = "Buy";//购买
        public static string INTERFACE = "Interface";//界面
        public static string FISH = "Fish";//鱼
        public static string TASTY = "Tasty";//美味
        public static string EXOTIC = "Exotic";//异域
        public static string BELOW = "Below";//以下
        public static string ADDED = "Added";//已添加的
        public static string REMOVE = "Remove";//移除
        public static string CART = "Cart";//购物车
        public static string CLOSE = "Close";//关闭
        public static string VALUE = "Value";//数值
        public static string PICTURE = "Picture";//图片
        public static string MOVEMENT = "Movement";//移动
        public static string SPEED = "Speed";//速度
        public static string ING = "ing";//进行中
        public static string ENERGY = "Energy";//能量
        public static string MAX = "Max";//最大
        public static string JUMP = "Jump";//跳转
        public static string START = "Start";//开始
        public static string GAME = "Game";//游戏
        public static string GRAVITY = "Gravity";//重力
        public static string FORCE = "Force";//力
        public static string VELOCITY = "Velocity";//向量
        public static string SIGN = "Sign";//标识
        public static string SCORE = "Score";//积分

        //组合A+B
        public static string Assemble(string labelA, string labelB) {
            return string.Concat(labelA, labelB);
        }

        //组合A+B+C
        public static string Assemble(string labelA, string labelB, string labelC) {
            return string.Concat(labelA, labelB, labelC);
        }
    }
}
