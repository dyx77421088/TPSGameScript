using TPSShoot.Bags;
using System;
using System.Collections.Generic;
using TPSShoot.Utils;
using UnityEngine;
using static TPSShoot.Bags.Item;

namespace TPSShoot.Manger
{
    /// <summary>
    /// 怪物掉落物品的管理
    /// </summary>
    public class MonsterDroppedManager : MonoBehaviour
    {
        [Header("一些掉落的参数")]
        [Tooltip("掉落属性的上增")]public int increase = 5; // 属性波动上下五点
        [Tooltip("掉落属性的下减")]public int reduce = 5; // 属性波动上下五点

        [Tooltip("一般品质的装备")]public float common = 0.5f; // 属性波动上下五点
        [Tooltip("不一般品质的装备")]public float unCommon = 0.2f; // 属性波动上下五点
        [Tooltip("稀有品质的装备")]public float rare = 0.15f; // 属性波动上下五点
        [Tooltip("史诗品质的装备")]public float epic = 0.05f; // 属性波动上下五点
        [Tooltip("传说品质的装备")]public float legendary = 0.08f; // 属性波动上下五点
        [Tooltip("远古品质的装备")]public float artifact = 0.02f; // 属性波动上下五点

        [Header("每个怪物掉落物品的概率")]
        public MonsterDroppedItem[] monsterDroppedItems;

        private static MonsterDroppedManager instance;
        public static MonsterDroppedManager Instance { get { return instance; } }   
        private void Awake()
        {
            instance = this;
            Subscribe();
        }
        private void Start()
        {
        }
        private void OnDestroy()
        {
            UnSubscribe();
        }
        private void Subscribe()
        {
            Events.PlayerKillMonster += OnKillMonster;
        }
        private void UnSubscribe()
        {
            Events.PlayerKillMonster -= OnKillMonster;
        }
        /// <summary>
        /// 击杀monster, grade:击杀的等级
        /// </summary>
        private void OnKillMonster(MonsterAttribute attr)
        {
            foreach (var monster in monsterDroppedItems)
            {
                if (monster.type == attr.type)
                {
                    foreach (var drop in monster.droppedItem)
                    {
                        if (RandomUtils.IsDropped(drop.probability)) // 概率掉落是否成立
                        {
                            Item item = PlayerBagBehaviour.Instance.GetRandomItemByType(drop.type, attr.grade);
                            if (PlayerBagBehaviour.Instance.GetKnapsack().StoryItem(item))
                            {
                                // 提示获得的物品
                                Events.PlayerInfoTipShow.Call("获得：" + item.DropingTip(), UI.PlayerInfoTipUI.PlayerInfoTipPoint.Left);
                            }
                            else
                            {
                                // 提示背包已满
                                Events.PlayerInfoTipShow.Call("背包已满", UI.PlayerInfoTipUI.PlayerInfoTipPoint.Center);
                            }
                        }
                    }
                }
            }
        }
        [Serializable]
        public class MonsterDroppedItem
        {
            [Tooltip("monster的类型")]public MonsterType type;
            [Tooltip("可以掉落物品")] public ItemTypeAndProbability[] droppedItem;
        }

        [Serializable]
        public class ItemTypeAndProbability
        {
            [Tooltip("类型")] public ItemType type;
            [Tooltip("概率")][Range(0, 1)] public float probability;
        }
    }
}
