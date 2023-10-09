using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TPSShoot
{
    public partial class Event<T1,T2> : IEvent
    {
        private readonly List<Action<T1, T2>> subscribers;
        public Event(string name) : base(name)
        {
            subscribers = new List<Action<T1, T2>>();
        }

        /// <summary>
        /// 订阅的活动依次执行
        /// </summary>
        public void Call(T1 pram1, T2 pram2)
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                subscribers[i].Invoke(pram1, pram2); // 执行
            }
        }

        public static Event<T1, T2> operator +(Event<T1, T2> e, Action<T1, T2> action)
        {
            e.CheckName(action);
            e.subscribers.Add(action);
            return e;
        }

        public static Event<T1, T2> operator -(Event<T1, T2> e, Action<T1, T2> action)
        {
            e.subscribers.Remove(action);
            return e;
        }

        private void CheckName(Action<T1, T2> action)
        {
            // 如果已经包含了这个名字的action就抛出
            if (subscribers.Contains(action))
            {
                ThrowNameException();
            }
        }
    }
}
