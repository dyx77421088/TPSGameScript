using TPSShoot.Bags;
using System;
using System.Collections.Generic;
using TPSShoot.Utils;
using UnityEngine;
using static TPSShoot.Bags.Item;

namespace TPSShoot.Manger
{
    /// <summary>
    /// ���������Ʒ�Ĺ���
    /// </summary>
    public class MonsterDroppedManager : MonoBehaviour
    {
        [Header("һЩ����Ĳ���")]
        [Tooltip("�������Ե�����")]public int increase = 5; // ���Բ����������
        [Tooltip("�������Ե��¼�")]public int reduce = 5; // ���Բ����������

        [Tooltip("һ��Ʒ�ʵ�װ��")]public float common = 0.5f; // ���Բ����������
        [Tooltip("��һ��Ʒ�ʵ�װ��")]public float unCommon = 0.2f; // ���Բ����������
        [Tooltip("ϡ��Ʒ�ʵ�װ��")]public float rare = 0.15f; // ���Բ����������
        [Tooltip("ʷʫƷ�ʵ�װ��")]public float epic = 0.05f; // ���Բ����������
        [Tooltip("��˵Ʒ�ʵ�װ��")]public float legendary = 0.08f; // ���Բ����������
        [Tooltip("Զ��Ʒ�ʵ�װ��")]public float artifact = 0.02f; // ���Բ����������

        [Header("ÿ�����������Ʒ�ĸ���")]
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
        /// ��ɱmonster, grade:��ɱ�ĵȼ�
        /// </summary>
        private void OnKillMonster(MonsterAttribute attr)
        {
            foreach (var monster in monsterDroppedItems)
            {
                if (monster.type == attr.type)
                {
                    foreach (var drop in monster.droppedItem)
                    {
                        if (RandomUtils.IsDropped(drop.probability)) // ���ʵ����Ƿ����
                        {
                            Item item = PlayerBagBehaviour.Instance.GetRandomItemByType(drop.type, attr.grade);
                            if (PlayerBagBehaviour.Instance.GetKnapsack().StoryItem(item))
                            {
                                // ��ʾ��õ���Ʒ
                                Events.PlayerInfoTipShow.Call("��ã�" + item.DropingTip(), UI.PlayerInfoTipUI.PlayerInfoTipPoint.Left);
                            }
                            else
                            {
                                // ��ʾ��������
                                Events.PlayerInfoTipShow.Call("��������", UI.PlayerInfoTipUI.PlayerInfoTipPoint.Center);
                            }
                        }
                    }
                }
            }
        }
        [Serializable]
        public class MonsterDroppedItem
        {
            [Tooltip("monster������")]public MonsterType type;
            [Tooltip("���Ե�����Ʒ")] public ItemTypeAndProbability[] droppedItem;
        }

        [Serializable]
        public class ItemTypeAndProbability
        {
            [Tooltip("����")] public ItemType type;
            [Tooltip("����")][Range(0, 1)] public float probability;
        }
    }
}
