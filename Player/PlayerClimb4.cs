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
    public float climbSpeed = 2f; // �������ٶ�
    public float climbTopSpeed = 0.5f; // ���ﶥ�˵��ٶ�
    public Transform climbHeadPos, climbFootPos; // ������λ��pos


    private Rigidbody body;
    private Animator animator;

    // ****************������һЩ״̬**************
    private bool isClimb = false; // �����Ƿ���������
    private bool isClimbTop = false;// �Ƿ��ڲ���������˵Ķ�����
    private bool isWall = false; // �Ƿ���ǽ��
    private bool isRayHead, isRayFoot; // ͷ���ͽŲ��Ƿ��������䵽ǽ��

    // ��ɫ��ǹ��������״̬����Ҫ��ǹ������
    private PlayerGun playerGun;

    private float horizontal, vertical;

    private Vector3 headRayPosition; // ͷ����������ߴ򵽵�Ŀ��λ��
    private Vector3 footRayPosition; // �Ų��������ߴ򵽵�Ŀ��λ��
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
        // ��������
        //if (!GameManage.Instance.CanClimb()) return;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // ����Ƿ���Ҫ��������
        CheckClimb();
        // ����������Ҫ���еĲ���
        if (isClimb && !isClimbTop)
        {
            //Climbing();

            //GetHeadFootRay();
            //// û����ǽ�ϣ��Ͱѽ�ɫ����ǽ��
            //if (!isWall)
            //{
                
            //    SetBodyPositionToWall();
            //}

            //if (CheckClimbTop())
            //{
            //    // �Ƕ�
            //    InitClimbTop();
            //}
            if (Input.GetKey(KeyCode.P))
            {
            }
        }



    }

    /// <summary>
    /// ����Ƿ��Ѿ�Ҫ�Ƕ���
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

    // ��������������������һ���ؼ�֡�����������һ���ִ��
    public void OnClimbComplete()
    {
        // ����������Ϻ�ִ�еĲ���
        Debug.Log("������ɣ�");
        animator.ResetTrigger("ClimbTop");
        animator.SetBool("Climbing", false);

        
        isClimb = false;
        isClimbTop = false;
        // ��ǹ
        playerGun.SetGunActive(true);

        SetClimbInfo();
    }

    private Vector3 headPos, footPos;
    /// <summary>
    /// ���ͷ��λ�÷�������ߺͽŲ�λ�÷��������
    /// </summary>
    private void GetHeadFootRay()
    {
        // ���ͷ�ͽŵ�λ��
        headPos = GetZ0Pos(climbHeadPos.position);
        footPos = GetZ0Pos(climbFootPos.position);

        isRayHead = isRayFoot = false; // ��ʼ��Ϊ��
        // ��ͷ�ͽŷ���Բ,��ý����λ��
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
        // ��climbHeadPos������ת�����ֲ����꣬�����climbHeadPos.localPosition���
        Vector3 localClimbHeadPos = transform.InverseTransformPoint(pos);
        Vector3 localHeadPos = new Vector3(localClimbHeadPos.x, localClimbHeadPos.y, 0); // ͷ����z��Ϊ0
        return transform.TransformPoint(localHeadPos); // ת������������
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
        //Debug.Log("�Ự��" + transform.TransformPoint(GetIkPos(HumanBodyBones.LeftHand)));
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.LeftHand)), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.LeftFoot)), 0.1f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.RightHand)), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(GetIkPos(HumanBodyBones.RightFoot)), 0.1f);


    //}

    /// <summary>
    /// ������ɫ��λ�ã��ý�ɫ������ǽ��
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
        Debug.Log("�޸�������");

        // �Ƚϲο����λ�ú����ߵ�λ���ĸ������ɫ����
        bool b = AbsMinZ(climbFootPos.position.z, footRayPosition.z);
        if ( Mathf.Abs(climbFootPos.position.z - footRayPosition.z) > 0.1f)
        {
            // ��ɫ����ͷ�����ߺ�ǽ���ཻ�ĵ���ת
            //transform.RotateAround(headRayPosition, headRayTrans.right, b ? 0.2f : -0.2f);
            // ������ת��
            Vector3 rotationAxis = Vector3.Cross(footRayPosition - headRayPosition, transform.right).normalized;

            // ʹ��ת�ᳯ��Ŀ���
            Quaternion targetRotation = Quaternion.LookRotation(footRayPosition - headRayPosition, rotationAxis);

            // ������ת
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        }
        else
        {
            isRayFoot = true;
        }

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

        animator.SetFloat("ClimbX", 0); // ��ʼ��Ϊ0
        animator.SetFloat("ClimbY", 0); // ��ʼ��Ϊ0
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
    /// ������
    /// </summary>
    private void Climbing()
    {
        // ��ɫδ������ͣ����
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

        // ��ɫ�����ƶ�
        transform.Translate(transform.up * vertical * climbSpeed * Time.deltaTime);
        animator.SetFloat("ClimbY", vertical);
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
            body.useGravity = false; // ȡ������
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
            // ����xYz���˶�
            body.constraints &= ~RigidbodyConstraints.FreezePositionX;
            body.constraints &= ~RigidbodyConstraints.FreezePositionY;
            body.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            body.constraints |= RigidbodyConstraints.FreezePositionX; // ����x��
            body.constraints |= RigidbodyConstraints.FreezePositionY; // ����y��
            body.constraints |= RigidbodyConstraints.FreezePositionZ; // ����z��
        }
    }

}
