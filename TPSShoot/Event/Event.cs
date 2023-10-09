using System;
using System.Collections.Generic;

namespace TPSShoot
{
    public partial class Event : IEvent
    {
        // ����
        private readonly List<Action> subscribers;
        public Event(string name) : base(name)
        {
            subscribers = new List<Action>();
        }

        /// <summary>
        /// ���ĵĻ����ִ��
        /// </summary>
        public void Call()
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                subscribers[i].Invoke(); // ִ��
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
            // ����Ѿ�������������ֵ�action���׳�
            if (subscribers.Contains(action))
            {
                ThrowNameException();
            }
        }
    }
}
