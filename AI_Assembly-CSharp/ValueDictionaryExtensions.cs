// Decompiled with JetBrains decompiler
// Type: ValueDictionaryExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

public static class ValueDictionaryExtensions
{
  public static ValueDictionary<TKey2, TValue> New<TKey1, TKey2, TValue>(
    this ValueDictionary<TKey1, TKey2, TValue> dictionary)
  {
    return new ValueDictionary<TKey2, TValue>();
  }

  public static ValueDictionary<TKey2, TKey3, TValue> New<TKey1, TKey2, TKey3, TValue>(
    this ValueDictionary<TKey1, TKey2, TKey3, TValue> dictionary)
  {
    return new ValueDictionary<TKey2, TKey3, TValue>();
  }

  public static ValueDictionary<TKey2, TKey3, TKey4, TValue> New<TKey1, TKey2, TKey3, TKey4, TValue>(
    this ValueDictionary<TKey1, TKey2, TKey3, TKey4, TValue> dictionary)
  {
    return new ValueDictionary<TKey2, TKey3, TKey4, TValue>();
  }

  public static ValueDictionary<TKey2, TKey3, TKey4, TKey5, TValue> New<TKey1, TKey2, TKey3, TKey4, TKey5, TValue>(
    this ValueDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TValue> dictionary)
  {
    return new ValueDictionary<TKey2, TKey3, TKey4, TKey5, TValue>();
  }

  public static ValueDictionary<TKey2, TKey3, TKey4, TKey5, TKey6, TValue> New<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue>(
    this ValueDictionary<TKey1, TKey2, TKey3, TKey4, TKey5, TKey6, TValue> dictionary)
  {
    return new ValueDictionary<TKey2, TKey3, TKey4, TKey5, TKey6, TValue>();
  }
}
