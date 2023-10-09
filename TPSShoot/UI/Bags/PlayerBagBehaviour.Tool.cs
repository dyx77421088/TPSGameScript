using System.Collections.Generic;
using TPSShoot.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour
    {
        public bool isDrag;

        private bool isTipShow;
        private Text text1;
        private Text text2;
        private CanvasGroup canvasGroup;
        private RectTransform rect;

        private bool isTipContrastShow;
        private Text text1Contrast;
        private Text text2Contrast;
        private CanvasGroup canvasGroupContrast;
        private RectTransform rectContrast;

        private Vector3 transV = Vector2.zero;
        public float TipWidth { get => rect.rect.width; }
        public float TipContrastWidth { get => rectContrast.rect.width; }
        public float TipHeight { get => rect.rect.height; }
        public float TipContrastHeight { get => rectContrast.rect.height; }
        private void AwakeInitTool()
        {
            text1 = tipTool.GetComponent<Text>();
            canvasGroup = tipTool.GetComponent<CanvasGroup>();
            text2 = tipTool.transform.GetChild(1).GetComponent<Text>();
            rect = tipTool.GetComponent<RectTransform>();

            text1Contrast = tipToolContrast.GetComponent<Text>();
            canvasGroupContrast = tipToolContrast.GetComponent<CanvasGroup>();
            text2Contrast = tipToolContrast.transform.GetChild(1).GetComponent<Text>();
            rectContrast = tipToolContrast.GetComponent<RectTransform>();
        }
        private void UpdateTipShow()
        {
            if (isTipShow)
            {
                //Vector2 v;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out v);
                Vector3 v3 = Input.mousePosition;
                if (v3.x < Width / 2) transV.x = TipWidth / 2;
                else transV.x = -TipWidth / 2;
                if (v3.y < Height / 2) transV.y = TipHeight / 2;
                else transV.y = -TipHeight / 2;
                SetTipPosition(v3 + transV);
            }
            if (isTipContrastShow)
            {
                SetTipContrastPosition();
            }
            if (isDrag)
            {
                dragItem.SetPosition(Input.mousePosition);
            }

            if (isDrag && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //InventoryManage.instance.HideDrag();
            }
        }

        #region 显示和隐藏drag和tiptool
        public void ShowTip(string text, string textContrast = null)
        {
            isTipShow = true;

            text1.text = text;
            text2.text = text;
            canvasGroup.alpha = 1.0f;

            if (textContrast != null)
            {
                isTipContrastShow = true;
                text1Contrast.text = textContrast;
                text2Contrast.text = textContrast;
                canvasGroupContrast.alpha = 1.0f;
            }
        }

        public void HideTip()
        {
            isTipShow = false;

            canvasGroup.alpha = 0.0f;

            isTipContrastShow = false;
            canvasGroupContrast.alpha = 0;
        }

        public void SetTipPosition(Vector3 v3)
        {
            rect.position = v3;
        }

        public void SetTipContrastPosition()
        {
            if (rect.position.x < Width / 2) rect.position += new Vector3(TipContrastWidth, 0);
            rectContrast.localPosition = rect.localPosition - new Vector3(TipWidth, 0); // 使用局部坐标
        }   

        public void ShowDrag(Item item, int amount)
        {
            dragItem.Show(item, amount);
            isDrag = true;
        }

        public void HideDrag()
        {
            dragItem.Hide();
            isDrag = false;
        }
        #endregion
    }
}
