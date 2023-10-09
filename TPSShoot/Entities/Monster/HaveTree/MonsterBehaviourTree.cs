using System.Collections;
using System.Collections.Generic;
using TPSShoot.Utils;
using UnityEngine;
using static TPSShoot.MonsterBehaviour;

namespace TPSShoot
{
    public partial class MonsterBehaviourTree : MonoBehaviour
    {
        [Header("������һЩ����")]
        [Tooltip("��ɵ��˺�")] public float damage = 10;
        [Tooltip("��������")] public float attackSqrDistance = 1f;
        [Tooltip("������������")] public float attackOutSqrDistance = 2 * 2f;
        [Tooltip("������Χ")] public float attackSphereRadius = 0.3f;
        [Tooltip("����ƫ��")] public Vector3 attackOffset = new Vector3(0, 1, 1);


        [Header("�����һЩ����")]
        [Tooltip("ͷ��")] public Sprite avatar;
        [Tooltip("����״̬����ʱ��")] public float hitTime = 10;
        [Tooltip("���˶�����cd")] public float hitAnimCD = 5;
        [Tooltip("Ѱ·�ٶ�")] public float runSpeed = 5;
        [Tooltip("�ص���������ٶ�")] public float toBirthSpeed = 8;
        [Tooltip("����ʱ��")] public float dieTime = 8;
        public MonsterAttribute monsterAttribute = new MonsterAttribute();

        // һЩ����
        public static readonly int speedHash = Animator.StringToHash("Speed");
        public static readonly int attackHash = Animator.StringToHash("Attack");
        public static readonly int dieHash = Animator.StringToHash("Die");
        public static readonly int hitHash = Animator.StringToHash("Hit");

        [HideInInspector]
        public Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
            // ��ʼ���ȼ�
            monsterAttribute.grade = RandomUtils.RandomInt(monsterAttribute.minGrade, monsterAttribute.maxGrade);
            // ��ʼ������
            monsterAttribute.StartInit();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private bool isHit;
        private bool isHitAnim;
        private bool canHitAnim = true; // �ܲ��ܲ���hit����
        private Coroutine hitIe;
        public void OnChangeHP(int grade, int attack = 0, int magicAttack = 0)
        {
            Debug.Log("����");
            // �ܵ��˺�
            if (hitIe != null) { StopCoroutine(hitIe); }
            hitIe = StartCoroutine(hitIE());

            OnHit(grade, attack, magicAttack);

        }
        private IEnumerator hitIE()
        {
            isHit = true;
            yield return new WaitForSeconds(hitTime);
            isHit = false;
        }
    }
}
