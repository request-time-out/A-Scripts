﻿// Decompiled with JetBrains decompiler
// Type: AIProject.ItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class ItemData : ScriptableObject
  {
    public List<ItemData.Param> param;

    public ItemData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public int ID = -1;
      public int IconID = -1;
      public string Name = string.Empty;
      public string Explanation = string.Empty;
      public int nameHash = -1;
      public int Grade;
      public int Rate;
    }
  }
}