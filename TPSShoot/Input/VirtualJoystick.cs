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
        [Tooltip("������Ǹ�ͼƬ")]public Image joystickImage;

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
            //����Ļ�����ת��Ϊָ�� RectTransform �ڵı��������ķ�������ͨ������ UI ����У����ڴ��������¼����� UI Ԫ�ؽ��н�����
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backImage.rectTransform, e.position, e.pressEventCamera, out pos))
            {
                _tx = pos.x;
                _ty = pos.y;

                // pos.magnitude��ó��ȣ��������һ�룬��Ҫ����,�����ڱ���ͼƬ֮��
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
