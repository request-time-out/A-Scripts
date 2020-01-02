// Decompiled with JetBrains decompiler
// Type: AIProject.RecyclingItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace AIProject
{
  public struct RecyclingItemInfo : IComparable<RecyclingItemInfo>
  {
    public int CategoryID;
    public int ItemID;
    public int IconID;
    public bool Adult;
    public List<string> ItemNameList;
    public StuffItemInfo ItemInfo;

    public int CompareTo(RecyclingItemInfo x)
    {
      return this.CategoryID == x.CategoryID ? this.ItemID - x.ItemID : this.CategoryID - x.CategoryID;
    }
  }
}
