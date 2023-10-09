using TPSShoot.Bags;
using TPSShoot.Manger;
using UnityEngine;

namespace TPSShoot.Utils
{
    public class RandomUtils
    {
        public static float RandomFloat(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static int RandomInt(int min, int max)
        {
            return Random.Range(min, max);
        }
        // 正数
        private static int RandomIntPositive(int min, int max)
        {
            min = Mathf.Max(min, 1);
            max = Mathf.Max(max, 1);
            return Random.Range(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original">原先的值</param>
        /// <param name="randomDiff">上下浮动多少</param>
        /// <returns></returns>
        public static int RandomAttribute(int original, int randomDiff)
        {
            return RandomIntPositive(original - randomDiff, original + randomDiff);
        }
        public static int RandomAttribute(int original, int increase, int reduce)
        {
            return RandomIntPositive(original - reduce, original + increase);
        }
        /// <summary>
        /// 在给出的概率中是否为真
        /// </summary>
        public static bool IsDropped (float probability)
        {
            if (probability <= 0 || probability > 1f) return false;
            return probability >= RandomFloat(0, 1);
        }

        /// <summary>
        /// 随机品质
        /// </summary>
        public static Item.ItemQuality RandomQuality()
        {
            // 品质
            float[] rq = {MonsterDroppedManager.Instance.common
                    ,  MonsterDroppedManager.Instance.unCommon
                    ,  MonsterDroppedManager.Instance.rare
                    ,  MonsterDroppedManager.Instance.epic
                    ,  MonsterDroppedManager.Instance.legendary
                    ,  MonsterDroppedManager.Instance.artifact
            };
            float t = 0;
            for (int i = 0; i < rq.Length; i++) 
            {
                t += rq[i]; // 累加，最后一定会是1
                if (IsDropped(t))
                {
                    return GetQuality(i);
                }
            }
            return Item.ItemQuality.Common;
        }

        private static Item.ItemQuality GetQuality(int type)
        {
            switch (type)
            {
                case 0:
                    return Item.ItemQuality.Common;
                case 1:
                    return Item.ItemQuality.Uncommon;
                case 2:
                    return Item.ItemQuality.Rare;
                case 3:
                    return Item.ItemQuality.Epic;
                case 4:
                    return Item.ItemQuality.Legendary;
                case 5:
                    return Item.ItemQuality.Artifact;
            }
            return Item.ItemQuality.Common;
        }
    }
}
