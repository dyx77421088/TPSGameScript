
namespace TPSShoot
{
    /// <summary>
    /// �Ӽ�������������¼�
    /// </summary>
    public partial class Events
    {
        public static Event<int> PlayerSwapWeapon;// �л�����
        public static Event FireRequest; // ��ǹ������
        public static Event ReloadRequest; // �����е�����
        public static Event JumpRequest; // ��Ծ������
        public static Event AimRequest; // ��׼������

        public static Event GamePauseRequest; // ��ͣ��Ϸ������
        public static Event GameResumeRequest; // ������Ϸ������
        public static Event BagRequest; // �򿪱�����رձ���������

        public static Event SwordAttackRequest; // ����������
        public static Event<PlayerBehaviour.PlayerSwordAttackMode> SwordSkillAttackRequest; // �����ܹ�������
    }
}
