using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed = 5f;
    public float climbTopSpeed = 0.5f;

    

    private MeshCollider ms;
    private Rigidbody body;
    private Animator animator;
    private bool isClimb = false; // 现在是否正在攀爬
    private bool isClimbTop = false;// 是否在播放爬到最顶端的动画了
    private PlayerGun playerGun;
    private bool isTransitionComplete = false;

    private float checkCD = 0.2f; // 每这么多检测一次射线
    private float checkNum = 0;
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
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        

        if (!isClimb)
        {
            SetClimbInfo();
            
            //Debug.Log("进来发射射线");
            // 发射一个射线检测前方是否碰到障碍
            RaycastHit hit;
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, 0.6f);
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.6f))
            {

                if (vertical != 0f)
                {
                    

                    isClimb = true;
                    Debug.Log("爬！！！");
                    // 收枪
                    playerGun.SetGunActive(false);

                    animator.SetBool("Climbing", true);

                    
                }
            }

            
        }

        if (isClimb && !isClimbTop)
        {
            
            if (vertical == 0) animator.enabled = false;
            else animator.enabled = true;

            
            SetClimbInfo() ;
            transform.Translate(Vector3.up * vertical * climbSpeed * Time.deltaTime);

            // 设置ik点

            RaycastHit hit3;
            if (!Physics.Raycast(transform.position + Vector3.up * 1, transform.forward, out hit3, 0.2f))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * climbSpeed);
            }

            checkNum += Time.deltaTime;
            if (vertical == 0) return;
            if (checkNum < checkCD) return;
            checkNum = 0;
            // 在攀爬中如果接触到地面了，那就结束攀爬
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.2f))
            {

                if (vertical < 0f)
                {
                    isClimb = false;
                    Debug.Log("退出攀爬");
                    // 拿枪
                    playerGun.SetGunActive(true);

                    animator.SetBool("Climbing", false);

                }
            }

            RaycastHit hit2;
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, 2f);
            // 如果快要攀爬上去了，那就播放登顶动画并退出攀爬
            // 表示在该位置未有碰撞物，退出
            if (!Physics.Raycast(transform.position + Vector3.up * 1, transform.forward, out hit2, 2f))
            {
                animator.SetTrigger("ClimbTop");
                isClimbTop = true;

                //isClimb = false;
            }
            else
            {
                // 有碰撞到,调整手的位置
                //TZ(hit2);

                // 根据碰撞点调整角色的位置
                //Vector3 point = new Vector3(hit.point.x + 2, hit.point.y, hit.point.z);
                //transform.position = point;
            }

            
        }
        
        if (isClimbTop )
        {
            // 慢慢向上攀爬
            transform.Translate(Vector3.up * climbTopSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 当前是否正在攀爬
    /// </summary>
    public bool IsClimb()
    {
        return isClimb || isClimbTop;
    }

    private void TZ(RaycastHit hit)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(2);

        Debug.Log(currentState.IsName("Climbing"));
        // 检查是否过渡动画已经完成
        if (currentState.IsName("Climbing") && currentState.normalizedTime >= 1f)
        {
            isTransitionComplete = true;
        }
        //Debug.Log(isTransitionComplete);
        if (isTransitionComplete)
        {
            Debug.Log("调整!!!");
            Vector3 point = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            animator.MatchTarget(point, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0);
        }
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
