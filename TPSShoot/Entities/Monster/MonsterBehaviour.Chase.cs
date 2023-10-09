using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// 追寻状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourChaseStatus : MonsterBehaviourStatus
        {
            // 计算是否在原地踏步
            private float count = 0;
            private float n = 100;
            private Vector3 lastPoint; // 上一个位置


            private MonsterBehaviour _mb;
            public MonsterBehaviourChaseStatus(MonsterBehaviour mb) : base(mb)
            {
                _mb = mb;
            }

            public override void OnEnter()
            {
                Debug.Log("现在进入了寻路状态");
                _mb._animator.SetFloat(_speedHash, 1);
                _mb.StartNavAgent(_mb.runSpeed, Vector3.zero);
            }

            public override void OnExit()
            {
            }

            public override void OnUpdate()
            {
                // 朝向目标
                _mb.LookAtLerp(_mb._agent.steeringTarget);
                // 是否能切换为攻击状态
                if (_mb.CanChangeAttack()) _mb.ChangeStatus(_mb._attackStatus);
                // 超出范围了
                else if (IsPlayerLost()) _mb.ChangeStatus(_mb._toBirthStatus);
                // 改变新的目标
                else if (Time.frameCount % 4 == 0)
                {
                    if (Vector3.SqrMagnitude(_mb.transform.position - lastPoint) < 0.4f)
                    {
                        count++;
                        if (count >= n)
                        {
                            _mb.ChangeStatus(_mb._toBirthStatus); // 返回出生点
                        }
                    }
                    else
                    {
                        count = 0;
                    }

                    lastPoint = _mb.transform.position;
                    UpdateNewPoint();
                }

            }

            /// <summary>
            /// 在追寻中是否 角色逃出搜索范围了
            /// </summary>
            private bool IsPlayerLost()
            {
                return _mb.GetSqrDistance() > _mb.distance && !_mb.isHit;
            }

            /// <summary>
            /// 改变到新的位置
            /// </summary>
            private void UpdateNewPoint()
            {
                Debug.Log(_mb._playerBehaviour);

                // 改变目的地
                _mb._agent.destination = _mb._playerBehaviour.transform.position;
            }
        }
    }
}
