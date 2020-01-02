// Decompiled with JetBrains decompiler
// Type: HPointList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class HPointList : MonoBehaviour
{
  public Dictionary<int, List<HPoint>> lst;
  private HPoint[] HPoints;
  [SerializeField]
  private HPointList.AreaInfo[] Areas;

  public HPointList()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    this.lst = new Dictionary<int, List<HPoint>>();
    for (int index1 = 0; index1 < this.Areas.Length; ++index1)
    {
      int index2 = index1;
      this.HPoints = (HPoint[]) this.Areas[index2].HPoints.GetComponentsInChildren<HPoint>();
      foreach (Component hpoint in this.HPoints)
      {
        ParticleSystem componentInChildren = (ParticleSystem) hpoint.GetComponentInChildren<ParticleSystem>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null) && ((Component) componentInChildren).get_gameObject().get_activeSelf())
          ((Component) componentInChildren).get_gameObject().SetActive(false);
      }
      this.lst.Add(this.Areas[index2].Area, new List<HPoint>((IEnumerable<HPoint>) this.HPoints));
    }
  }

  [Serializable]
  private class AreaInfo
  {
    [Label("エリア")]
    public int Area;
    [Label("Hポイント")]
    public GameObject HPoints;
  }

  [Serializable]
  public class LoadInfo
  {
    public string Path;
    public string Name;
    public string Manifest;
  }
}
