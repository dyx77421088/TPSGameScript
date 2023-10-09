
namespace TPSShoot
{
    /// <summary>
    /// 从键盘输入出发的事件
    /// </summary>
    public partial class Events
    {
        public static Event<int> PlayerSwapWeapon;// 切换武器
        public static Event FireRequest; // 开枪的请求
        public static Event ReloadRequest; // 换弹夹的请求
        public static Event JumpRequest; // 跳跃的请求
        public static Event AimRequest; // 瞄准的请求

        public static Event GamePauseRequest; // 暂停游戏的请求
        public static Event GameResumeRequest; // 继续游戏的请求
        public static Event BagRequest; // 打开背包或关闭背包的请求

        public static Event SwordAttackRequest; // 剑攻击请求
        public static Event<PlayerBehaviour.PlayerSwordAttackMode> SwordSkillAttackRequest; // 剑技能攻击请求
    }
}
