using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace JeonLib
{
    public static class JeonLib
    {
        public static void ClassCopy<T, T1>(ref T TargetClass, ref T1 BaseClass)
        {
            System.Type type = typeof(T);
            System.Type type2 = typeof(T1);
            FieldInfo[] fields = type2.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                object obj2 = type2.GetField(fields[i].Name).GetValue((T1)BaseClass);
                try
                {
                    type.GetField(fields[i].Name).SetValue((T)TargetClass, obj2);
                }
                catch
                {
                    object[] args = new object[] { fields[i].Name };
                    Debug.LogWarningFormat("데이터 Copy 실패 - {0}", args);
                }
            }
        }

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }
            if (source == null)
            {
                return default(T);
            }
            IFormatter formatter = new BinaryFormatter();
            Stream serializationStream = new MemoryStream();
            using (serializationStream)
            {
                formatter.Serialize(serializationStream, source);
                serializationStream.Seek(0L, SeekOrigin.Begin);
                return (T)formatter.Deserialize(serializationStream);
            }
        }
        public static bool GetBoolWithStrings(string str1, params string[] str2)
        {
            for (int i = 0; i < str2.Length; i++)
            {
                if (str1.Equals(str2[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetNames<T>(ref T expr)
        {
            FieldInfo[] fields = expr.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            List<string> list = new List<string>();
            for (int i = 0; i < fields.Length; i++)
            {
                list.Add(fields[i].Name);
            }
            return list;
        }

        public static void SetCollection<T>(ref T target, string fieldname, object value)
        {
            try
            {
                string str = value.ToString();
                if (!str.Equals("-"))
                {
                    FieldInfo field = typeof(T).GetField(fieldname);
                    if (field.FieldType.IsEnum)
                    {
                        value = Enum.Parse(field.FieldType, str);
                    }
                    field.SetValue((T)target, Convert.ChangeType(value, field.FieldType));
                }
            }
            catch (Exception exception)
            {
                object[] args = new object[] { fieldname, value.ToString(), exception };
                Debug.LogErrorFormat("{0}를 찾을 수 없습니다. (값:{1}) \r\n{2}", args);
            }
        }
    }
}

