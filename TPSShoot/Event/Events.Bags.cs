using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        public static Event BagsShowAttributeText; // ��ʾ���Ե�text
        public static Event BagsComputeAttribute; // ��������

        public static Event<string, string> BagsShowTip; // ��ʾtip
        public static Event BagsHideTip; // ����tip

        public static Event<ItemUi> BagsKnapsackPutOn; // ����װ��


    }

}