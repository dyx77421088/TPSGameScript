using System.Collections.Generic;
using TPSShoot.Bags;

namespace TPSShoot
{
    public partial class Events
    {
        public static Event GamePause; // 暂停游戏
        public static Event GameResume; // 重新开始游戏

        public static Event ApplicationLoaded; // 应用加载成功 了
        public static Event<List<Item>> ItemsJsonLoaded; // item的json加载完成了

        public static Event PlayerLoaded; // 角色加载完成了
        public static Event AllAddressablesLoaded; // 所有的都加载完成了

        public static Event MobileInputMode; // 手机输入模式
        public static Event DesktopInputMode; // 电脑输入模式
    }

}