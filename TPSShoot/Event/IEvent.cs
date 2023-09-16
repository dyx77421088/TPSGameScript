using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public abstract class IEvent
    {
        protected readonly string name;

        protected IEvent(string name)
        {
            this.name = name;
        }

        protected void ThrowNameException()
        {
            UnityEngine.Debug.LogException(new System.Exception("提交了相同的event: " + name));
        }
    }
}
