using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {


        // monster֮���
        public static Event<MonsterAttribute> MonsterHit; // monster �ܵ��˺�

        public static Event<MonsterAttribute, PlayerAIBehaviour> TargetHide; // Ŀ����ս

        // AI
        public static Event<PlayerAIBehaviour> PlayerAIHit; // ai �ܵ��˺�
        public static Event<PlayerAIBehaviour> PlayerAIAddHP; // ai ��Ѫ

    }

}