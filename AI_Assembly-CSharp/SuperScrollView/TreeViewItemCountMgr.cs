// Decompiled with JetBrains decompiler
// Type: SuperScrollView.TreeViewItemCountMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace SuperScrollView
{
  public class TreeViewItemCountMgr
  {
    private List<TreeViewItemCountData> mTreeItemDataList = new List<TreeViewItemCountData>();
    private bool mIsDirty = true;
    private TreeViewItemCountData mLastQueryResult;

    public void AddTreeItem(int count, bool isExpand)
    {
      this.mTreeItemDataList.Add(new TreeViewItemCountData()
      {
        mTreeItemIndex = this.mTreeItemDataList.Count,
        mChildCount = count,
        mIsExpand = isExpand
      });
      this.mIsDirty = true;
    }

    public void Clear()
    {
      this.mTreeItemDataList.Clear();
      this.mLastQueryResult = (TreeViewItemCountData) null;
      this.mIsDirty = true;
    }

    public TreeViewItemCountData GetTreeItem(int treeIndex)
    {
      return treeIndex < 0 || treeIndex >= this.mTreeItemDataList.Count ? (TreeViewItemCountData) null : this.mTreeItemDataList[treeIndex];
    }

    public void SetItemChildCount(int treeIndex, int count)
    {
      if (treeIndex < 0 || treeIndex >= this.mTreeItemDataList.Count)
        return;
      this.mIsDirty = true;
      this.mTreeItemDataList[treeIndex].mChildCount = count;
    }

    public void SetItemExpand(int treeIndex, bool isExpand)
    {
      if (treeIndex < 0 || treeIndex >= this.mTreeItemDataList.Count)
        return;
      this.mIsDirty = true;
      this.mTreeItemDataList[treeIndex].mIsExpand = isExpand;
    }

    public void ToggleItemExpand(int treeIndex)
    {
      if (treeIndex < 0 || treeIndex >= this.mTreeItemDataList.Count)
        return;
      this.mIsDirty = true;
      TreeViewItemCountData mTreeItemData = this.mTreeItemDataList[treeIndex];
      mTreeItemData.mIsExpand = !mTreeItemData.mIsExpand;
    }

    public bool IsTreeItemExpand(int treeIndex)
    {
      TreeViewItemCountData treeItem = this.GetTreeItem(treeIndex);
      return treeItem != null && treeItem.mIsExpand;
    }

    private void UpdateAllTreeItemDataIndex()
    {
      if (!this.mIsDirty)
        return;
      this.mLastQueryResult = (TreeViewItemCountData) null;
      this.mIsDirty = false;
      int count = this.mTreeItemDataList.Count;
      if (count == 0)
        return;
      TreeViewItemCountData mTreeItemData1 = this.mTreeItemDataList[0];
      mTreeItemData1.mBeginIndex = 0;
      mTreeItemData1.mEndIndex = !mTreeItemData1.mIsExpand ? 0 : mTreeItemData1.mChildCount;
      int mEndIndex = mTreeItemData1.mEndIndex;
      for (int index = 1; index < count; ++index)
      {
        TreeViewItemCountData mTreeItemData2 = this.mTreeItemDataList[index];
        mTreeItemData2.mBeginIndex = mEndIndex + 1;
        mTreeItemData2.mEndIndex = mTreeItemData2.mBeginIndex + (!mTreeItemData2.mIsExpand ? 0 : mTreeItemData2.mChildCount);
        mEndIndex = mTreeItemData2.mEndIndex;
      }
    }

    public int TreeViewItemCount
    {
      get
      {
        return this.mTreeItemDataList.Count;
      }
    }

    public int GetTotalItemAndChildCount()
    {
      int count = this.mTreeItemDataList.Count;
      if (count == 0)
        return 0;
      this.UpdateAllTreeItemDataIndex();
      return this.mTreeItemDataList[count - 1].mEndIndex + 1;
    }

    public TreeViewItemCountData QueryTreeItemByTotalIndex(int totalIndex)
    {
      if (totalIndex < 0)
        return (TreeViewItemCountData) null;
      int count = this.mTreeItemDataList.Count;
      if (count == 0)
        return (TreeViewItemCountData) null;
      this.UpdateAllTreeItemDataIndex();
      if (this.mLastQueryResult != null && this.mLastQueryResult.mBeginIndex <= totalIndex && this.mLastQueryResult.mEndIndex >= totalIndex)
        return this.mLastQueryResult;
      int num1 = 0;
      int num2 = count - 1;
      while (num1 <= num2)
      {
        int index = (num1 + num2) / 2;
        TreeViewItemCountData mTreeItemData = this.mTreeItemDataList[index];
        if (mTreeItemData.mBeginIndex <= totalIndex && mTreeItemData.mEndIndex >= totalIndex)
        {
          this.mLastQueryResult = mTreeItemData;
          return mTreeItemData;
        }
        if (totalIndex > mTreeItemData.mEndIndex)
          num1 = index + 1;
        else
          num2 = index - 1;
      }
      return (TreeViewItemCountData) null;
    }
  }
}
