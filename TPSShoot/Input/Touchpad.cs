using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TPSShoot
{
    [RequireComponent(typeof(Image))]
    public class Touchpad : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Header("灵敏度")]
        [SerializeField] private float _sensitivity = 20f;

        private float _horizontalValue, _verticalValue;
        // 上一次的值
        private float _lastHorizontalValue, _lastVerticalValue;
        private bool _isDragging;

        public bool IsDragging { get { return _isDragging; } }
        public float HorizontalValue 
        { 
            get 
            { 
                if (_lastHorizontalValue == _horizontalValue) return 0; 
                else
                {
                    _lastHorizontalValue = _horizontalValue;
                    return _isDragging ? _horizontalValue : 0;
                }
            } 
        }
        public float VerticalValue 
        {
            get
            {
                if (_lastVerticalValue == _verticalValue) return 0;
                else
                {
                    _lastVerticalValue = _verticalValue;
                    return _isDragging ?  _verticalValue : 0;
                }
            }
        }


        private void Start()
        {
            if (!GetComponent<Image>().raycastTarget)
                Debug.LogError("触摸屏的raycast value应该设置为true");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            _horizontalValue = 0;
            _verticalValue = 0;
        }
        public void OnDrag(PointerEventData e)
        {
            _horizontalValue = e.delta.x * 0.0061f * _sensitivity;
            _verticalValue = e.delta.y * 0.0061f * _sensitivity;
        }
    }
}
