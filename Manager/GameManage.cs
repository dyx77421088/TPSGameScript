using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

public class GameManage : MyMonoInstance<GameManage>
{
    // Start is called before the first frame update
    void Start()
    {
        HideMouse();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    ShowMouse();
        //}
        //// ����򿪱�������ʾ�����
        //if (PlayerBagBehaviour.Instance.IsOpenBag())
        //{
        //    ShowMouse();
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    // δ�򿪱���������������
        //    HideMouse();
        //}
    }

    public void HideMouse()
    {
        // ���������
        Cursor.visible = false;

        // ������굽��Ļ���ģ���ֹ���뿪����
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMouse()
    {
        // �˳�ȫ��ģʽ
        Screen.fullScreen = false;

        // ��ʾ�����
        Cursor.visible = true;

        // �������
        Cursor.lockState = CursorLockMode.None;
    }

    //public bool MouseVisible()
    //{
    //    return Cursor.visible;
    //}

    /// <summary>
    /// �Ƿ��ܽ����ƶ������
    /// </summary>
    public bool CanMoveOrShoot()
    {
        // ���Ϊ����״̬�ұ���δ�򿪣��Ϳ��Խ��н�ɫ�ƶ��Ȳ���
        return !Cursor.visible && !BagsManager.Instance.IsOpenBag() && !PlayerClimb3.Instance.IsClimb();
    }
    /// <summary>
    /// �Ƿ��ܽ�������
    /// </summary>
    public bool CanClimbOrCamera()
    {
        // ���Ϊ����״̬�ұ���δ��
        return !Cursor.visible && !BagsManager.Instance.IsOpenBag();
    }
}
