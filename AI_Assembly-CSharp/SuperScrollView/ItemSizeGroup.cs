// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ItemSizeGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace SuperScrollView
{
  public class ItemSizeGroup
  {
    private int mDirtyBeginIndex = 100;
    public float[] mItemSizeArray;
    public float[] mItemStartPosArray;
    public int mItemCount;
    public float mGroupSize;
    public float mGroupStartPos;
    public float mGroupEndPos;
    public int mGroupIndex;
    private float mItemDefaultSize;
    private int mMaxNoZeroIndex;

    public ItemSizeGroup(int index, float itemDefaultSize)
    {
      this.mGroupIndex = index;
      this.mItemDefaultSize = itemDefaultSize;
      this.Init();
    }

    public void Init()
    {
      this.mItemSizeArray = new float[100];
      if ((double) this.mItemDefaultSize != 0.0)
      {
        for (int index = 0; index < this.mItemSizeArray.Length; ++index)
          this.mItemSizeArray[index] = this.mItemDefaultSize;
      }
      this.mItemStartPosArray = new float[100];
      this.mItemStartPosArray[0] = 0.0f;
      this.mItemCount = 100;
      this.mGroupSize = this.mItemDefaultSize * (float) this.mItemSizeArray.Length;
      if ((double) this.mItemDefaultSize != 0.0)
        this.mDirtyBeginIndex = 0;
      else
        this.mDirtyBeginIndex = 100;
    }

    public float GetItemStartPos(int index)
    {
      return this.mGroupStartPos + this.mItemStartPosArray[index];
    }

    public bool IsDirty
    {
      get
      {
        return this.mDirtyBeginIndex < this.mItemCount;
      }
    }

    public float SetItemSize(int index, float size)
    {
      if (index > this.mMaxNoZeroIndex && (double) size > 0.0)
        this.mMaxNoZeroIndex = index;
      float mItemSize = this.mItemSizeArray[index];
      if ((double) mItemSize == (double) size)
        return 0.0f;
      this.mItemSizeArray[index] = size;
      if (index < this.mDirtyBeginIndex)
        this.mDirtyBeginIndex = index;
      float num = size - mItemSize;
      this.mGroupSize += num;
      return num;
    }

    public void SetItemCount(int count)
    {
      if (count < this.mMaxNoZeroIndex)
        this.mMaxNoZeroIndex = count;
      if (this.mItemCount == count)
        return;
      this.mItemCount = count;
      this.RecalcGroupSize();
    }

    public void RecalcGroupSize()
    {
      this.mGroupSize = 0.0f;
      for (int index = 0; index < this.mItemCount; ++index)
        this.mGroupSize += this.mItemSizeArray[index];
    }

    public int GetItemIndexByPos(float pos)
    {
      if (this.mItemCount == 0)
        return -1;
      int num1 = 0;
      int num2 = this.mItemCount - 1;
      if ((double) this.mItemDefaultSize == 0.0)
      {
        if (this.mMaxNoZeroIndex < 0)
          this.mMaxNoZeroIndex = 0;
        num2 = this.mMaxNoZeroIndex;
      }
      while (num1 <= num2)
      {
        int index = (num1 + num2) / 2;
        float mItemStartPos = this.mItemStartPosArray[index];
        float num3 = mItemStartPos + this.mItemSizeArray[index];
        if ((double) mItemStartPos <= (double) pos && (double) num3 >= (double) pos)
          return index;
        if ((double) pos > (double) num3)
          num1 = index + 1;
        else
          num2 = index - 1;
      }
      return -1;
    }

    public void UpdateAllItemStartPos()
    {
      if (this.mDirtyBeginIndex >= this.mItemCount)
        return;
      for (int index = this.mDirtyBeginIndex >= 1 ? this.mDirtyBeginIndex : 1; index < this.mItemCount; ++index)
        this.mItemStartPosArray[index] = this.mItemStartPosArray[index - 1] + this.mItemSizeArray[index - 1];
      this.mDirtyBeginIndex = this.mItemCount;
    }

    public void ClearOldData()
    {
      for (int mItemCount = this.mItemCount; mItemCount < 100; ++mItemCount)
        this.mItemSizeArray[mItemCount] = 0.0f;
    }
  }
}
