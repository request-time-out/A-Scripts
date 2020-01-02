// Decompiled with JetBrains decompiler
// Type: MinimapNavimesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MinimapNavimesh : MonoBehaviour
{
  [SerializeField]
  private int MapID;
  [SerializeField]
  private MinimapNavimesh.Map mapNaviMesh;
  public Dictionary<int, bool> areaGroupActive;
  private Dictionary<int, MinimapNavimesh.AreaGroupInfo> table;

  public MinimapNavimesh()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    this.areaGroupActive = new Dictionary<int, bool>();
    if (!Singleton<Resources>.Instance.Map.AreaGroupTable.TryGetValue(this.MapID, out this.table))
      this.table = new Dictionary<int, MinimapNavimesh.AreaGroupInfo>();
    foreach (KeyValuePair<int, MinimapNavimesh.AreaGroupInfo> keyValuePair in this.table)
    {
      bool flag = true;
      if (keyValuePair.Value.OpenStateID[0] != -1)
      {
        for (int index1 = 0; index1 < keyValuePair.Value.OpenStateID.Count; ++index1)
        {
          int index2 = keyValuePair.Value.OpenStateID[index1];
          flag &= Singleton<Game>.Instance.Environment.AreaOpenState[index2];
        }
      }
      this.areaGroupActive.Add(keyValuePair.Key, flag);
    }
    this.Reflesh();
  }

  public void Reflesh()
  {
    if (this.MapID != Singleton<Manager.Map>.Instance.MapID)
      return;
    if ((this.table == null || this.table.Count == 0) && !Singleton<Resources>.Instance.Map.AreaGroupTable.TryGetValue(this.MapID, out this.table))
      this.table = new Dictionary<int, MinimapNavimesh.AreaGroupInfo>();
    foreach (KeyValuePair<int, MinimapNavimesh.AreaGroupInfo> keyValuePair in this.table)
    {
      bool flag = true;
      if (keyValuePair.Value.OpenStateID[0] != -1)
      {
        for (int index1 = 0; index1 < keyValuePair.Value.OpenStateID.Count; ++index1)
        {
          int index2 = keyValuePair.Value.OpenStateID[index1];
          flag &= Singleton<Game>.Instance.Environment.AreaOpenState[index2];
        }
      }
      this.areaGroupActive[keyValuePair.Key] = flag;
    }
    this.mapNaviMesh.VisibleSet(this.areaGroupActive);
  }

  [Serializable]
  private class Map
  {
    [SerializeField]
    private MinimapNavimesh.AreaNaviInfo[] areaNaviMeshs;

    public void VisibleSet(Dictionary<int, bool> areaActive)
    {
      foreach (MinimapNavimesh.AreaNaviInfo areaNaviInfo in this.areaNaviMeshs)
      {
        if (areaActive.ContainsKey(areaNaviInfo.areaGroupID))
          areaNaviInfo.AreaVisibleSet(areaActive[areaNaviInfo.areaGroupID]);
      }
      foreach (MinimapNavimesh.AreaNaviInfo areaNaviInfo in this.areaNaviMeshs)
      {
        if (!areaActive.ContainsKey(areaNaviInfo.areaGroupID))
          areaActive.Add(areaNaviInfo.areaGroupID, areaNaviInfo.Active);
        else
          areaActive[areaNaviInfo.areaGroupID] = areaNaviInfo.Active;
      }
    }
  }

  [Serializable]
  private class AreaNaviInfo
  {
    public bool Active = true;
    public int areaGroupID;
    [SerializeField]
    private GameObject[] naviMeshs;

    public void AreaVisibleSet(bool _active)
    {
      if (this.Active == _active)
        return;
      foreach (GameObject gameObject in this.naviMeshs)
        gameObject.SetActive(_active);
      this.Active = _active;
    }
  }

  public class AreaGroupInfo
  {
    public List<int> areaID = new List<int>();
    public List<int> OpenStateID = new List<int>();
  }
}
