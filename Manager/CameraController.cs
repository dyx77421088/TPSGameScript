using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    public Transform target; // 摄像机围绕的点
    public float rotateSpeed = 5;
    public Transform player; // 主角
    public Transform gun;
    public float torsoRNum = 107f;
    public Transform playerLookAt;


    public Quaternion y = Quaternion.identity;

    public Transform playerGunAttackPos;
    public MMD4MecanimBone torso; // 上半身
    public MMD4MecanimBone neck;
    public Transform uiCenterPos;
    public float addAg = 50;

    public Transform lookCube;
    private float maxMouseY = 45;
    private Vector3 offset;
    private bool attackStatus = false, attackDown = false;
    private float mouseX;
    private float mouseY;

    private Animator animator;

    void Start()
    {
        if (player == null )
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            animator = player.GetComponent<Animator>();
        }
        if (target == null )
        {
            target = player.GetChild(2);
        }

        offset = target.position - transform.position;

        if (neck != null)
        {
            neck.userRotation = Quaternion.Euler(-45 * player.up);
        }
    }

    void LateUpdate()
    {
        // 限制条件
        if (!GameManage.Instance.CanClimbOrCamera()) return;

        // 如果没有进入射击视角，摄像头就在角色头上
        LookPlayer();

        UpdateFirePosition();
    }

    private void UpdateFirePosition()
    {
        //attackDown = Input.GetMouseButtonDown(1);
        //if (attackDown) // 按下右键
        //{
            
        //    neck.userRotation = Quaternion.Euler(-45 * player.up);
        //    torso.userRotation = Quaternion.Euler(65 * player.up);
        //    player.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z), transform.up);

        //    // 摄像头的位置调整到角色攻击位置
        //    transform.position = playerGunAttackPos.position;
        //    transform.rotation = playerGunAttackPos.rotation;

        //    attackStatus = true;
        //}
        //if (Input.GetMouseButtonUp(1)) // 松开右键
        //{
        //    attackStatus = false;
        //    neck.userRotation = Quaternion.identity;
        //    torso.userRotation = Quaternion.identity;
        //}


        if (Input.GetMouseButton(1)) 
        {
            player.LookAt(new Vector3(playerLookAt.position.x, player.position.y, playerLookAt.position.z));
            torso.transform.LookAt(playerLookAt.position);
        }

        //bool b = Input.GetMouseButton(1);
        //if (!b)
        //{
        //    neck.userRotation = Quaternion.identity;
        //    torso.userRotation = Quaternion.identity;

        //    return;
        //}
        //else
        //{
        //    neck.userRotation = Quaternion.Euler(-45 * player.up);
        //    torso.userRotation = Quaternion.Euler(torsoRNum * player.up);
        //}
        //if (playerGunAttackPos != null)
        //{
        //    //player.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z), transform.up);

        //    Vector3 cameraEulerAngles = transform.eulerAngles;
        //    Transform characterTransform = player.transform;
        //    Vector3 characterEulerAngles = characterTransform.eulerAngles;
        //    characterEulerAngles.y = cameraEulerAngles.y; // 跟随摄像机旋转
        //    characterTransform.eulerAngles = characterEulerAngles;

            


        //    transform.position = playerGunAttackPos.position;

        //    Transform character = transform; // 假设你想旋转的角色是当前脚本所在物体的Transform组件
        //    Transform targetObject = playerGunAttackPos; // 假设目标物体是一个Transform组件，你可以用目标物体的引用来替换targetTransform

        //    Vector3 rotationEuler = character.eulerAngles; // 获取角色当前的欧拉角
        //    //rotationEuler.x = targetObject.eulerAngles.x; // 将角色的X轴与目标物体的X轴旋转角度相同
        //    character.eulerAngles = rotationEuler; // 将新的欧拉角应用到角色上
        //    //playerGunAttackPos.LookAt(transform.position + transform.forward * 40);

        //    //y.y = mouseX;

        //    Ray r = Camera.main.ScreenPointToRay(uiCenterPos.position);
        //    RaycastHit hit;
        //    if (Physics.Raycast(r, out hit, 100))
        //    {
        //        lookCube.position = hit.point;
        //        Debug.Log(gun.transform.position);
        //        //Vector3 forwardDirection = player.forward;
        //        //Vector3 directionToTarget = hit.point - player.position;
        //        //float angle = Vector3.SignedAngle(forwardDirection, directionToTarget, player.up);
        //        ////Debug.Log(Quaternion.Euler(player.up * (angle + addAg)));
        //        //torso.userRotation = Quaternion.Euler(player.up * (angle + 0));


        //        //Vector3 currentRotation = torso.userRotation.eulerAngles;
        //        //float yRotation = currentRotation.y;// 将Y轴旋转角度保存下来。
        //        //Vector3 targetPoint = hit.point; // 目标点的位置
        //        //Vector3 direction = targetPoint - neck.transform.position;
        //        ////direction.y = 0f; // 忽略Y轴分量
        //        //direction.Normalize(); // 归一化向量
        //        //Quaternion xzRotation = Quaternion.LookRotation(direction, Vector3.up);
        //        //// 将Y轴旋转角度重新应用到新的旋转中。
        //        //Vector3 newRotation = xzRotation.eulerAngles;
        //        //newRotation.y = yRotation;
        //        //// 将新的旋转应用到物体。
        //        //torso.userRotation = Quaternion.Euler(newRotation);
        //        //lookCube.rotation = torso.userRotation;

        //        Vector3 currentRotation = gun.transform.eulerAngles;
        //        float yRotation = currentRotation.y;// 将Y轴旋转角度保存下来。
        //        Vector3 targetPoint = hit.point; // 目标点的位置
        //        Vector3 direction = targetPoint - gun.transform.position;
        //        //direction.y = 0f; // 忽略Y轴分量
        //        direction.Normalize(); // 归一化向量
        //        Quaternion xzRotation = Quaternion.LookRotation(direction, Vector3.up);
        //        // 将Y轴旋转角度重新应用到新的旋转中。
        //        Vector3 newRotation = xzRotation.eulerAngles;
        //        newRotation.y = yRotation;
        //        // 将新的旋转应用到物体。
        //        gun.rotation = Quaternion.Euler(newRotation);
        //        lookCube.rotation = gun.rotation;
        //        //lookCube.rotation = Quaternion.Euler(newRotation);
        //    }
        //    //Debug.Log(y.x);

        //    //bone.userRotation = new Quaternion(0, y.x, 0, 1);

        //}
    }

    private Vector3 hitPos;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(hitPos, .2f);
    //}

    private void LookPlayer()
    {
        //Debug.Log("1111111111");
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;

        // 最大的旋转角度（y）
        mouseY = mouseY > maxMouseY ? maxMouseY : mouseY;
        mouseY = mouseY < -maxMouseY ? -maxMouseY : mouseY;

        //Quaternion rotation = Quaternion.Euler(mouseY, attackStatus ? 0 : mouseX, 0);
        //transform.position = (attackStatus ? playerGunAttackPos.position : target.position) 
        //    - (rotation * (attackStatus ? Vector3.one : offset));
        //transform.LookAt(attackStatus ? playerGunAttackPos : target);

        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.position = (target.position)
            - (rotation * (offset));
        transform.LookAt(target);
    }
}
