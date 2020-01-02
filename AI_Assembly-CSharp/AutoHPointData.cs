// Decompiled with JetBrains decompiler
// Type: AutoHPointData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

public class AutoHPointData : SerializedScriptableObject
{
  public Dictionary<string, List<ValueTuple<int, Vector3>>> Points;

  public AutoHPointData()
  {
    base.\u002Ector();
  }

  public void Allocation(
    Dictionary<string, List<ValueTuple<int, Vector3>>> pointLists)
  {
    this.Points.Clear();
    using (Dictionary<string, List<ValueTuple<int, Vector3>>>.Enumerator enumerator = pointLists.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, List<ValueTuple<int, Vector3>>> current = enumerator.Current;
        this.Points.Add(current.Key, current.Value);
      }
    }
  }

  public void Allocation(
    Dictionary<string, List<int>> pointAreaIDLists,
    Dictionary<string, List<Vector3>> pointPosLists)
  {
    this.Points.Clear();
    foreach (KeyValuePair<string, List<int>> pointAreaIdList in pointAreaIDLists)
      this.Points.Add(pointAreaIdList.Key, new List<ValueTuple<int, Vector3>>());
    using (Dictionary<string, List<ValueTuple<int, Vector3>>>.Enumerator enumerator = this.Points.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, List<ValueTuple<int, Vector3>>> current = enumerator.Current;
        for (int index = 0; index < pointAreaIDLists[current.Key].Count; ++index)
          current.Value.Add(new ValueTuple<int, Vector3>(pointAreaIDLists[current.Key][index], pointPosLists[current.Key][index]));
      }
    }
  }

  public void Release()
  {
    this.Points = (Dictionary<string, List<ValueTuple<int, Vector3>>>) null;
    GC.Collect();
  }
}
