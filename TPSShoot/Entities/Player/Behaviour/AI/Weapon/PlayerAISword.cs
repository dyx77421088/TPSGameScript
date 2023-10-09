using UnityEngine;

namespace TPSShoot
{
    public class PlayerAISword : PlayerWeapon
    {
        public PlayerAIBehaviour pab;
        [Header("������������")]
        [Tooltip("����ƫ��")] public Vector3 attackOffset = new Vector3(0, 1, 1f);
        [Tooltip("������Χ")] public float attackSphereRadius = 1f;
        [Header("��Ч����")]
        public PlayerSwordWeaponSoundSettings playerSwordSound;

        public void OnAttack(int count)
        {
            // ����һ�����壬��������ײ��
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