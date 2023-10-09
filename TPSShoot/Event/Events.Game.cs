using System.Collections.Generic;
using TPSShoot.Bags;

namespace TPSShoot
{
    public partial class Events
    {
        public static Event GamePause; // ��ͣ��Ϸ
        public static Event GameResume; // ���¿�ʼ��Ϸ

        public static Event ApplicationLoaded; // Ӧ�ü��سɹ� ��
        public static Event<List<Item>> ItemsJsonLoaded; // item��json���������

        public static Event PlayerLoaded; // ��ɫ���������
        public static Event AllAddressablesLoaded; // ���еĶ����������

        public static Event MobileInputMode; // �ֻ�����ģʽ
        public static Event DesktopInputMode; // ��������ģʽ
    }

}