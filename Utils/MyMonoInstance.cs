using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMonoInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    public virtual void Awake()
    {
        instance = this as T;
    }
}
