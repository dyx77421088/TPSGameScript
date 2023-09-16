using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        // 开枪
        public static Event PlayerFire;
        
        public static Event PlayerReloaded; // 换弹完成

        public static Event PlayerAimActive; // 瞄准激活
        public static Event PlayerAimOut; // 瞄准结束

        // 角色属性相关的
        public static Event PlayerChangeCurrentHP; // 角色改变当前血量
        public static Event PlayerChangeMAXHP; // 角色改变最大血量
    }

}