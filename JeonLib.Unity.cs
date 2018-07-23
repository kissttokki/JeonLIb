using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace JeonLib.Unity
{
    public class JeonLibUnity
    {
        public static void DrawRect(Vector2 _botleft, float _x, float _y, float _time = 3.402823E+38f, Color _colour = default(Color))
        {
            if (_colour == new Color())
            {
                _colour = Color.white;
            }
            Vector2 vector = _botleft + ((Vector2)(Vector2.up * _y));
            Vector2 vector2 = _botleft + ((Vector2)(Vector2.right * _x));
            Vector2 vector3 = (Vector2)((_botleft + (Vector2.right * _x)) + (Vector2.up * _y));
            Debug.DrawLine((Vector3)vector, (Vector3)vector3, _colour, _time);
            Debug.DrawLine((Vector3)_botleft, (Vector3)vector, _colour, _time);
            Debug.DrawLine((Vector3)_botleft, (Vector3)vector2, _colour, _time);
            Debug.DrawLine((Vector3)vector2, (Vector3)vector3, _colour, _time);
        }
    }
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;

        private void Awake()
        {
            Singleton<T>.instance = base.GetComponent<T>();
        }

        public static T GET
        {
            get
            {
                if (Singleton<T>.instance == null)
                {
                    Singleton<T>.instance = UnityEngine.Object.FindObjectOfType<T>();
                    if (Singleton<T>.instance == null)
                    {
                        object[] args = new object[] { typeof(T).ToString() };
                        Debug.LogFormat("싱글톤 에러 : {0} 을 못찾음.", args);
                        return default(T);
                    }
                }
                return Singleton<T>.instance;
            }
        }
    }
}
