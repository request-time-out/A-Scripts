// Decompiled with JetBrains decompiler
// Type: AIProject.AgentAdvPresentInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class AgentAdvPresentInfo : ScriptableObject
  {
    public List<AgentAdvPresentInfo.Param> param;

    public AgentAdvPresentInfo()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public List<AgentAdvPresentInfo.ItemData> ItemData = new List<AgentAdvPresentInfo.ItemData>();
      public int ID;
      public int ItemID;

      public Param()
      {
      }

      public Param(AgentAdvPresentInfo.Param src)
      {
        this.ID = src.ID;
        this.ItemID = src.ItemID;
        this.ItemData = src.ItemData.ToList<AgentAdvPresentInfo.ItemData>();
      }
    }

    [Serializable]
    public class ItemData
    {
      public int nameHash = -1;
    }
  }
}
