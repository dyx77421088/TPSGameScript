using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Gun gun; // 使用的枪

    private Animator animator;

    // 动画的属性
    private int buttleOutId = Animator.StringToHash("ButtleOut"); // 子弹耗尽换弹夹
    private int buttleLeftId = Animator.StringToHash("ButtleLeft"); // 子弹未耗尽换弹夹
    private int fireId = Animator.StringToHash("Fire"); // 开枪

    private bool canShoot = true; // 能否射击，当角色正在换弹夹的时候就不能射击
    private bool reloadButtle = false; // 是否正在换弹
    private float attackInterval = 0.1f; // 攻击间隔
    private float attackTime;

    private int currentBullet; // 当前的弹夹数
    private int remainBullet; // 备用的弹夹数
    private int oneClipBullet; // 一轮子弹最大的弹夹数

    private PlayerClimb3 playerClimb3;
    void Start()
    {
        animator = GetComponent<Animator>();
        remainBullet = gun.remainBullet;
        oneClipBullet = gun.oneClipBullet;
        currentBullet = oneClipBullet;
        UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        playerClimb3 = GetComponent<PlayerClimb3>();
    }

    // Update is called once per frame
    void Update()
    {
        // 限制条件
        if (!GameManage.Instance.CanMoveOrShoot() || playerClimb3.IsClimb()) return;
        // 如果角色是奔跑状态也不能开枪
        if (animator.GetFloat("RunSpeed") > 1.1f) return;
        if (Input.GetKey(KeyCode.Q))
        {
        }
        AmmunitionExchange();
        Shoot();
    }

    /// <summary>
    /// 换弹
    /// </summary>
    private void AmmunitionExchange()
    {
        // 按R键换弹
        if (Input.GetKeyDown(KeyCode.R))
        {
            canShoot = false;
            LoadButtleAnimatioin(Gun.GunAudio.ButtleLeft);
        }
    }

    /// <summary>
    /// 进行射击
    /// </summary>
    private void Shoot()
    {
        // 计时器
        attackTime += Time.deltaTime;
        bool isLeftMouse = Input.GetMouseButton(0);
        // 检测现在动画的状态是否为可射击状态
        canShoot = !animator.GetBool("NoGun");
        bool isFire = animator.GetCurrentAnimatorStateInfo(0).IsName("Fire");
        //if (!isLeftMouse || !canShoot) animator.SetBool(fire, false);
        if (!canShoot || (currentBullet == 0 && isFire))
        {
            return;
        }
        
        if (reloadButtle)
        {
            reloadButtle = false;
            LoadButtle(); // 加载子弹
        }
        // 修改ui
        UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        if (attackTime > attackInterval)
        {
            if (isLeftMouse)
            {
                if (currentBullet > 0)
                {
                    attackTime = 0;
                    // 进行射击
                    // 1.减少子弹，并修改ui
                    if (ReduceButtle()) // 这是正在射击
                    {
                        //animator.SetBool(fire, true);
                        if (canShoot)
                        {
                            // 以xx秒的时间切换到这个动画中
                            //animator.CrossFadeInFixedTime(fireId, 0.1f);
                            animator.SetTrigger("Attack");
                            gun.PlayGunAudio(); // 射击，
                            gun.PlayLight(); // 灯光
                        }
                    }
                }
                else
                {
                    reloadButtle = true;
                    LoadButtleAnimatioin();
                }
                
            }
            else
            {
                
                gun.PlayLight(false); // 灯光
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            currentBullet = 1000;
        }
    }

    /// <summary>
    /// 减少子弹相关的操作
    /// </summary>
    /// <returns></returns>
    private bool ReduceButtle()
    {

        if (currentBullet <= 0 && remainBullet <= 0 || !canShoot) return false;

        currentBullet--;
        // 子弹为0
        if (currentBullet == 0)
        {
            // 修改ui
            UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        }
        if (canShoot)
        {
            // 修改ui
            UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        }
        return true;
    }

    /// <summary>
    /// 换弹夹
    /// </summary>
    private void LoadButtleAnimatioin(Gun.GunAudio audio = Gun.GunAudio.ButtleOut)
    {
        // 弹夹充足，不需要换弹, 或者备用不足
        if (currentBullet == oneClipBullet || remainBullet == 0) { return; }
        remainBullet += currentBullet;
        currentBullet = 0;

        // 如果换弹且子弹大于0
        if (remainBullet > 0)
        {
            reloadButtle = true; // 正在换弹
            Debug.Log("换弹");
            // 播放换弹声音
            gun.PlayGunAudio(audio);
            // 播放动画
            animator.SetTrigger(audio == Gun.GunAudio.ButtleOut ? buttleOutId : buttleLeftId);
            canShoot = false;
        }
    }

    /// <summary>
    /// 加载子弹
    /// </summary>
    private void LoadButtle()
    {
        int addButtle = (currentBullet + remainBullet) >= oneClipBullet ? oneClipBullet : (currentBullet + remainBullet);
        remainBullet -= addButtle - currentBullet;
        currentBullet = addButtle;

        // 修改ui
        UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
    }
}
