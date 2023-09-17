using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        // ��ǹ
        public static Event PlayerFire;
        
        public static Event PlayerShowWeapon; // �ó�ǹ
        public static Event PlayerHideWeapon; // ����ǹ

        public static Event PlayerReloaded; // �������

        public static Event PlayerAimActive; // ��׼����
        public static Event PlayerAimOut; // ��׼����

        // ��ɫ������ص�
        public static Event PlayerChangeCurrentHP; // ��ɫ�ı䵱ǰѪ��
        public static Event PlayerChangeMAXHP; // ��ɫ�ı����Ѫ��

        public static Event PlayerOpenBag; // ��ɫ�򿪱���
        public static Event PlayerCloseBag; // ��ɫ�رձ���
        public static Event PlayerDied; // ��ɫ����

    }

}