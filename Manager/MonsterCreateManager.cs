using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace TPSShoot.Manger
{
    public class MonsterCreateManager : MonoBehaviour
    {
        [Header("出生点设置")]
        public MonsterCreatePoint[] monsterCreatePoint;

        private System.Random random = new System.Random();

        private void Start()
        {
            foreach (var monster in monsterCreatePoint)
            {
                MonsterCreateInfo[] ts = monster.pointList.GetComponentsInChildren<MonsterCreateInfo>();

                foreach(var t in ts)
                {
                    StartCoroutine(StartCreateMonster(monster, t));
                }
            }
        }
        private IEnumerator StartCreateMonster(MonsterCreatePoint point, MonsterCreateInfo birthInfo)
        {
            while (true)
            {
                yield return new WaitUntil(() => birthInfo.go == null);
                if(!birthInfo.isFirstBirth) yield return new WaitForSeconds(point.birthTime);
                birthInfo.isFirstBirth = false;
                // 创建
                CreateMonster(point, birthInfo);
            }
        }

        private void CreateMonster(MonsterCreatePoint point, MonsterCreateInfo birthPoint)
        {
            int type = random.Next(0, point.monsterPrefabs.Length);
            birthPoint.go = Instantiate(point.monsterPrefabs[type], birthPoint.GetBirthPosition(), birthPoint.GetBirthRotation());
        }

        [Serializable]
        public class MonsterCreatePoint
        {
            [Tooltip("出生点集合")]public GameObject pointList;
            [Tooltip("这个出生点可以产生的怪物类型")]public GameObject[] monsterPrefabs;
            [Tooltip("重新产生的时间")]public float birthTime;
            
        }
    }
}
