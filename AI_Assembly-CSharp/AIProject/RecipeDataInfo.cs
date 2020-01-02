// Decompiled with JetBrains decompiler
// Type: AIProject.RecipeDataInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class RecipeDataInfo
  {
    public RecipeDataInfo(RecipeData.Param param)
    {
      this.nameHash = param.nameHash;
      this.NeedList = param.NeedList.Select<RecipeData.NeedData, RecipeDataInfo.NeedData>((Func<RecipeData.NeedData, RecipeDataInfo.NeedData>) (s => new RecipeDataInfo.NeedData(s))).Where<RecipeDataInfo.NeedData>((Func<RecipeDataInfo.NeedData, bool>) (p => !p.hasError)).ToArray<RecipeDataInfo.NeedData>();
      this.CreateSum = param.CreateSum;
    }

    public int nameHash { get; } = -1;

    public int CreateSum { get; } = 1;

    public RecipeDataInfo.NeedData[] NeedList { get; }

    public class NeedData
    {
      public NeedData(RecipeData.NeedData needData)
      {
        this.info = Singleton<Resources>.Instance.GameInfo.FindItemInfo(needData.nameHash);
        if (this.hasError)
          Debug.LogError((object) string.Format("NeedData:{0}[Name none]", (object) needData.nameHash));
        else
          this.Sum = needData.Sum;
      }

      private StuffItemInfo info { get; }

      public bool hasError
      {
        get
        {
          return this.info == null;
        }
      }

      public string Name
      {
        get
        {
          return this.info.Name;
        }
      }

      public int nameHash
      {
        get
        {
          return this.info.nameHash;
        }
      }

      public int Sum { get; } = 1;

      public int CategoryID
      {
        get
        {
          return this.info.CategoryID;
        }
      }

      public int ID
      {
        get
        {
          return this.info.ID;
        }
      }
    }
  }
}
