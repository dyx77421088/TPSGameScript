using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {


        // monster之类的
        public static Event<MonsterAttribute> MonsterHit; // monster 受到伤害

        public static Event<MonsterAttribute, PlayerAIBehaviour> TargetHide; // 目标脱战

        // AI
        public static Event<PlayerAIBehaviour> PlayerAIHit; // ai 受到伤害
        public static Event<PlayerAIBehaviour> PlayerAIAddHP; // ai 回血

    }

}