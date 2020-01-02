// Decompiled with JetBrains decompiler
// Type: Serialization`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
  [SerializeField]
  private List<TKey> keys;
  [SerializeField]
  private List<TValue> values;
  private Dictionary<TKey, TValue> target;

  public Serialization(Dictionary<TKey, TValue> target)
  {
    this.target = target;
  }

  public Dictionary<TKey, TValue> ToDictionary()
  {
    return this.target;
  }

  public void OnBeforeSerialize()
  {
    this.keys = new List<TKey>((IEnumerable<TKey>) this.target.Keys);
    this.values = new List<TValue>((IEnumerable<TValue>) this.target.Values);
  }

  public void OnAfterDeserialize()
  {
    int capacity = Math.Min(this.keys.Count, this.values.Count);
    this.target = new Dictionary<TKey, TValue>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.target.Add(this.keys[index], this.values[index]);
  }
}
