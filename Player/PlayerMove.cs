using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float rotateSpeed = 5;
    public float speed = 5;

    private Animator animator;
    private Transform mainCamera;
    private PlayerClimb playerClimb;
    private PlayerClimb3 playerClimb3;
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.transform;
        playerClimb3 = GetComponent<PlayerClimb3>();
    }

    private string RunAnimation = "Run";
    private float multiply = 1;
    void Update()
    {
        // 限制条件
        if (!GameManage.Instance.CanMoveOrShoot()) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float runSpeed = Mathf.Abs(h) > Mathf.Abs(v) ? Mathf.Abs(h) : Mathf.Abs(v);

        if (playerClimb3 != null)
        {
            if (playerClimb3.IsClimb()) return;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            multiply = 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            multiply = 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            multiply = multiply == 0.5f ? 1 : 0.5f;
        }

        if (h != 0 || v != 0)
        {
            //Vector3 cameraRotation = mainCamera.rotation.eulerAngles;
            //Quaternion cameraQuaternion = Quaternion.Euler(cameraRotation);
            //Vector3 cameraDirection = cameraQuaternion * Vector3.forward;
            //float distance = 10f; // 假设相机与角色之间的距离为10个单位
            //cameraDirection *= distance;

            //Vector3 v3 = transform.position + cameraDirection;
            //v3.y = transform.position.y; // y轴不变
            ////v3.x = v > 0 ? v3.x : -v3.x;
            //transform.LookAt(v > 0 ? v3 : -v3);

            Vector3 cameraEulerAngles = mainCamera.rotation.eulerAngles;
            Vector3 forwardDirection = Quaternion.Euler(0, cameraEulerAngles.y, 0) * Vector3.forward;
            Vector3 rightDirection = Quaternion.Euler(0, cameraEulerAngles.y + 90, 0) * Vector3.forward;

            // 根据按键输入计算出移动方向
            Vector3 movementDirection = v * forwardDirection + h * rightDirection;
            transform.LookAt(transform.position + movementDirection);
            //Debug.Log(movementDirection);
            //transform.position = (movementDirection);
            //CharacterController controller = GetComponent<CharacterController>();
            //controller.Move(movementDirection * speed * Time.deltaTime);

            
        }
        animator.SetFloat("RunSpeed", runSpeed * multiply);
        //else
        //{
        //    animator.SetBool(RunAnimation, false);
        //}




    }
}
