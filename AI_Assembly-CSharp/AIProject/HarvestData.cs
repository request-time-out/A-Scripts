// Decompiled with JetBrains decompiler
// Type: AIProject.HarvestData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class HarvestData : ScriptableObject
  {
    public List<HarvestData.Param> param;

    public HarvestData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Data
    {
      public int nameHash = -1;
      public int stock = 1;
      public int percent = 100;
      public int group;
    }

    [Serializable]
    public class Param
    {
      public int nameHash = -1;
      public List<HarvestData.Data> data = new List<HarvestData.Data>();
      public int time;
    }
  }
}
