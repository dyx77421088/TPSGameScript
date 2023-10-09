using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot.UI
{
    public abstract class CanvasElement : MonoBehaviour
    {
        [Range(0f, 10f)]
        public float showTime;
        [Range(0f, 10f)]
        public float hideTime;

        public bool IsHide { get; private set; }
        public bool IsShow { get; private set; }
        protected Coroutine showCorutine, hideCorutine;
        /// <summary>
        /// 订阅
        /// </summary>
        public virtual void SubScribe() { }
        public virtual void UnSubScribe() { }
        /// <summary>
        /// 一些事件完成
        /// </summary>
        protected virtual void StartShow() { }
        protected virtual void StartHide() { }
        protected virtual void FinishShow() { }
        protected virtual void FinishHide() { }

        protected void Show()
        {
            StopHideCoroutine();
            StopShowCoroutine();

            gameObject.SetActive(true);
            IsShow = true;
            IsHide = false;
            StartShow();
            showCorutine = DelayAction(showTime, FinishShow);
        }

        protected void Hide() 
        {
            if (!gameObject.activeSelf) return;

            StopHideCoroutine();
            StopShowCoroutine();

            StartHide();
            hideCorutine = DelayAction(hideTime, ()=>
            {
                FinishHide();
                gameObject.SetActive(false);
                IsHide = true;
                IsShow = false;
            });
        }


        /// <summary>
        /// 多少秒之后执行action
        /// </summary>
        private Coroutine DelayAction(float sec, Action action)
        {
            return StartCoroutine(DelayCoroutine(sec, action));
        }
        private IEnumerator DelayCoroutine(float sec, Action action)
        {
            yield return new WaitForSeconds(sec);
            action.Invoke();
        }

        private void StopShowCoroutine()
        {
            if (showCorutine != null)
            {
                StopCoroutine(showCorutine);
                showCorutine = null;
            }
        }
        private void StopHideCoroutine()
        {
            if (hideCorutine != null)
            {
                StopCoroutine(hideCorutine);
                hideCorutine = null;
            }
        }
    }
}
