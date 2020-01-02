// Decompiled with JetBrains decompiler
// Type: SuperScrollView.TreeViewItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace SuperScrollView
{
  public class TreeViewItemData
  {
    private List<ItemData> mChildItemDataList = new List<ItemData>();
    public string mName;
    public string mIcon;

    public int ChildCount
    {
      get
      {
        return this.mChildItemDataList.Count;
      }
    }

    public void AddChild(ItemData data)
    {
      this.mChildItemDataList.Add(data);
    }

    public ItemData GetChild(int index)
    {
      return index < 0 || index >= this.mChildItemDataList.Count ? (ItemData) null : this.mChildItemDataList[index];
    }
  }
}
