using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TPSShoot;
using UnityEngine;

namespace TPSShoot
{
    public partial class Events
    {
        static Events()
        {
            FieldInfo[] fields = typeof(Events).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo field in fields)
            {
                // IsSubclassOf ����Ƿ�������������
                if (field.FieldType.IsSubclassOf(typeof(IEvent)))
                {
                    // ����ķ�������ֵ��������õĶ���Ϊ��̬�ĵ�һ����������Ϊnull
                    // Activator.CreateInstance ʵ��������,field.FieldType���������
                    field.SetValue(null, Activator.CreateInstance(field.FieldType, field.Name));
                }
            }
        }
    }
}
