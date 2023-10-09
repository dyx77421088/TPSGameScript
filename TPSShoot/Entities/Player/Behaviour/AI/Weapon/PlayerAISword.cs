using UnityEngine;

namespace TPSShoot
{
    public class PlayerAISword : PlayerWeapon
    {
        public PlayerAIBehaviour pab;
        [Header("攻击属性设置")]
        [Tooltip("攻击偏移")] public Vector3 attackOffset = new Vector3(0, 1, 1f);
        [Tooltip("攻击范围")] public float attackSphereRadius = 1f;
        [Header("音效设置")]
        public PlayerSwordWeaponSoundSettings playerSwordSound;

        public void OnAttack(int count)
        {
            // 创建一个球体，并返回碰撞体
            Collider[] coolider = Physics.OverlapSphere(
                transform.position + transform.rotation * attackOffset,
                attackSphereRadius * 2 * count,
                LayerMask.GetMask(Layers.Player)
            );
            playerSwordSound.Play(count);
            foreach (Collider c in coolider)
            {
                c.GetComponent<PlayerBehaviour>()?.OnHit(
                    pab.CurrentGrade,
                    pab.aiAttribute.aggressivity
                    ); 

            }
        }
    }
}