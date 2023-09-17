using System;
using System.Reflection;
using System.Linq;

using UnityEngine;

public class AutoLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadAutoLoadableObjects()
    {
        const string assemblyName = "Assembly-CSharp";

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.GetName().Name.Equals(assemblyName))
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(IAutoLoadable)))
                    {
                        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                    }
                }
            }
        }
    }
}
