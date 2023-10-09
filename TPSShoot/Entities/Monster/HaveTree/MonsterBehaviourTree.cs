using System.Collections;
using System.Collections.Generic;
using TPSShoot.Utils;
using UnityEngine;
using static TPSShoot.MonsterBehaviour;

namespace TPSShoot
{
    public partial class MonsterBehaviourTree : MonoBehaviour
    {
        [Header("攻击的一些设置")]
        [Tooltip("造成的伤害")] public float damage = 10;
        [Tooltip("攻击距离")] public float attackSqrDistance = 1f;
        [Tooltip("攻击超出距离")] public float attackOutSqrDistance = 2 * 2f;
        [Tooltip("攻击范围")] public float attackSphereRadius = 0.3f;
        [Tooltip("攻击偏移")] public Vector3 attackOffset = new Vector3(0, 1, 1);


        [Header("怪物的一些属性")]
        [Tooltip("头像")] public Sprite avatar;
        [Tooltip("受伤状态持续时间")] public float hitTime = 10;
        [Tooltip("受伤动画的cd")] public float hitAnimCD = 5;
        [Tooltip("寻路速度")] public float runSpeed = 5;
        [Tooltip("回到出生点的速度")] public float toBirthSpeed = 8;
        [Tooltip("死亡时间")] public float dieTime = 8;
        public MonsterAttribute monsterAttribute = new MonsterAttribute();

        // 一些动画
        public static readonly int speedHash = Animator.StringToHash("Speed");
        public static readonly int attackHash = Animator.StringToHash("Attack");
        public static readonly int dieHash = Animator.StringToHash("Die");
        public static readonly int hitHash = Animator.StringToHash("Hit");

        [HideInInspector]
        public Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
            // 初始化等级
            monsterAttribute.grade = RandomUtils.RandomInt(monsterAttribute.minGrade, monsterAttribute.maxGrade);
            // 初始化属性
            monsterAttribute.StartInit();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private bool isHit;
        private bool isHitAnim;
        private bool canHitAnim = true; // 能不能播放hit动画
        private Coroutine hitIe;
        public void OnChangeHP(int grade, int attack = 0, int magicAttack = 0)
        {
            Debug.Log("受伤");
            // 受到伤害
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
