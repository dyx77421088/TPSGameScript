using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TPSShoot
{
    public partial class Event<T> : IEvent
    {
        private readonly List<Action<T>> subscribers;
        public Event(string name) : base(name)
        {
            subscribers = new List<Action<T>>();
        }

        /// <summary>
        /// 订阅的活动依次执行
        /// </summary>
        public void Call(T pram)
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                subscribers[i].Invoke(pram); // 执行
            }
        }

        public static Event<T> operator +(Event<T> e, Action<T> action)
        {
            e.CheckName(action);
            e.subscribers.Add(action);
            return e;
        }

        public static Event<T> operator -(Event<T> e, Action<T> action)
        {
            e.subscribers.Remove(action);
            return e;
        }

        private void CheckName(Action<T> action)
        {
            // 如果已经包含了这个名字的action就抛出
            if (subscribers.Contains(action))
            {
                ThrowNameException();
            }
        }
    }
}
