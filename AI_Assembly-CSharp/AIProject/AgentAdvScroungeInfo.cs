// Decompiled with JetBrains decompiler
// Type: AIProject.AgentAdvScroungeInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class AgentAdvScroungeInfo : ScriptableObject
  {
    public List<AgentAdvScroungeInfo.Param> param;

    public AgentAdvScroungeInfo()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public List<AgentAdvScroungeInfo.ItemData> ItemData = new List<AgentAdvScroungeInfo.ItemData>();
      public int ID;

      public Param()
      {
      }

      public Param(AgentAdvScroungeInfo.Param src)
      {
        this.ID = src.ID;
        this.ItemData = src.ItemData.ToList<AgentAdvScroungeInfo.ItemData>();
      }
    }

    [Serializable]
    public class ItemData
    {
      public int nameHash = -1;
      public int sum = 1;
    }
  }
}
