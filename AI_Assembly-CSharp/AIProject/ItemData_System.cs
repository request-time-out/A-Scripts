// Decompiled with JetBrains decompiler
// Type: AIProject.ItemData_System
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class ItemData_System : ScriptableObject
  {
    public List<ItemData_System.Param> param;

    public ItemData_System()
    {
      base.\u002Ector();
    }

    public static StuffItemInfo Convert(int category, ItemData_System.Param param)
    {
      return new StuffItemInfo(category, new ItemData.Param()
      {
        ID = param.ID,
        IconID = param.IconID,
        Name = param.Name,
        Grade = param.Grade,
        Rate = param.Rate,
        Explanation = param.Explanation
      }, true);
    }

    [Serializable]
    public class Param
    {
      public int ID = -1;
      public int IconID = -1;
      public string Name = string.Empty;
      public string Explanation = string.Empty;
      public int Grade;
      public int Rate;
    }
  }
}
