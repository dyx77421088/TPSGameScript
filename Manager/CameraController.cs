using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    public Transform target; // �����Χ�Ƶĵ�
    public float rotateSpeed = 5;
    public Transform player; // ����
    public Transform gun;
    public float torsoRNum = 107f;
    public Transform playerLookAt;


    public Quaternion y = Quaternion.identity;

    public Transform playerGunAttackPos;
    public MMD4MecanimBone torso; // �ϰ���
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
        // ��������
        if (!GameManage.Instance.CanClimbOrCamera()) return;

        // ���û�н�������ӽǣ�����ͷ���ڽ�ɫͷ��
        LookPlayer();

        UpdateFirePosition();
    }

    private void UpdateFirePosition()
    {
        //attackDown = Input.GetMouseButtonDown(1);
        //if (attackDown) // �����Ҽ�
        //{
            
        //    neck.userRotation = Quaternion.Euler(-45 * player.up);
        //    torso.userRotation = Quaternion.Euler(65 * player.up);
        //    player.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z), transform.up);

        //    // ����ͷ��λ�õ�������ɫ����λ��
        //    transform.position = playerGunAttackPos.position;
        //    transform.rotation = playerGunAttackPos.rotation;

        //    attackStatus = true;
        //}
        //if (Input.GetMouseButtonUp(1)) // �ɿ��Ҽ�
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
        //    characterEulerAngles.y = cameraEulerAngles.y; // �����������ת
        //    characterTransform.eulerAngles = characterEulerAngles;

            


        //    transform.position = playerGunAttackPos.position;

        //    Transform character = transform; // ����������ת�Ľ�ɫ�ǵ�ǰ�ű����������Transform���
        //    Transform targetObject = playerGunAttackPos; // ����Ŀ��������һ��Transform������������Ŀ��������������滻targetTransform

        //    Vector3 rotationEuler = character.eulerAngles; // ��ȡ��ɫ��ǰ��ŷ����
        //    //rotationEuler.x = targetObject.eulerAngles.x; // ����ɫ��X����Ŀ�������X����ת�Ƕ���ͬ
        //    character.eulerAngles = rotationEuler; // ���µ�ŷ����Ӧ�õ���ɫ��
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
        //        //float yRotation = currentRotation.y;// ��Y����ת�Ƕȱ���������
        //        //Vector3 targetPoint = hit.point; // Ŀ����λ��
        //        //Vector3 direction = targetPoint - neck.transform.position;
        //        ////direction.y = 0f; // ����Y�����
        //        //direction.Normalize(); // ��һ������
        //        //Quaternion xzRotation = Quaternion.LookRotation(direction, Vector3.up);
        //        //// ��Y����ת�Ƕ�����Ӧ�õ��µ���ת�С�
        //        //Vector3 newRotation = xzRotation.eulerAngles;
        //        //newRotation.y = yRotation;
        //        //// ���µ���תӦ�õ����塣
        //        //torso.userRotation = Quaternion.Euler(newRotation);
        //        //lookCube.rotation = torso.userRotation;

        //        Vector3 currentRotation = gun.transform.eulerAngles;
        //        float yRotation = currentRotation.y;// ��Y����ת�Ƕȱ���������
        //        Vector3 targetPoint = hit.point; // Ŀ����λ��
        //        Vector3 direction = targetPoint - gun.transform.position;
        //        //direction.y = 0f; // ����Y�����
        //        direction.Normalize(); // ��һ������
        //        Quaternion xzRotation = Quaternion.LookRotation(direction, Vector3.up);
        //        // ��Y����ת�Ƕ�����Ӧ�õ��µ���ת�С�
        //        Vector3 newRotation = xzRotation.eulerAngles;
        //        newRotation.y = yRotation;
        //        // ���µ���תӦ�õ����塣
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

        // ������ת�Ƕȣ�y��
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
