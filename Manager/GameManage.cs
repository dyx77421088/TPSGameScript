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
        //// 如果打开背包就显示鼠标光标
        //if (PlayerBagBehaviour.Instance.IsOpenBag())
        //{
        //    ShowMouse();
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    // 未打开背包按鼠标左键隐藏
        //    HideMouse();
        //}
    }

    public void HideMouse()
    {
        // 隐藏鼠标光标
        Cursor.visible = false;

        // 锁定鼠标到屏幕中心，防止其离开窗口
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMouse()
    {
        // 退出全屏模式
        Screen.fullScreen = false;

        // 显示鼠标光标
        Cursor.visible = true;

        // 解锁鼠标
        Cursor.lockState = CursorLockMode.None;
    }

    //public bool MouseVisible()
    //{
    //    return Cursor.visible;
    //}

    /// <summary>
    /// 是否能进行移动或射击
    /// </summary>
    public bool CanMoveOrShoot()
    {
        // 鼠标为锁定状态且背包未打开，就可以进行角色移动等操作
        return !Cursor.visible && !BagsManager.Instance.IsOpenBag() && !PlayerClimb3.Instance.IsClimb();
    }
    /// <summary>
    /// 是否能进行攀爬
    /// </summary>
    public bool CanClimbOrCamera()
    {
        // 鼠标为锁定状态且背包未打开
        return !Cursor.visible && !BagsManager.Instance.IsOpenBag();
    }
}
