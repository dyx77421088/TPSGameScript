using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        public static Event BagsShowAttributeText; // 显示属性的text
        public static Event BagsComputeAttribute; // 计算属性

        public static Event<string, string> BagsShowTip; // 显示tip
        public static Event BagsHideTip; // 隐藏tip

        public static Event<ItemUi> BagsKnapsackPutOn; // 穿上装备


    }

}