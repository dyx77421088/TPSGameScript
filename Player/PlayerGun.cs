using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject gun;
    public Transform leftHandIk;
    public Transform rightHandIk;
    public Transform rightHandAttackIk;

    private Animator animator;
    private GunManager gunManager;
    private PlayerClimb playerClimb;
    private Camera cam;
    void Start()
    {
        animator = GetComponent<Animator>();
        gunManager = gun.GetComponent<GunManager>();
        playerClimb = GetComponent<PlayerClimb>();
        cam = Camera.main;

        // 获取手和头部的IK控制器（如Full Body IK插件）
        //var handIK = GetComponent<FullBodyBipedIK>().references.leftHand;
        //var headIK = GetComponent<FullBodyBipedIK>().references.head;

        //// 设置IK控制器的目标位置
        //handIK.position = targetPosition;
        //headIK.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // 限制条件
        if (!GameManage.Instance.CanMoveOrShoot()) return;

        if (playerClimb != null)
        {
            if (playerClimb.IsClimb()) return;
        }

        // 如果角色是奔跑状态也不能开枪
        if (animator.GetFloat("RunSpeed") > 1.1f) return;
        if (Input.GetMouseButton(0))
        {
            animator.SetTrigger("Attack");
            //gunManager.switchPos(GunManager.GunPosition.AttackPos);


            Quaternion q = transform.rotation;
            q.y = cam.transform.rotation.y;
            transform.rotation = q;

        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SetGunActive(true);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            SetGunActive(false);
        }
    }

    public void SetGunActive(bool active)
    {
        animator.SetBool("NoGun", !active);
        gun.SetActive (active);
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        //Debug.Log(layerIndex);
        if (/*layerIndex == 1 &&*/ animator != null && gun.activeSelf)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIk.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIk.rotation);
            
            //if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Fire"))
            //{
            //    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIk.position);
            //    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIk.rotation);
            //}
            //else
            {
                //animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandAttackIk.position);
                //animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandAttackIk.rotation);
            }

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            //MMD4MecanimModel;
        }
    }
}
