using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerClimb2 : MyMonoInstance<PlayerClimb2>
{
    // 攀爬时需要ik的四个点
    public Transform leftHand, rightHand, leftFoot, rightFoot;
    public Transform playerLeftHandIk, playerRightHandIk, playerLeftFootIk, playerRightFootIk;
    public Transform pos1, pos2;

    public float climbSpeed = 5f;
    public float climbTopSpeed = 0.5f;

    private MeshCollider ms;
    private Rigidbody body;
    private Animator animator;

    private bool isClimb = false; // 现在是否正在攀爬
    private bool isClimbTop = false;// 是否在播放爬到最顶端的动画了
    private bool isWall = false; // 是否在墙上
    private PlayerGun playerGun;
    private bool isTransitionComplete = false;

    private float toWallOffset = 0.1f;

    //private float checkCD = 0.2f; // 每这么多检测一次射线
    //private float checkNum = 0;

    private float horizontal, vertical;

    private Vector3 targetPosition; // 射线打到的目标位置

    //private float longIk = 0.4f, shortIk = 0.2f, widthIk = 0.3f;

    #region 移动的距离和距离计数
    //private float moveDis = 2;
    //private float moveDisNum = 0;
    #endregion
    void Start()
    {
        animator = GetComponent<Animator>();
        playerGun = GetComponent<PlayerGun>();
        body = GetComponent<Rigidbody>();
        ms = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // 限制条件
        //if (!GameManage.Instance.CanClimb()) return;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // 检测是否需要进行攀爬
        CheckClimb();
        // 在攀爬中需要进行的操作
        if (isClimb)
        {
            Climbing();

            // 没有在墙上，就把角色靠到墙上
            if (!isWall)
            {
                SetBodyPositionToWall();
            }
        }



    }

    private void OnDrawGizmos()
    {
        
    }

    /// <summary>
    /// 把角色设置到墙上
    /// </summary>
    private void SetBodyPositionToWall()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Debug.Log("成功匹配！！");
            isWall = true;
            transform.position = targetPosition;
            return;
        }

        Debug.Log("开始移动了 ！！");
        Vector3 lerpTargetPos = Vector3.MoveTowards(transform.position, targetPosition, 0.2f);
        transform.position = lerpTargetPos;
    }

    public bool IsClimb()
    {
        return isClimb;
    }

    #region 检测攀爬和初始化攀爬信息
    /// <summary>
    /// 检测是否需要进行攀爬
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    private void CheckClimb()
    {

        if (!isClimb)
        {
            //Debug.Log("进来发射射线");
            // 发射一个射线检测前方是否碰到障碍
            //if (vertical != 0f || horizontal != 0f)
            {
                
                RaycastHit hit;
                Debug.DrawRay(transform.position + transform.up * 1.2f, transform.forward * 0.6f, Color.red);
                if (Physics.Raycast(transform.position + transform.up * 1.2f, transform.forward, out hit, 0.6f))
                {
                    InitClimb(hit);
                }
            }
        }
    }

    /// <summary>
    /// 开始攀爬后，初始化攀爬的信息
    /// </summary>
    /// <param name="hit"></param>
    private void InitClimb(RaycastHit hit)
    {
        isClimb = true;
        // 禁用重力和锁定xyz轴
        SetClimbInfo();
        Debug.Log("爬！！！");
        // 收枪
        playerGun.SetGunActive(false);

        animator.SetBool("Climbing", true);
        targetPosition = hit.point + hit.normal * toWallOffset;
    }

    #endregion
   
    
    /// <summary>
    /// 攀爬中
    /// </summary>
    private void Climbing()
    {
        // 角色未动就暂停动画
        if (vertical == 0)
        {
            //animator.enabled = false;
            return;
        }
        else
        {
            //animator.enabled = true;
        }

        // 角色慢慢移动
        transform.Translate(transform.up * vertical * climbSpeed * Time.deltaTime);

        //// 移动了规定的步数后重新定位手和脚的位置
        //moveDisNum += Time.deltaTime;
        ////if (moveDisNum > moveDis)
        //{
        //    moveDisNum = 0;
        //    float z = 0, z2 = transform.position.z;
        //    Vector3 pos1 = transform.position, pos2 = transform.position;
        //    RaycastHit hit1, hit2;

        //    if (Physics.Raycast(transform.position + transform.up * 1.6f, transform.forward, out hit1, 1.2f))
        //    {
        //        animator.MatchTarget()
        //    }

        //}
        // 向下移动
        if (vertical < 0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.2f))
            {

                if (vertical < 0f)
                {
                    isClimb = false;
                    // 解锁重力和xyz轴
                    SetClimbInfo();
                    Debug.Log("退出攀爬");
                    // 拿枪
                    playerGun.SetGunActive(true);
                    // 退出攀爬
                    animator.SetBool("Climbing", false);

                }
            }
        }
        // 向上移动
        else
        {

        }
    }


    #region 攀爬动画中的两个帧时会调用的方法
    /// <summary>
    /// 攀爬开始准备阶段(在攀爬动画开始时的关键帧处调用）
    /// </summary>
    public void ClimbStart()
    {
        //Debug.Log("开始调用！！ClimbStart");
        
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + transform.up * 0.4f, transform.forward, out hit, 2f))
        //{
        //    //Debug.Log("射线+++" + hit.point);
        //    //animator.MatchTarget(hit.point, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0.1f, 0.2f);
        //    //animator.MatchTarget(hit.point, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0.1f, 0.2f);
        //    //animator.MatchTarget(hit.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0, 1.1f);

        //    //Debug.Log(Mathf.Abs((transform.position - hit.point).sqrMagnitude));
        //    //if (Mathf.Abs((transform.position - hit.point).sqrMagnitude) > 0.04f)
        //    //{
        //    //    Vector3 v3 = transform.position;
        //    //    v3.z = hit.point.z + hit.normal.z * 0.2f;
        //    //    transform.position = v3;
        //    //    //transform.DOMoveZ(hit.point.z, 0.1f);
        //    //    //();
        //    //}
        //    isWall = false;
        //    targetPosition = hit.point + hit.normal * 0.5f;

        //}

    }
    /// <summary>
    /// 增加角色高度(在攀爬动画的第0.2秒关键帧处调用）
    /// </summary>
    public void ClimbRise()
    {
        Debug.Log("移动！！ClimbRise");
        // 角色慢慢移动
        //transform.Translate(transform.up * vertical * climbSpeed * Time.deltaTime);
    }
    #endregion
    
    private bool AbsMinZ(float x, float y)
    {
        return Mathf.Abs(x - transform.position.z) < Mathf.Abs(y - transform.position.z);
    }

    private void SetClimbInfo()
    {
        if (isClimb)
        {
            body.useGravity = false; // 取消重力
            body.constraints |= RigidbodyConstraints.FreezePositionX; // 锁定x轴
            body.constraints |= RigidbodyConstraints.FreezePositionY; // 锁定y轴
            body.constraints |= RigidbodyConstraints.FreezePositionZ; // 锁定z轴
        }
        else
        {
            body.useGravity = true;
            // 解锁xYz轴运动
            body.constraints &= ~RigidbodyConstraints.FreezePositionX;
            body.constraints &= ~RigidbodyConstraints.FreezePositionY;
            body.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

}
