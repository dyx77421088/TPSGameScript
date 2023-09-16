using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimatior : MonoBehaviour
{
    public float jumpMoveSpeed = 5f;

    private Rigidbody rb;
    private Animator animator;

    // ��ȡ��ǰ����״̬��Ϣ
    AnimatorStateInfo animStateInfo;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // ��������
        if (!GameManage.Instance.CanMoveOrShoot()) return;

        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Space) && !animStateInfo.IsName("Jump"))
        {
            animator.SetTrigger("Jump");

            Vector3 downForce = new Vector3(0, 20f, 0); // ������Ҫ��������С
            rb.AddForce(downForce, ForceMode.Force);


            

        }
        if (animStateInfo.IsName("Jump"))
        {
            // �����ײ
            RaycastHit hit;
            Debug.DrawRay(transform.position, -transform.up, Color.red);
            if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
            {
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            horizontal = Mathf.Abs(horizontal);
            vertical = Mathf.Abs(vertical);
            float speed = horizontal > vertical ? horizontal : vertical;
            if (speed > 0.001f)
            {
                rb.AddForce(transform.forward * 50 * speed);
            }
            
            //Debug.Log(vertical);
            //Vector3 movement = new Vector3(horizontal, 0f, vertical) * jumpMoveSpeed * Time.deltaTime;
            //transform.Translate(movement, Space.Self);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            // ��ӡ��ǰ����������
            Debug.Log("��ǰ���ڲ��ŵĶ����ǣ�" + animStateInfo.IsName("Idle"));
        }
    }
}
