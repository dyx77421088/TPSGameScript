using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot.UI
{
    public class CanvasManager : MonoBehaviour
    {
        protected CanvasElement[] canvasElements;
        private void Awake()
        {
            canvasElements = GetComponentsInChildren<CanvasElement>();
            foreach (CanvasElement ce in canvasElements)
            {
                ce.gameObject.SetActive(true);
                ce.SubScribe(); // ¶©ÔÄ
                ce.gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            foreach (CanvasElement ce in canvasElements)
            {
                ce.UnSubScribe(); // È¡Ïû¶©ÔÄ
            }
        }
    }

}
