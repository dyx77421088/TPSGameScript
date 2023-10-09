using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace TPSShoot
{
    public class PlayerSword : PlayerWeapon
    {
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
                attackSphereRadius * count,
                LayerMask.GetMask(Layers.Monster) | LayerMask.GetMask(Layers.AI)
            );
            playerSwordSound.Play(count);
            foreach (Collider c in coolider)
            {
                c.GetComponent<MonsterBehaviour>()?.OnChangeHP(
                    PlayerBehaviour.Instance.CurrentGrade,
                    PlayerBehaviour.Instance.playerAttribute.CurrentAggressivity
                    ); 

                c.GetComponent<PlayerAIBehaviour>()?.OnHit(
                    PlayerBehaviour.Instance.CurrentGrade,
                    PlayerBehaviour.Instance.GetAggressivity()
                    ); 
            }
        }
    }
}