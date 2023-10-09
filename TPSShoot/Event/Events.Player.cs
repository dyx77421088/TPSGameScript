using System;
using TPSShoot.UI;
using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        // 开枪
        public static Event PlayerFire;
        
        public static Event PlayerShowGunWeapon; // 拿出枪
        public static Event PlayerHideGunWeapon; // 隐藏枪

        public static Event PlayerShowSwordWeapon; // 拿剑的状态
        public static Event PlayerHideSwordWeapon; // 隐藏剑的状态
        public static Event<PlayerBehaviour.PlayerSwordAttackMode> PlayerSwordSkill; // 使用剑的技能

        public static Event PlayerReloaded; // 换弹完成

        public static Event PlayerAimActive; // 瞄准激活
        public static Event PlayerAimOut; // 瞄准结束

        

        public static Event<int, Action> PlayerAddBulletAmount; // 增加子弹
        // 角色属性相关的
        public static Event<int, int> PlayerReturnHPAndMP; // 角色回血和回蓝
        public static Event PlayerChangeCurrentHP; // 改变当前血量
        public static Event PlayerChangeMAXHP; // 角色改变最大血量
        public static Event PlayerChangeCurrentMP; // 改变当前蓝量
        public static Event PlayerChangeMAXMP; // 角色改变最大蓝量

        public static Event PlayerOpenBag; // 角色打开背包
        public static Event PlayerCloseBag; // 角色关闭背包
        public static Event PlayerDied; // 角色死亡

        public static Event<int> PlayerAddExp; // 角色获得经验
        public static Event PlayerChangeEXP; // 角色改变当前的经验条
        public static Event PlayerUpgradeRequest; // 角色升级请求
        public static Event PlayerGradeChange; // 角色等级改变

        // 提示信息之类的
        public static Event<string, PlayerInfoTipUI.PlayerInfoTipPoint> PlayerInfoTipShow; // 左边的提示信息

        // monster之类的 <类型，等级>
        public static Event<MonsterAttribute> PlayerKillMonster; // 击杀monster
    }

}