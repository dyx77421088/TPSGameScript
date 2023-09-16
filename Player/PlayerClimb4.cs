using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerClimb4 : MyMonoInstance<PlayerClimb4>
{
    public float rotationSpeed = 5f;
    public float climbSpeed = 2f; // 攀爬的速度
    public float climbTopSpeed = 0.5f; // 到达顶端的速度
    public Transform climbHeadPos, climbFootPos; // 用来定位的pos


    private Rigidbody body;
    private Animator animator;

    // ****************攀爬的一些状态**************
    private bool isClimb = false; // 现在是否正在攀爬
    private bool isClimbTop = false;// 是否在播放爬到最顶端的动画了
    private bool isWall = false; // 是否在墙上
    private bool isRayHead, isRayFoot; // 头部和脚部是否有射线射到墙上

    // 角色的枪，在攀爬状态中需要把枪给收起
    private PlayerGun playerGun;

    private float horizontal, vertical;

    private Vector3 headRayPosition; // 头部发射的射线打到的目标位置
    private Vector3 footRayPosition; // 脚部发射射线打到的目标位置
    private Transform headRayTrans;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerGun = GetComponent<PlayerGun>();
        body = GetComponent<Rigidbody>();
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
        if (isClimb && !isClimbTop)
        {
            //Climbing();

            //GetHeadFootRay();
            //// 没有在墙上，就把角色靠到墙上
            //if (!isWall)
            //{
                
            //    SetBodyPositionToWall();
            //}

            //if (CheckClimbTop())
            //{
            //    // 登顶
            //    InitClimbTop();
            //}
            if (Input.GetKey(KeyCode.P))
            {
            }
        }



    }

    /// <summary>
    /// 检测是否已经要登顶了
    /// </summary>
    private bool CheckClimbTop()
    {
        RaycastHit hit;
        Vector3 dir = transform.forward;
        dir.y = 0;
        return !Physics.SphereCast(headPos + Vector3.up * 0.2f, 0.1f, dir, out hit, 1f);
    }

    private void InitClimbTop()
    {
        animator.enabled = true;
        animator.SetTrigger("ClimbTop");
        isClimbTop = true;
    }

    // 这个在攀爬动画中添加了一个关键帧，所以在最后一秒会执行
    public void OnClimbComplete()
    {
        // 动画播放完毕后执行的操作
        Debug.Log("攀爬完成！");
        animator.ResetTrigger("ClimbTop");
        animator.SetBool("Climbing", false);

        
        isClimb = false;
        isClimbTop = false;
        // 拿枪
        playerGun.SetGunActive(true);

        SetClimbInfo();
    }

    private Vector3 headPos, footPos;
    /// <summary>
    /// 获得头部位置发射的射线和脚部位置发射的射线
    /// </summary>
    private void GetHeadFootRay()
    {
        // 获得头和脚的位置
        headPos = GetZ0Pos(climbHeadPos.position);
        footPos = GetZ0Pos(climbFootPos.position);

        isRayHead = isRayFoot = false; // 初始化为否
        // 从头和脚发射圆,获得交点的位置
        RaycastHit hit;
        Debug.DrawRay(headPos, transform.forward * 1, Color.green);
        if (Physics.SphereCast(headPos, 0.1f, transform.forward, out hit, 1f))
        {
            headRayPosition = hit.point;
            headRayTrans = hit.transform;
            isRayHead = true;
        }

        if (Physics.SphereCast(footPos, 0.1f, transform.forward, out hit, 1f))
        {
            footRayPosition = hit.point;
            isRayFoot = true;
        }
    }

    private Vector3 GetZ0Pos(Vector3 pos)
    {
        // 把climbHeadPos的坐标转换到局部坐标，好像和climbHeadPos.localPosition差不多
        Vector3 localClimbHeadPos = transform.InverseTransformPoint(pos);
        Vector3 localHeadPos = new Vector3(localClimbHeadPos.x, localClimbHeadPos.y, 0); // 头部的z轴为0
        return transform.TransformPoint(localHeadPos); // 转换成世界坐标
    }

    //private void OnDrawGizmos()
    //{
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(headPos, 0.02f);
        //Gizmos.DrawSphere(climbHeadPos.position, 0.02f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(footPos, 0.02f);
        //Gizmos.DrawSphere(climbFootPos.position, 0.02f);

        //Gizmos.color = Color.yellow;
        //Debug.Log("会话中" + transform.TransformPoint(GetIkPos(HumanBodyBones.LeftHand)));
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.LeftHand)), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.LeftFoot)), 0.1f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.RightHand)), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.RightFoot)), 0.1f);


    //}

    /// <summary>
    /// 调整角色的位置，让角色紧挨着墙壁
    /// </summary>
    private void SetBodyPositionToWall()
    {
        if (!isRayFoot && !isRayHead)
        {
            return;
        }


        Vector3 v3 = transform.position - climbHeadPos.position + headRayPosition;
        if (Vector3.Distance(v3, transform.position) > 0.05f)
        {
            v3.x = transform.position.x;
            v3.y = transform.position.y;
            transform.position = v3;
        }
        else
        {
            isRayHead = true;
        }
        Debug.Log("修复！！！");

        // 比较参考点的位置和射线的位置哪个距离角色更近
        bool b = AbsMinZ(climbFootPos.position.z, footRayPosition.z);
        if ( Mathf.Abs(climbFootPos.position.z - footRayPosition.z) > 0.1f)
        {
            // 角色绕着头部射线和墙壁相交的点旋转
            //transform.RotateAround(headRayPosition, headRayTrans.right, b ? 0.2f : -0.2f);
            // 计算旋转轴
            Vector3 rotationAxis = Vector3.Cross(footRayPosition - headRayPosition, transform.right).normalized;

            // 使旋转轴朝向目标点
            Quaternion targetRotation = Quaternion.LookRotation(footRayPosition - headRayPosition, rotationAxis);

            // 进行旋转
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        }
        else
        {
            isRayFoot = true;
        }

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
            if (vertical != 0f || horizontal != 0f)
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

        animator.SetFloat("ClimbX", 0); // 初始化为0
        animator.SetFloat("ClimbY", 0); // 初始化为0
        animator.SetBool("Climbing", true);

        //targetPosition = hit.point + hit.normal;
    }

    private Vector3 GetIkPos(HumanBodyBones bones)
    {
        if (animator == null) return Vector3.zero;
        return transform.InverseTransformPoint(animator.GetBoneTransform(bones).position);
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
            animator.enabled = false;
            //OpenXYZ(false);
            return;
        }
        else
        {
            animator.enabled = true;
            //OpenXYZ(true);
        }

        // 角色慢慢移动
        transform.Translate(transform.up * vertical * climbSpeed * Time.deltaTime);
        animator.SetFloat("ClimbY", vertical);
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


    public bool IsClimb()
    {
        return isClimb;
    }
    private bool AbsMinZ(float x, float y)
    {
        return Mathf.Abs(x - transform.position.z) < Mathf.Abs(y - transform.position.z);
    }

    private void SetClimbInfo()
    {
        if (isClimb)
        {
            body.useGravity = false; // 取消重力
            OpenXYZ(false);
        }
        else
        {
            body.useGravity = true;
            OpenXYZ(true);
        }
    }

    private void OpenXYZ(bool b)
    {
        if (b)
        {
            // 解锁xYz轴运动
            body.constraints &= ~RigidbodyConstraints.FreezePositionX;
            body.constraints &= ~RigidbodyConstraints.FreezePositionY;
            body.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            body.constraints |= RigidbodyConstraints.FreezePositionX; // 锁定x轴
            body.constraints |= RigidbodyConstraints.FreezePositionY; // 锁定y轴
            body.constraints |= RigidbodyConstraints.FreezePositionZ; // 锁定z轴
        }
    }

}
