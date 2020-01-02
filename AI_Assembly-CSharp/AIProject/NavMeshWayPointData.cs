// Decompiled with JetBrains decompiler
// Type: AIProject.NavMeshWayPointData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace AIProject
{
  public class NavMeshWayPointData : SerializedScriptableObject
  {
    [SerializeField]
    [DisableInPlayMode]
    private int _mapID;
    [SerializeField]
    [DisableInPlayMode]
    private int _areaID;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private Vector3[] _wayData;

    public NavMeshWayPointData()
    {
      base.\u002Ector();
    }

    public int MapID
    {
      get
      {
        return this._mapID;
      }
      set
      {
        this._mapID = value;
      }
    }

    public int AreaID
    {
      get
      {
        return this._areaID;
      }
    }

    public Vector3[] Points
    {
      get
      {
        return this._wayData;
      }
    }

    public void Allocation(Vector3[] positions)
    {
      this._wayData = new Vector3[positions.Length];
      for (int index = 0; index < positions.Length; ++index)
        this._wayData[index] = positions[index];
    }

    public void Release()
    {
      this._wayData = (Vector3[]) null;
      GC.Collect();
    }
  }
}
