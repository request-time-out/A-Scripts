// Decompiled with JetBrains decompiler
// Type: AIProject.AgentAdvPresentBirthdayInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class AgentAdvPresentBirthdayInfo : ScriptableObject
  {
    public List<AgentAdvPresentBirthdayInfo.Param> param;

    public AgentAdvPresentBirthdayInfo()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public List<AgentAdvPresentBirthdayInfo.ItemData> ItemData = new List<AgentAdvPresentBirthdayInfo.ItemData>();
      public int ID;
      public int ItemID;

      public Param()
      {
      }

      public Param(AgentAdvPresentBirthdayInfo.Param src)
      {
        this.ID = src.ID;
        this.ItemID = src.ItemID;
        this.ItemData = src.ItemData.ToList<AgentAdvPresentBirthdayInfo.ItemData>();
      }
    }

    [Serializable]
    public class ItemData
    {
      public int nameHash = -1;
    }
  }
}
