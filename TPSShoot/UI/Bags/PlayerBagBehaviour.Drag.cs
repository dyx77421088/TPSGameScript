using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TPSShoot.Bags
{
    /// <summary>
    /// ÍÏ×§±³°ü
    /// </summary>
    public partial class PlayerBagBehaviour : IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        
        private bool mouseDown = false;

        #region Êó±êÍÏ×§
        public void OnBeginDrag(PointerEventData eventData)
        {
            mouseDown = true;

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDrag)
            {
                Vector3 v3 = transform.position;
                v3 += new Vector3(Input.GetAxis("Mouse X") * dragSpeed, Input.GetAxis("Mouse Y") * dragSpeed);
                v3.x = v3.x > _width ? _width : v3.x;
                v3.x = v3.x < 0 ? 0 : v3.x;
                v3.y = v3.y > _height ? _height : v3.y;
                v3.y = v3.y < 0 ? 0 : v3.y;
                transform.position = v3;
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            mouseDown = false;
            bagsMoveTo.position = transform.position;
        }
        #endregion
    }
}
