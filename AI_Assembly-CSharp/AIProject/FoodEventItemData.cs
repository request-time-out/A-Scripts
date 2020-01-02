// Decompiled with JetBrains decompiler
// Type: AIProject.FoodEventItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class FoodEventItemData : ScriptableObject
  {
    public List<FoodEventItemData.Param> param;

    public FoodEventItemData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public string Name;
      public int CategoryID;
      public int ItemID;
      public int EventItemID;
      public string DateEventItemID;
    }
  }
}
