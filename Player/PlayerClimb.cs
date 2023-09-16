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
    private bool isClimb = false; // �����Ƿ���������
    private bool isClimbTop = false;// �Ƿ��ڲ���������˵Ķ�����
    private PlayerGun playerGun;
    private bool isTransitionComplete = false;

    private float checkCD = 0.2f; // ÿ��ô����һ������
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
            
            //Debug.Log("������������");
            // ����һ�����߼��ǰ���Ƿ������ϰ�
            RaycastHit hit;
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, 0.6f);
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.6f))
            {

                if (vertical != 0f)
                {
                    

                    isClimb = true;
                    Debug.Log("��������");
                    // ��ǹ
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

            // ����ik��

            RaycastHit hit3;
            if (!Physics.Raycast(transform.position + Vector3.up * 1, transform.forward, out hit3, 0.2f))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * climbSpeed);
            }

            checkNum += Time.deltaTime;
            if (vertical == 0) return;
            if (checkNum < checkCD) return;
            checkNum = 0;
            // ������������Ӵ��������ˣ��Ǿͽ�������
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.2f))
            {

                if (vertical < 0f)
                {
                    isClimb = false;
                    Debug.Log("�˳�����");
                    // ��ǹ
                    playerGun.SetGunActive(true);

                    animator.SetBool("Climbing", false);

                }
            }

            RaycastHit hit2;
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, 2f);
            // �����Ҫ������ȥ�ˣ��ǾͲ��ŵǶ��������˳�����
            // ��ʾ�ڸ�λ��δ����ײ��˳�
            if (!Physics.Raycast(transform.position + Vector3.up * 1, transform.forward, out hit2, 2f))
            {
                animator.SetTrigger("ClimbTop");
                isClimbTop = true;

                //isClimb = false;
            }
            else
            {
                // ����ײ��,�����ֵ�λ��
                //TZ(hit2);

                // ������ײ�������ɫ��λ��
                //Vector3 point = new Vector3(hit.point.x + 2, hit.point.y, hit.point.z);
                //transform.position = point;
            }

            
        }
        
        if (isClimbTop )
        {
            // ������������
            transform.Translate(Vector3.up * climbTopSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// ��ǰ�Ƿ���������
    /// </summary>
    public bool IsClimb()
    {
        return isClimb || isClimbTop;
    }

    private void TZ(RaycastHit hit)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(2);

        Debug.Log(currentState.IsName("Climbing"));
        // ����Ƿ���ɶ����Ѿ����
        if (currentState.IsName("Climbing") && currentState.normalizedTime >= 1f)
        {
            isTransitionComplete = true;
        }
        //Debug.Log(isTransitionComplete);
        if (isTransitionComplete)
        {
            Debug.Log("����!!!");
            Vector3 point = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            animator.MatchTarget(point, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(new Vector3(1, 1, 1), 0), 0);
        }
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
