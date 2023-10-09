using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerInfoTipUI : CanvasElement
    {
        [Header("出现的位置信息")]
        public RectTransform centerStart;
        public RectTransform centerFrom;
        public RectTransform leftStart;
        public RectTransform leftFrom;
        [Header("一些prefabs")]
        public GameObject textPrefabs;

        private List<string> listLeft = new List<string>();
        private List<string> listCenter = new List<string>();
        private Coroutine leftCoroutine;
        private Coroutine centerCoroutine;

        public override void SubScribe()
        {
            Events.GamePause += Hide;
            //Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;

            //Events.PlayerCloseBag += Show;
            Events.ApplicationLoaded += Show;
            Events.PlayerLoaded += Show;
            Events.GameResume += Show;

            Events.PlayerInfoTipShow += OnInfoTipShow;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            //Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;

            //Events.PlayerCloseBag -= Show;
            Events.ApplicationLoaded -= Show;
            Events.PlayerLoaded -= Show;
            Events.GameResume -= Show;

            Events.PlayerInfoTipShow -= OnInfoTipShow;
        }

        protected override void StartShow()
        {
            if (leftCoroutine == null)
            {
                leftCoroutine = StartCoroutine(WaitSecToLeft(0.5f));
            }
            if (centerCoroutine == null)
            {
                centerCoroutine = StartCoroutine(WaitSecToCenter(0.5f));
            }
        }

        protected override void StartHide()
        {
            if (leftCoroutine != null)
            {
                StopCoroutine(leftCoroutine);
                leftCoroutine = null;
            }
            if (centerCoroutine != null)
            {
                StopCoroutine(centerCoroutine);
                centerCoroutine = null;
            }
        }


        private void OnInfoTipShow(string text, PlayerInfoTipPoint point)
        {
            if (point == PlayerInfoTipPoint.Left) listLeft.Add(text);
            if (point == PlayerInfoTipPoint.Center) listCenter.Add(text);
        }
        private IEnumerator WaitSecToLeft(float sec)
        {
            while (PlayerBehaviour.Instance.IsAlive)
            {
                yield return new WaitForSeconds(sec);
                yield return new WaitUntil(()=> listLeft.Count != 0);
                OnShowTip(listLeft[0], PlayerInfoTipPoint.Left);
                listLeft.RemoveAt(0);
            }
        }
        private IEnumerator WaitSecToCenter(float sec)
        {
            while (PlayerBehaviour.Instance.IsAlive)
            {
                yield return new WaitForSeconds(sec);
                yield return new WaitUntil(() => listCenter.Count != 0);
                OnShowTip(listCenter[0], PlayerInfoTipPoint.Center);
                listCenter.RemoveAt(0);
            }
        }

        /// <summary>
        /// 显示text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="point"></param>
        private void OnShowTip(string text, PlayerInfoTipPoint point)
        {
            RectTransform start = point == PlayerInfoTipPoint.Left ? leftStart : centerStart;
            RectTransform from = point == PlayerInfoTipPoint.Left ? leftFrom : centerFrom;

            GameObject go = Instantiate(textPrefabs, start.position, start.rotation);
            go.transform.SetParent(transform);
            go.GetComponent<Text>().text = text;

            go.transform.DOMove(from.position, 3).OnComplete(() =>
            {
                Destroy(go, 1.5f);
            });
        }

        public enum PlayerInfoTipPoint
        {
            Left,
            Center,
        }
    }
}
