using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Gun gun; // ʹ�õ�ǹ

    private Animator animator;

    // ����������
    private int buttleOutId = Animator.StringToHash("ButtleOut"); // �ӵ��ľ�������
    private int buttleLeftId = Animator.StringToHash("ButtleLeft"); // �ӵ�δ�ľ�������
    private int fireId = Animator.StringToHash("Fire"); // ��ǹ

    private bool canShoot = true; // �ܷ����������ɫ���ڻ����е�ʱ��Ͳ������
    private bool reloadButtle = false; // �Ƿ����ڻ���
    private float attackInterval = 0.1f; // �������
    private float attackTime;

    private int currentBullet; // ��ǰ�ĵ�����
    private int remainBullet; // ���õĵ�����
    private int oneClipBullet; // һ���ӵ����ĵ�����

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
        // ��������
        if (!GameManage.Instance.CanMoveOrShoot() || playerClimb3.IsClimb()) return;
        // �����ɫ�Ǳ���״̬Ҳ���ܿ�ǹ
        if (animator.GetFloat("RunSpeed") > 1.1f) return;
        if (Input.GetKey(KeyCode.Q))
        {
        }
        AmmunitionExchange();
        Shoot();
    }

    /// <summary>
    /// ����
    /// </summary>
    private void AmmunitionExchange()
    {
        // ��R������
        if (Input.GetKeyDown(KeyCode.R))
        {
            canShoot = false;
            LoadButtleAnimatioin(Gun.GunAudio.ButtleLeft);
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    private void Shoot()
    {
        // ��ʱ��
        attackTime += Time.deltaTime;
        bool isLeftMouse = Input.GetMouseButton(0);
        // ������ڶ�����״̬�Ƿ�Ϊ�����״̬
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
            LoadButtle(); // �����ӵ�
        }
        // �޸�ui
        UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        if (attackTime > attackInterval)
        {
            if (isLeftMouse)
            {
                if (currentBullet > 0)
                {
                    attackTime = 0;
                    // �������
                    // 1.�����ӵ������޸�ui
                    if (ReduceButtle()) // �����������
                    {
                        //animator.SetBool(fire, true);
                        if (canShoot)
                        {
                            // ��xx���ʱ���л������������
                            //animator.CrossFadeInFixedTime(fireId, 0.1f);
                            animator.SetTrigger("Attack");
                            gun.PlayGunAudio(); // �����
                            gun.PlayLight(); // �ƹ�
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
                
                gun.PlayLight(false); // �ƹ�
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            currentBullet = 1000;
        }
    }

    /// <summary>
    /// �����ӵ���صĲ���
    /// </summary>
    /// <returns></returns>
    private bool ReduceButtle()
    {

        if (currentBullet <= 0 && remainBullet <= 0 || !canShoot) return false;

        currentBullet--;
        // �ӵ�Ϊ0
        if (currentBullet == 0)
        {
            // �޸�ui
            UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        }
        if (canShoot)
        {
            // �޸�ui
            UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
        }
        return true;
    }

    /// <summary>
    /// ������
    /// </summary>
    private void LoadButtleAnimatioin(Gun.GunAudio audio = Gun.GunAudio.ButtleOut)
    {
        // ���г��㣬����Ҫ����, ���߱��ò���
        if (currentBullet == oneClipBullet || remainBullet == 0) { return; }
        remainBullet += currentBullet;
        currentBullet = 0;

        // ����������ӵ�����0
        if (remainBullet > 0)
        {
            reloadButtle = true; // ���ڻ���
            Debug.Log("����");
            // ���Ż�������
            gun.PlayGunAudio(audio);
            // ���Ŷ���
            animator.SetTrigger(audio == Gun.GunAudio.ButtleOut ? buttleOutId : buttleLeftId);
            canShoot = false;
        }
    }

    /// <summary>
    /// �����ӵ�
    /// </summary>
    private void LoadButtle()
    {
        int addButtle = (currentBullet + remainBullet) >= oneClipBullet ? oneClipBullet : (currentBullet + remainBullet);
        remainBullet -= addButtle - currentBullet;
        currentBullet = addButtle;

        // �޸�ui
        UIManager.Instance.SetCartridgeText(currentBullet, remainBullet);
    }
}
