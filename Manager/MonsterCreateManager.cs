using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace TPSShoot.Manger
{
    public class MonsterCreateManager : MonoBehaviour
    {
        [Header("����������")]
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
                // ����
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
            [Tooltip("�����㼯��")]public GameObject pointList;
            [Tooltip("�����������Բ����Ĺ�������")]public GameObject[] monsterPrefabs;
            [Tooltip("���²�����ʱ��")]public float birthTime;
            
        }
    }
}
