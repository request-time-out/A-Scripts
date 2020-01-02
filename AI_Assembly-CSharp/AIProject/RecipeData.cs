// Decompiled with JetBrains decompiler
// Type: AIProject.RecipeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class RecipeData : ScriptableObject
  {
    public List<RecipeData.Param> param;

    public RecipeData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class NeedData
    {
      public int Sum = 1;
      public int nameHash = -1;
    }

    [Serializable]
    public class Param
    {
      public int CreateSum = 1;
      public List<RecipeData.NeedData> NeedList = new List<RecipeData.NeedData>();
      public int nameHash = -1;
    }
  }
}
