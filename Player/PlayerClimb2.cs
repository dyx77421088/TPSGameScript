using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerClimb2 : MyMonoInstance<PlayerClimb2>
{
    // ����ʱ��Ҫik���ĸ���
    public Transform leftHand, rightHand, leftFoot, rightFoot;
    public Transform playerLeftHandIk, playerRightHandIk, playerLeftFootIk, playerRightFootIk;
    public Transform pos1, pos2;

    public float climbSpeed = 5f;
    public float climbTopSpeed = 0.5f;

    private MeshCollider ms;
    private Rigidbody body;
    private Animator animator;

    private bool isClimb = false; // �����Ƿ���������
    private bool isClimbTop = false;// �Ƿ��ڲ���������˵Ķ�����
    private bool isWall = false; // �Ƿ���ǽ��
    private PlayerGun playerGun;
    private bool isTransitionComplete = false;

    private float toWallOffset = 0.1f;

    //private float checkCD = 0.2f; // ÿ��ô����һ������
    //private float checkNum = 0;

    private float horizontal, vertical;

    private Vector3 targetPosition; // ���ߴ򵽵�Ŀ��λ��

    //private float longIk = 0.4f, shortIk = 0.2f, widthIk = 0.3f;

    #region �ƶ��ľ���;������
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
        // ��������
        //if (!GameManage.Instance.CanClimb()) return;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // ����Ƿ���Ҫ��������
        CheckClimb();
        // ����������Ҫ���еĲ���
        if (isClimb)
        {
            Climbing();

            // û����ǽ�ϣ��Ͱѽ�ɫ����ǽ��
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
    /// �ѽ�ɫ���õ�ǽ��
    /// </summary>
    private void SetBodyPositionToWall()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Debug.Log("�ɹ�ƥ�䣡��");
            isWall = true;
            transform.position = targetPosition;
            return;
        }

        Debug.Log("��ʼ�ƶ��� ����");
        Vector3 lerpTargetPos = Vector3.MoveTowards(transform.position, targetPosition, 0.2f);
        transform.position = lerpTargetPos;
    }

    public bool IsClimb()
    {
        return isClimb;
    }

    #region ��������ͳ�ʼ��������Ϣ
    /// <summary>
    /// ����Ƿ���Ҫ��������
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    private void CheckClimb()
    {

        if (!isClimb)
        {
            //Debug.Log("������������");
            // ����һ�����߼��ǰ���Ƿ������ϰ�
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
    /// ��ʼ�����󣬳�ʼ����������Ϣ
    /// </summary>
    /// <param name="hit"></param>
    private void InitClimb(RaycastHit hit)
    {
        isClimb = true;
        // ��������������xyz��
        SetClimbInfo();
        Debug.Log("��������");
        // ��ǹ
        playerGun.SetGunActive(false);

        animator.SetBool("Climbing", true);
        targetPosition = hit.point + hit.normal * toWallOffset;
    }

    #endregion
   
    
    /// <summary>
    /// ������
    /// </summary>
    private void Climbing()
    {
        // ��ɫδ������ͣ����
        if (vertical == 0)
        {
            //animator.enabled = false;
            return;
        }
        else
        {
            //animator.enabled = true;
        }

        // ��ɫ�����ƶ�
        transform.Translate(transform.up * vertical * climbSpeed * Time.deltaTime);

        //// �ƶ��˹涨�Ĳ��������¶�λ�ֺͽŵ�λ��
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
        // �����ƶ�
        if (vertical < 0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.2f))
            {

                if (vertical < 0f)
                {
                    isClimb = false;
                    // ����������xyz��
                    SetClimbInfo();
                    Debug.Log("�˳�����");
                    // ��ǹ
                    playerGun.SetGunActive(true);
                    // �˳�����
                    animator.SetBool("Climbing", false);

                }
            }
        }
        // �����ƶ�
        else
        {

        }
    }


    #region ���������е�����֡ʱ����õķ���
    /// <summary>
    /// ������ʼ׼���׶�(������������ʼʱ�Ĺؼ�֡�����ã�
    /// </summary>
    public void ClimbStart()
    {
        //Debug.Log("��ʼ���ã���ClimbStart");
        
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + transform.up * 0.4f, transform.forward, out hit, 2f))
        //{
        //    //Debug.Log("����+++" + hit.point);
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
    /// ���ӽ�ɫ�߶�(�����������ĵ�0.2��ؼ�֡�����ã�
    /// </summary>
    public void ClimbRise()
    {
        Debug.Log("�ƶ�����ClimbRise");
        // ��ɫ�����ƶ�
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
            body.useGravity = false; // ȡ������
            body.constraints |= RigidbodyConstraints.FreezePositionX; // ����x��
            body.constraints |= RigidbodyConstraints.FreezePositionY; // ����y��
            body.constraints |= RigidbodyConstraints.FreezePositionZ; // ����z��
        }
        else
        {
            body.useGravity = true;
            // ����xYz���˶�
            body.constraints &= ~RigidbodyConstraints.FreezePositionX;
            body.constraints &= ~RigidbodyConstraints.FreezePositionY;
            body.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

}
