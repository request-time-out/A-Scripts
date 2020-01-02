// Decompiled with JetBrains decompiler
// Type: SuperScrollView.TreeViewItemCountData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace SuperScrollView
{
  public class TreeViewItemCountData
  {
    public bool mIsExpand = true;
    public int mTreeItemIndex;
    public int mChildCount;
    public int mBeginIndex;
    public int mEndIndex;

    public bool IsChild(int index)
    {
      return index != this.mBeginIndex;
    }

    public int GetChildIndex(int index)
    {
      return !this.IsChild(index) ? -1 : index - this.mBeginIndex - 1;
    }
  }
}
