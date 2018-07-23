using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace JeonLib.Unity
{
    public static class Extention
    {
        /// <summary>
        /// log 간략화
        /// </summary>
        /// <param name="obj"></param>
        public static void Log(this object obj)
        {
            Debug.Log(obj);
        }

        /// <summary>
        /// log 간략화
        /// </summary>
        /// <param name="obj"></param>
        public static void LogError(this object obj)
        {
            Debug.LogError(obj);
        }

        /// <summary>
        /// log 간략화
        /// </summary>
        /// <param name="obj"></param>
        public static void LogWarning(this object obj)
        {
            Debug.LogWarning(obj);
        }

        /// <summary>
        /// Vector3.Set 확장
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="X"></param>
        public static void Set(this Vector3 vec, float X)
        {
            vec = new Vector3(X, vec.y, vec.z);
        }

        /// <summary>
        /// Vector3.Set 확장
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void Set(this Vector3 vec, float X, float Y)
        {
            vec = new Vector3(X, Y, vec.z);
        }
        /// <summary>
        /// 해당 트랜스폼의 자식객체들을 전부 삭제
        /// </summary>
        /// <param name="_tr"></param>
        public static void ClearChild(this Transform _tr)
        {
            for (int i = 0; i < _tr.childCount; i++)
            {
                UnityEngine.Object.Destroy(_tr.GetChild(i).gameObject);
            }
            _tr.DetachChildren();
        }

        /// <summary>
        /// 콜렉션 디버그 로깅 메소드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void ShowThis<T>(this T t)
        {
            System.Type type = t.GetType();
            if (type.IsArray)
            {
                StringBuilder builder = new StringBuilder();
                foreach (object obj2 in t as Array)
                {
                    builder.Append(obj2);
                    builder.Append(" ");
                }
                Debug.Log(builder.ToString());
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    foreach (object obj3 in (t as IDictionary).Keys)
                    {
                        object[] args = new object[] { obj3, (t as IDictionary)[obj3] };
                        Debug.LogFormat("Dictionary : ({0} : {1})", args);
                    }
                }
                else if (type is IEnumerable)
                {
                    int num = 0;
                    foreach (object obj4 in t as IEnumerable)
                    {
                        object[] objArray2 = new object[] { num, obj4 };
                        Debug.LogFormat("IEnumerable : ({0} : {1})", objArray2);
                        num++;
                    }
                }
                else if (type is ICollection)
                {
                    int num2 = 0;
                    foreach (object obj5 in t as ICollection)
                    {
                        object[] objArray3 = new object[] { num2, obj5 };
                        Debug.LogFormat("Collection : ({0} : {1})", objArray3);
                        num2++;
                    }
                }
            }
            else
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                List<string> list = new List<string>();
                for (int i = 0; i < fields.Length; i++)
                {
                    list.Add(fields[i].Name);
                }
                foreach (string str in list)
                {
                    try
                    {
                        object[] objArray4 = new object[] { str, type.GetField(str).GetValue(t) };
                        Debug.LogFormat("{0} : {1}", objArray4);
                    }
                    catch
                    {
                        if (type.GetField(str).FieldType is IDictionary)
                        {
                            foreach (object obj6 in (t as IDictionary).Keys)
                            {
                                object[] objArray6 = new object[] { obj6, (t as IDictionary)[obj6] };
                                Debug.LogFormat("Dictonary : ({0} : {1})", objArray6);
                            }
                        }
                        else if (type.GetField(str).FieldType is IList)
                        {
                            int num4 = 0;
                            foreach (object obj7 in t as IList)
                            {
                                object[] objArray7 = new object[] { num4, obj7 };
                                Debug.LogFormat("IEnumerable : ({0} : {1})", objArray7);
                                num4++;
                            }
                        }
                        else if (type.GetField(str).FieldType is ICollection)
                        {
                            int num5 = 0;
                            foreach (object obj8 in t as ICollection)
                            {
                                object[] objArray8 = new object[] { num5, obj8 };
                                Debug.LogFormat("Collection : ({0} : {1})", objArray8);
                                num5++;
                            }
                        }
                        object[] objArray5 = new object[] { type, str };
                        Debug.LogFormat("테스트 에러 : {0} {1}", objArray5);
                    }
                }
            }
        }



    }
}

