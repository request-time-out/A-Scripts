// Decompiled with JetBrains decompiler
// Type: AIProject.CollectionExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public static class CollectionExtensions
  {
    public static bool IsNullOrEmpty<T>(this T[] source)
    {
      return source == null || source.Length == 0;
    }

    public static bool IsNullOrEmpty<T>(this List<T> source)
    {
      return source == null || source.Count == 0;
    }

    public static bool IsNullOrEmpty<TKey, TSource>(this Dictionary<TKey, TSource> source)
    {
      return source == null || source.Count == 0;
    }

    public static bool IsNullOrEmpty<TKey, TSource>(this ReadOnlyDictionary<TKey, TSource> source)
    {
      return source == null || source.get_Count() == 0;
    }

    public static bool IsNullOrEmpty<T>(this Queue<T> source)
    {
      return source == null || source.Count == 0;
    }

    public static bool Exists<T>(this T[] source, Predicate<T> predicate)
    {
      foreach (T obj in source)
      {
        if (predicate(obj))
          return true;
      }
      return false;
    }

    public static bool Exists<T>(this List<T> source, Predicate<T> predicate)
    {
      foreach (T obj in source)
      {
        if (predicate(obj))
          return true;
      }
      return false;
    }

    public static bool Exists<TKey, TSource>(
      this Dictionary<TKey, TSource> source,
      Predicate<KeyValuePair<TKey, TSource>> predicate)
    {
      foreach (KeyValuePair<TKey, TSource> keyValuePair in source)
      {
        if (predicate(keyValuePair))
          return true;
      }
      return false;
    }

    public static bool Exists<T>(this Queue<T> source, Predicate<T> predicate)
    {
      foreach (T obj in source)
      {
        if (predicate(obj))
          return true;
      }
      return false;
    }

    public static T[] Range<T>(this T[] source, int start, int count)
    {
      if (start < 0 || count <= 0)
        return (T[]) null;
      T[] objArray = new T[count];
      for (int index = 0; index < count; ++index)
        objArray[index] = source[index + start];
      return objArray;
    }

    public static int Sum<T>(this T[] source, Func<T, int> selector)
    {
      if (source == null)
        return 0;
      int num = 0;
      foreach (T obj in source)
        num += selector(obj);
      return num;
    }

    public static T[] Shuffle<T>(this T[] source)
    {
      if (source == null)
        return (T[]) null;
      if (source.Length == 0)
        return new T[0];
      int length = source.Length;
      T[] objArray = new T[length];
      Array.Copy((Array) source, (Array) objArray, length);
      Random random = new Random();
      int index1 = length;
      while (1 < index1)
      {
        --index1;
        int index2 = random.Next(index1 + 1);
        T obj = objArray[index2];
        objArray[index2] = objArray[index1];
        objArray[index1] = obj;
      }
      return objArray;
    }

    public static T Pop<T>(this List<T> source)
    {
      if (source.IsNullOrEmpty<T>())
      {
        Debug.LogError((object) string.Format("List is Empty: {0}", (object) nameof (source)));
        return default (T);
      }
      T obj = source.FirstOrDefault<T>();
      source.RemoveAt(0);
      return obj;
    }

    public static void PushFront<T>(this List<T> source, T item)
    {
      if (source == null)
        Debug.LogException((Exception) new ArgumentNullException(nameof (source)));
      else
        source.Insert(0, item);
    }

    public static T[] Shuffle<T>(this List<T> source)
    {
      if (source.IsNullOrEmpty<T>())
        return (T[]) null;
      int count = source.Count;
      T[] objArray = new T[count];
      Array.Copy((Array) source.ToArray(), (Array) objArray, count);
      Random random = new Random();
      int index1 = count;
      while (1 < index1)
      {
        --index1;
        int index2 = random.Next(index1 + 1);
        T obj = objArray[index2];
        objArray[index2] = objArray[index1];
        objArray[index1] = obj;
      }
      return objArray;
    }

    public static List<T> Range<T>(this List<T> source, int start, int count)
    {
      if (start < 0 || count <= 0)
        return (List<T>) null;
      List<T> objList = new List<T>();
      for (int index = 0; index < count; ++index)
        objList[index] = source[index + start];
      return objList;
    }

    public static int Sum<T>(this List<T> source, Func<T, int> selector)
    {
      if (source == null)
        return 0;
      int num = 0;
      foreach (T obj in source)
        num += selector(obj);
      return num;
    }

    public static float Sum<T>(this List<T> source, Func<T, float> selector)
    {
      if (source == null)
        return 0.0f;
      float num = 0.0f;
      foreach (T obj in source)
        num += selector(obj);
      return num;
    }

    public static T GetElement<T>(this T[] source, int index)
    {
      if (source.IsNullOrEmpty<T>())
        return default (T);
      return index >= 0 && index < source.Length ? source[index] : default (T);
    }

    public static T GetElement<T>(this List<T> source, int index)
    {
      if (source.IsNullOrEmpty<T>())
        return default (T);
      return index >= 0 && index < source.Count ? source[index] : default (T);
    }

    public static T GetElement<T>(this ReadOnlyCollection<T> source, int index)
    {
      if (source.IsNullOrEmpty<T>())
        return default (T);
      return index >= 0 && index < source.Count ? source[index] : default (T);
    }

    public static KeyValuePair<TKey, TValue> Max<TKey, TValue>(
      this Dictionary<TKey, TValue> source,
      Func<KeyValuePair<TKey, TValue>, float> func)
    {
      float num1 = 0.0f;
      KeyValuePair<TKey, TValue> keyValuePair1 = new KeyValuePair<TKey, TValue>();
      foreach (KeyValuePair<TKey, TValue> keyValuePair2 in source)
      {
        float num2 = func(keyValuePair2);
        if ((double) num2 > (double) num1)
        {
          num1 = num2;
          keyValuePair1 = keyValuePair2;
        }
      }
      return keyValuePair1;
    }
  }
}
