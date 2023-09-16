using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ����������࣬�����б����򿪶�����״̬
/// </summary>
public class BagsManager : MyMonoInstance<BagsManager>, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // ������ʱ�ƶ�����λ��
    public Transform bagsMoveTo;
    public float dragSpeed = 8f;
    private bool showBag = false;

    private float width, height;
    private bool mouseDown = false;

    private void Start()
    {
        if (bagsMoveTo == null)
        {
            GameObject go = new GameObject("BagsMoveTo");
            Instantiate(go);
            go.transform.parent = transform.parent;
            bagsMoveTo = go.transform;
            bagsMoveTo.position = new Vector3 (0, 0, 0);
        }
        width = Screen.width;
        height = Screen.height;

    }
    private void Update()
    {
        if (width != Screen.width ||  height != Screen.height)
        {
            Debug.Log("�ı��˿��");
            width = Screen.width;
            height = Screen.height;
            
            // �ı��˿�ߵļ���
        }
    }
    public void OpenOrHideBag()
    {
        if (showBag)
        {
            //bagAnimator.SetTrigger("NoBag");
            //transform.DOMove(BagsMoveTo.position, 2);
            transform.DOMove(new Vector3(width * 2, -height * 2), 1);
        }
        else
        {
            //bagAnimator.SetTrigger("Bag");
            //transform.DOMove(new Vector3(-100, 300), 2);
            transform.DOMove(bagsMoveTo.position, 1);
        }
        showBag = !showBag;
    }

    public bool IsOpenBag()
    {
        return showBag;
    }

    #region �����ק
    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseDown = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!InventoryManage.Instance.isDrag)
        {
            Vector3 v3 = transform.position;
            v3 += new Vector3(Input.GetAxis("Mouse X") * dragSpeed, Input.GetAxis("Mouse Y") * dragSpeed);
            v3.x = v3.x > width ? width : v3.x;
            v3.x = v3.x < 0 ? 0 : v3.x;
            v3.y = v3.y > height ? height : v3.y;
            v3.y = v3.y < 0 ? 0 : v3.y;
            transform.position = v3;
        }
            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mouseDown=false;
        bagsMoveTo.position = transform.position;
    }
    #endregion
}
