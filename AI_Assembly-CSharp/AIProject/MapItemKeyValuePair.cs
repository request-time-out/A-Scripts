// Decompiled with JetBrains decompiler
// Type: AIProject.MapItemKeyValuePair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public class MapItemKeyValuePair
  {
    [SerializeField]
    private int _id;
    [SerializeField]
    private GameObject _itemObj;

    public int ID
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
      }
    }

    public GameObject ItemObj
    {
      get
      {
        return this._itemObj;
      }
      set
      {
        this._itemObj = value;
      }
    }
  }
}
