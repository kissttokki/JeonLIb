using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;


public static class ExtentionGlobal
{
    /// <summary>
    /// 해당 콜렉션의 요소들을 뽑아내어 Action 루틴을 실행
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_col">콜렉션</param>
    /// <param name="_do">Do something</param>
    public static void AllDo<T>(this IEnumerable<T> _col, Action<T> _do)
    {
        if (_col != null)
        {
            T[] localArray = _col.ToArray<T>();
            for (int i = localArray.Length - 1; i >= 0; i--)
            {
                _do(localArray[i]);
            }
        }
    }
    



    /// <summary>
    /// Dictionary 벨류값이 disposable이면 dispose시킴과 동시에 Dictionary를 비움
    /// </summary>
    /// <param name="target"></param>
    public static void DisposeAndClearDic(this IDictionary<object, IDisposable> target)
    {
        foreach (KeyValuePair<object, IDisposable> pair in target)
        {
            pair.Value.Dispose();
        }
        target.Clear();
    }

   /// <summary>
   /// Dispose하고 null값 할당
   /// </summary>
   /// <param name="obj"></param>
    public static void DisposeOnNULL(this IDisposable obj)
    {
        if (obj != null)
        {
            obj.Dispose();
            obj = null;
        }
    }


    /// <summary>
    /// 자식 객체들을 List로 받아옴
    /// </summary>
    /// <param name="tr"></param>
    /// <returns></returns>
    public static List<Transform> GetChildAll(this Transform tr)
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < tr.childCount; i++)
        {
            list.Add(tr.GetChild(i));
        }
        return list;
    }

    /// <summary>
    /// 주로 확률 검사할때 쓰는 메소드
    /// 0~100의 범위일때 _num는 확률이 된다.
    /// e.g) _num = 20, maximumValue = 100이면 True가 나올 확률 20%
    /// </summary>
    /// <param name="_num"></param>
    /// <param name="maximumValue"></param>
    /// <returns></returns>
    public static bool getRandomBool(this int _num, int maximumValue = 100)
    {
        return (UnityEngine.Random.Range(0, maximumValue) <= _num);
    }

    /// <summary>
    /// 주로 확률 검사할때 쓰는 메소드 
    /// 0~1.0f의 범위일때 _num는 확률이 된다.
    /// e.g) _num = 0.2f, maximumValue = 1.0f이면 True가 나올 확률 20%
    /// </summary>
    /// <param name="_num"></param>
    /// <param name="maximumValue"></param>
    /// <returns></returns>
    public static bool getRandomBool(this float _num,float maximumValue = 1.0f)
    {
        return (UnityEngine.Random.Range(0f, maximumValue) <= _num);
    }



    /// <summary>
    /// Linq 최적화 (Any)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool JAny<T>(this IEnumerable<T> source)
    {
        return (source.ToArray<T>().Length != 0);
    }

    /// <summary>
    /// Linq 최적화 람다식 용(Any)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static bool JAny<T>(this IEnumerable<T> source, Func<T, bool> a)
    {
        T[] localArray = source.ToArray<T>();
        for (int i = 0; i < localArray.Length; i++)
        {
            if (a(localArray[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Linq 최적화 (First)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T JFirst<T>(this IEnumerable<T> source)
    {
        T[] localArray = source.ToArray<T>();
        if (localArray.Length <= 0)
        {
            throw new ArgumentNullException();
        }
        return localArray[0];
    }

    /// <summary>
    /// Linq 최적화 람다식 용(First)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static T JFirst<T>(this IEnumerable<T> source, Func<T, bool> a)
    {
        T[] localArray = source.ToArray<T>();
        for (int i = 0; i < localArray.Length; i++)
        {
            if (a(localArray[i]))
            {
                return localArray[i];
            }
        }
        throw new ArgumentNullException();
    }

  

    public static T Next<T>(this T src) where T: struct, IConvertible
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException(string.Format("Argumnent {0} is not an Enum", typeof(T).FullName));
        }
        T[] values = (T[]) Enum.GetValues(src.GetType());
        int index = Array.IndexOf<T>(values, src) + 1;
        return ((values.Length == index) ? values[0] : values[index]);
    }

 
    /// <summary>
    /// TypeChange
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T ToChangeType<T>(this object obj)
    {
        return (T) Convert.ChangeType(obj, typeof(T));
    }

    /// <summary>
    /// ToEnum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this object value)
    {
        return (T) Enum.Parse(typeof(T), value.ToString(), true);
    }

    /// <summary>
    /// ToFloat
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static float ToFloat(this object obj)
    {
        float num;
        try
        {
            num = (float) Convert.ToDouble(obj);
        }
        catch
        {
            Debug.LogError("FormatException: Input string was not in the correct format DATA : " + obj.ToString());
            throw new FormatException();
        }
        return num;
    }

    /// <summary>
    /// ToInt32
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int ToInt32(this object obj)
    {
        int num;
        try
        {
            num = Convert.ToInt32(obj);
        }
        catch
        {
            Debug.LogError("FormatException: Input string was not in the correct format DATA : " + obj.ToString());
            throw new FormatException();
        }
        return num;
    }


    /// <summary>
    /// Safely Invoke
    /// </summary>
    /// <param name="_action"></param>
    public static void SafeInvoke(this Action _action)
    {
        if (_action == null)
            return;
        _action();
    }
    /// <summary>
    /// Safely Invoke
    /// </summary>
    /// <param name="_action"></param>
    public static void SafeInvoke<T>(this Action<T> _action,T _data)
    {
        if (_action == null)
            return;

        _action(_data);
    }
    /// <summary>
    /// Safely Invoke
    /// </summary>
    /// <param name="_action"></param>
    public static void SafeInvoke<T,T1>(this Action<T,T1> _act, T _data, T1 _data2)
    {
        if (_act == null)
            return;

        _act(_data,_data2);
    }




    #region Between Part
    /// <summary>
    /// 수가 해당 범위에 있는지 확인
    /// </summary>
    /// <param name="_number">수</param>
    /// <param name="_min">최소값</param>
    /// <param name="_max">최대값</param>
    /// <param name="_includeMinimum">최소값 포함?</param>
    /// <param name="_includeMaximum">최대값 포함?</param>
    /// <returns></returns>
    public static bool Between(this int _number, int _min, int _max, bool _includeMinimum = true, bool _includeMaximum = true)
    {
        if (_includeMinimum & _includeMaximum)
        {
            return ((_number >= _min) && (_number <= _max));
        }
        if (_includeMinimum)
        {
            return ((_number >= _min) && (_number < _max));
        }
        if (_includeMaximum)
        {
            return ((_number > _min) && (_number <= _max));
        }
        return ((_number > _min) && (_number < _max));
    }
    public static bool Between(this int _number, float _min, float _max, bool _includeMinimum = true, bool _includeMaximum = true)
    {
        if (_includeMinimum & _includeMaximum)
        {
            return ((_number >= _min) && (_number <= _max));
        }
        if (_includeMinimum)
        {
            return ((_number >= _min) && (_number < _max));
        }
        if (_includeMaximum)
        {
            return ((_number > _min) && (_number <= _max));
        }
        return ((_number > _min) && (_number < _max));
    }

    public static bool Between(this float _number, float _min, float _max, bool _includeMinimum = true, bool _includeMaximum = true)
    {
        if (_includeMinimum & _includeMaximum)
        {
            return ((_number >= _min) && (_number <= _max));
        }
        if (_includeMinimum)
        {
            return ((_number >= _min) && (_number < _max));
        }
        if (_includeMaximum)
        {
            return ((_number > _min) && (_number <= _max));
        }
        return ((_number > _min) && (_number < _max));
    }
    #endregion

}
