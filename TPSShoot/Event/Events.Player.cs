using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        // ��ǹ
        public static Event PlayerFire;
        
        public static Event PlayerReloaded; // �������

        public static Event PlayerAimActive; // ��׼����
        public static Event PlayerAimOut; // ��׼����

        // ��ɫ������ص�
        public static Event PlayerChangeCurrentHP; // ��ɫ�ı䵱ǰѪ��
        public static Event PlayerChangeMAXHP; // ��ɫ�ı����Ѫ��
    }

}