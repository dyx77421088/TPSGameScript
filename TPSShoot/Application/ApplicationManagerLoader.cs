using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TPSShoot
{
    public class ApplicationManagerLoader : IAutoLoadable
    {
        static ApplicationManagerLoader()
        {
            new GameObject(typeof(ApplicationManager).Name, typeof(ApplicationManager));
        }
    }
}
