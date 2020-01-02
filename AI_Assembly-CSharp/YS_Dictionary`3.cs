// Decompiled with JetBrains decompiler
// Type: YS_Dictionary`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class YS_Dictionary<TKey, TValue, TPair> where TPair : YS_KeyAndValue<TKey, TValue>, new()
{
  [SerializeField]
  protected List<TPair> list;
  protected Dictionary<TKey, TValue> table;

  public YS_Dictionary()
  {
    this.list = new List<TPair>();
  }

  public Dictionary<TKey, TValue> GetTable()
  {
    if (this.table == null)
      this.table = YS_Dictionary<TKey, TValue, TPair>.ConvertListToDictionary(this.list);
    return this.table;
  }

  public TValue GetValue(TKey key)
  {
    return this.GetTable().Keys.Contains<TKey>(key) ? this.GetTable()[key] : default (TValue);
  }

  public void SetValue(TKey key, TValue value)
  {
    if (this.GetTable().Keys.Contains<TKey>(key))
      this.table[key] = value;
    else
      this.table.Add(key, value);
  }

  public void Reset()
  {
    this.table = new Dictionary<TKey, TValue>();
    this.list = new List<TPair>();
  }

  public void Apply()
  {
    this.list = YS_Dictionary<TKey, TValue, TPair>.ConvertDictionaryToList(this.table);
  }

  public int Length
  {
    get
    {
      return this.list == null ? 0 : this.list.Count;
    }
  }

  private static Dictionary<TKey, TValue> ConvertListToDictionary(List<TPair> list)
  {
    Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
    foreach (TPair pair in list)
    {
      YS_KeyAndValue<TKey, TValue> ysKeyAndValue = (YS_KeyAndValue<TKey, TValue>) pair;
      dictionary.Add(ysKeyAndValue.Key, ysKeyAndValue.Value);
    }
    return dictionary;
  }

  private static List<TPair> ConvertDictionaryToList(Dictionary<TKey, TValue> table)
  {
    List<TPair> pairList = new List<TPair>();
    if (table != null)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in table)
      {
        TPair pair = new TPair();
        pair.Key = keyValuePair.Key;
        pair.Value = keyValuePair.Value;
        pairList.Add(pair);
      }
    }
    return pairList;
  }
}
