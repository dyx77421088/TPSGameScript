using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour : MonoBehaviour
    {
        [Header("背包的一些属性")]
        [Tooltip("拖拽背包的速度")]public float dragSpeed = 8f;
        [Tooltip("背包打开时移动到的位置")]public Transform bagsMoveTo;

        private static PlayerBagBehaviour _instance;
        private bool _isOpenBag;
        private float width, height;

        public static PlayerBagBehaviour Instance { get { return _instance; } }
        public bool IsOpenBag { get => _isOpenBag; }
        private void Awake()
        {
            _instance = this;
            width = Screen.width;
            height = Screen.height;
            SubScribe();
        }
        private void Start()
        {
            if (bagsMoveTo == null)
            {
                GameObject go = new GameObject("BagsMoveTo");
                Instantiate(go);
                go.transform.parent = transform.parent;
                bagsMoveTo = go.transform;
                bagsMoveTo.position = new Vector3(0, 0, 0);
            }
        }
        private void Update()
        {
            if (width != Screen.width || height != Screen.height)
            {
                Debug.Log("改变了宽高");
                width = Screen.width;
                height = Screen.height;

                // 改变了宽高的监听
            }
        }

        private void OnDestroy()
        {
            UnSubScribe();
        }
        private void SubScribe()
        {
            Events.BagRequest += OnBagsRequest;
        }
        private void UnSubScribe()
        {
            Events.BagRequest -= OnBagsRequest;
        }

        /// <summary>
        /// 打开背包或关闭背包的请求
        /// </summary>
        private void OnBagsRequest()
        {
            if (!_isOpenBag)
            {
                OpenBag();
            }
            else
            {
                CloseBag();
            }
        }

        #region 打开和关闭背包
        private void OpenBag()
        {
            _isOpenBag = true;
            transform.DOMove(bagsMoveTo.position, 1);
            Events.PlayerOpenBag.Call();
        }

        private void CloseBag()
        {
            _isOpenBag = false;
            transform.DOMove(new Vector3(width * 2, -height * 2), 1);
            Events.PlayerCloseBag.Call();
        }

        #endregion
    }
}
