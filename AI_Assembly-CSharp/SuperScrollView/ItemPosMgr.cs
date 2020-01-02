// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ItemPosMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace SuperScrollView
{
  public class ItemPosMgr
  {
    private List<ItemSizeGroup> mItemSizeGroupList = new List<ItemSizeGroup>();
    private int mDirtyBeginIndex = int.MaxValue;
    public float mItemDefaultSize = 20f;
    public const int mItemMaxCountPerGroup = 100;
    public float mTotalSize;
    private int mMaxNotEmptyGroupIndex;

    public ItemPosMgr(float itemDefaultSize)
    {
      this.mItemDefaultSize = itemDefaultSize;
    }

    public void SetItemMaxCount(int maxCount)
    {
      this.mDirtyBeginIndex = 0;
      this.mTotalSize = 0.0f;
      int num1 = maxCount % 100;
      int count1 = num1;
      int index1 = maxCount / 100;
      if (num1 > 0)
        ++index1;
      else
        count1 = 100;
      int count2 = this.mItemSizeGroupList.Count;
      if (count2 > index1)
      {
        int count3 = count2 - index1;
        this.mItemSizeGroupList.RemoveRange(index1, count3);
      }
      else if (count2 < index1)
      {
        if (count2 > 0)
          this.mItemSizeGroupList[count2 - 1].ClearOldData();
        int num2 = index1 - count2;
        for (int index2 = 0; index2 < num2; ++index2)
          this.mItemSizeGroupList.Add(new ItemSizeGroup(count2 + index2, this.mItemDefaultSize));
      }
      else if (count2 > 0)
        this.mItemSizeGroupList[count2 - 1].ClearOldData();
      int count4 = this.mItemSizeGroupList.Count;
      if (count4 - 1 < this.mMaxNotEmptyGroupIndex)
        this.mMaxNotEmptyGroupIndex = count4 - 1;
      if (this.mMaxNotEmptyGroupIndex < 0)
        this.mMaxNotEmptyGroupIndex = 0;
      if (count4 == 0)
        return;
      for (int index2 = 0; index2 < count4 - 1; ++index2)
        this.mItemSizeGroupList[index2].SetItemCount(100);
      this.mItemSizeGroupList[count4 - 1].SetItemCount(count1);
      for (int index2 = 0; index2 < count4; ++index2)
        this.mTotalSize += this.mItemSizeGroupList[index2].mGroupSize;
    }

    public void SetItemSize(int itemIndex, float size)
    {
      int index1 = itemIndex / 100;
      int index2 = itemIndex % 100;
      float num = this.mItemSizeGroupList[index1].SetItemSize(index2, size);
      if ((double) num != 0.0 && index1 < this.mDirtyBeginIndex)
        this.mDirtyBeginIndex = index1;
      this.mTotalSize += num;
      if (index1 <= this.mMaxNotEmptyGroupIndex || (double) size <= 0.0)
        return;
      this.mMaxNotEmptyGroupIndex = index1;
    }

    public float GetItemPos(int itemIndex)
    {
      this.Update(true);
      return this.mItemSizeGroupList[itemIndex / 100].GetItemStartPos(itemIndex % 100);
    }

    public bool GetItemIndexAndPosAtGivenPos(float pos, ref int index, ref float itemPos)
    {
      this.Update(true);
      index = 0;
      itemPos = 0.0f;
      int count = this.mItemSizeGroupList.Count;
      if (count == 0)
        return true;
      ItemSizeGroup itemSizeGroup = (ItemSizeGroup) null;
      int num1 = 0;
      int num2 = count - 1;
      if ((double) this.mItemDefaultSize == 0.0)
      {
        if (this.mMaxNotEmptyGroupIndex < 0)
          this.mMaxNotEmptyGroupIndex = 0;
        num2 = this.mMaxNotEmptyGroupIndex;
      }
      while (num1 <= num2)
      {
        int index1 = (num1 + num2) / 2;
        ItemSizeGroup mItemSizeGroup = this.mItemSizeGroupList[index1];
        if ((double) mItemSizeGroup.mGroupStartPos <= (double) pos && (double) mItemSizeGroup.mGroupEndPos >= (double) pos)
        {
          itemSizeGroup = mItemSizeGroup;
          break;
        }
        if ((double) pos > (double) mItemSizeGroup.mGroupEndPos)
          num1 = index1 + 1;
        else
          num2 = index1 - 1;
      }
      if (itemSizeGroup == null)
        return false;
      int itemIndexByPos = itemSizeGroup.GetItemIndexByPos(pos - itemSizeGroup.mGroupStartPos);
      if (itemIndexByPos < 0)
        return false;
      index = itemIndexByPos + itemSizeGroup.mGroupIndex * 100;
      itemPos = itemSizeGroup.GetItemStartPos(itemIndexByPos);
      return true;
    }

    public void Update(bool updateAll)
    {
      int count = this.mItemSizeGroupList.Count;
      if (count == 0 || this.mDirtyBeginIndex >= count)
        return;
      int num = 0;
      for (int mDirtyBeginIndex = this.mDirtyBeginIndex; mDirtyBeginIndex < count; ++mDirtyBeginIndex)
      {
        ++num;
        ItemSizeGroup mItemSizeGroup = this.mItemSizeGroupList[mDirtyBeginIndex];
        ++this.mDirtyBeginIndex;
        mItemSizeGroup.UpdateAllItemStartPos();
        if (mDirtyBeginIndex == 0)
        {
          mItemSizeGroup.mGroupStartPos = 0.0f;
          mItemSizeGroup.mGroupEndPos = mItemSizeGroup.mGroupSize;
        }
        else
        {
          mItemSizeGroup.mGroupStartPos = this.mItemSizeGroupList[mDirtyBeginIndex - 1].mGroupEndPos;
          mItemSizeGroup.mGroupEndPos = mItemSizeGroup.mGroupStartPos + mItemSizeGroup.mGroupSize;
        }
        if (!updateAll && num > 1)
          break;
      }
    }
  }
}
