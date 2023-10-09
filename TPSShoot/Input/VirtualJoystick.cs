using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TPSShoot
{
    [RequireComponent(typeof(Image))]
    public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Tooltip("里面的那个图片")]public Image joystickImage;

        private Image backImage;
        private float backHalfWidth;
        public float HorizontalValue { get; private set; }
        public float VerticalValue { get; private set; }


        private void Start()
        {
            backImage = GetComponent<Image>();

            backHalfWidth = backImage.rectTransform.sizeDelta.x / 2;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            HorizontalValue = 0;
            VerticalValue = 0;
            joystickImage.transform.localPosition = Vector3.zero;
        }
        private float _tx, _ty;
        public void OnDrag(PointerEventData e)
        {
            Vector2 pos;
            //将屏幕坐标点转换为指定 RectTransform 内的本地坐标点的方法。它通常用于 UI 编程中，用于处理输入事件并与 UI 元素进行交互。
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backImage.rectTransform, e.position, e.pressEventCamera, out pos))
            {
                _tx = pos.x;
                _ty = pos.y;

                // pos.magnitude获得长度，如果大于一半，就要变少,控制在背景图片之内
                if (pos.magnitude > backHalfWidth)
                {
                    float mul = backHalfWidth / pos.magnitude;
                    _tx *= mul;
                    _ty *= mul;
                }

                joystickImage.transform.localPosition = new Vector2(_tx, _ty);

                HorizontalValue = _tx / backHalfWidth;
                VerticalValue = _ty / backHalfWidth;
            }
            //joystickImage.transform.position = e.position;
        }
    }
}
