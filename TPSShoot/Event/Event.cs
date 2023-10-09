using System;
using System.Collections.Generic;

namespace TPSShoot
{
    public partial class Event : IEvent
    {
        // 订阅
        private readonly List<Action> subscribers;
        public Event(string name) : base(name)
        {
            subscribers = new List<Action>();
        }

        /// <summary>
        /// 订阅的活动依次执行
        /// </summary>
        public void Call()
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                subscribers[i].Invoke(); // 执行
            }
        }

        public static Event operator +(Event e, Action action)
        {
            e.CheckName(action);
            e.subscribers.Add(action);
            return e;
        }

        public static Event operator -(Event e, Action action)
        {
            e.subscribers.Remove(action);
            return e;
        }

        private void CheckName(Action action)
        {
            // 如果已经包含了这个名字的action就抛出
            if (subscribers.Contains(action))
            {
                ThrowNameException();
            }
        }
    }
}
