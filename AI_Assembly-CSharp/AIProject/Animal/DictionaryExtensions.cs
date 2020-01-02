// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.DictionaryExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;
using System.Collections.Generic;

namespace AIProject.Animal
{
  public static class DictionaryExtensions
  {
    public static KeyValuePair<T1, T2> Rand<T1, T2>(this Dictionary<T1, T2> source)
    {
      if (((IReadOnlyDictionary<T1, T2>) source).IsNullOrEmpty<T1, T2>())
        return new KeyValuePair<T1, T2>();
      List<KeyValuePair<T1, T2>> keyValuePairList = ListPool<KeyValuePair<T1, T2>>.Get();
      foreach (KeyValuePair<T1, T2> keyValuePair in source)
        keyValuePairList.Add(keyValuePair);
      KeyValuePair<T1, T2> keyValuePair1 = keyValuePairList.Rand<KeyValuePair<T1, T2>>();
      ListPool<KeyValuePair<T1, T2>>.Release(keyValuePairList);
      return keyValuePair1;
    }

    public static T2 RandValue<T1, T2>(this Dictionary<T1, T2> source)
    {
      if (((IReadOnlyDictionary<T1, T2>) source).IsNullOrEmpty<T1, T2>())
        return default (T2);
      List<KeyValuePair<T1, T2>> keyValuePairList = ListPool<KeyValuePair<T1, T2>>.Get();
      foreach (KeyValuePair<T1, T2> keyValuePair in source)
        keyValuePairList.Add(keyValuePair);
      T2 obj = keyValuePairList.Rand<KeyValuePair<T1, T2>>().Value;
      ListPool<KeyValuePair<T1, T2>>.Release(keyValuePairList);
      return obj;
    }

    public static T1 RandKey<T1, T2>(this Dictionary<T1, T2> source)
    {
      if (((IReadOnlyDictionary<T1, T2>) source).IsNullOrEmpty<T1, T2>())
        return default (T1);
      List<KeyValuePair<T1, T2>> keyValuePairList = ListPool<KeyValuePair<T1, T2>>.Get();
      foreach (KeyValuePair<T1, T2> keyValuePair in source)
        keyValuePairList.Add(keyValuePair);
      T1 key = keyValuePairList.Rand<KeyValuePair<T1, T2>>().Key;
      ListPool<KeyValuePair<T1, T2>>.Release(keyValuePairList);
      return key;
    }

    public static KeyValuePair<T1, T2> GetPair<T1, T2>(
      this Dictionary<T1, T2> source,
      T1 key)
    {
      T2 obj;
      source.TryGetValue(key, out obj);
      return new KeyValuePair<T1, T2>(key, obj);
    }

    public static bool ActiveInState(this KeyValuePair<int, AnimalPlayState> source)
    {
      return source.Value != null && 0 <= source.Key && source.Value.MainStateInfo.ActiveInState;
    }

    public static bool ActiveOutState(this KeyValuePair<int, AnimalPlayState> source)
    {
      return source.Value != null && 0 <= source.Key && source.Value.MainStateInfo.ActiveOutState;
    }

    public static float AddValue<T1>(this Dictionary<T1, float> source, T1 key, float add)
    {
      float num = 0.0f;
      if (source.TryGetValue(key, out num))
        source[key] = (num += add);
      return num;
    }

    public static int AddValue<T1>(this Dictionary<T1, int> source, T1 key, int add)
    {
      int num = 0;
      if (source.TryGetValue(key, out num))
        source[key] = (num += add);
      return num;
    }

    public static bool TryAddValue<T1>(
      this Dictionary<T1, float> source,
      T1 key,
      float add,
      out float get)
    {
      get = 0.0f;
      if (!source.TryGetValue(key, out get))
        return false;
      source[key] = (get += add);
      return true;
    }

    public static bool TryAddValue<T1>(
      this Dictionary<T1, int> source,
      T1 key,
      int add,
      out int get)
    {
      get = 0;
      if (!source.TryGetValue(key, out get))
        return false;
      source[key] = (get += add);
      return true;
    }

    public static bool AddNonContains<T1, T2>(this Dictionary<T1, T2> source, T1 key, T2 value)
    {
      if (source == null || source.ContainsKey(key))
        return false;
      source[key] = value;
      return true;
    }
  }
}
