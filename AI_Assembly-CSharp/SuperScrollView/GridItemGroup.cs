// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridItemGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SuperScrollView
{
  public class GridItemGroup
  {
    private int mGroupIndex = -1;
    private int mCount;
    private LoopGridViewItem mFirst;
    private LoopGridViewItem mLast;

    public int Count
    {
      get
      {
        return this.mCount;
      }
    }

    public LoopGridViewItem First
    {
      get
      {
        return this.mFirst;
      }
    }

    public LoopGridViewItem Last
    {
      get
      {
        return this.mLast;
      }
    }

    public int GroupIndex
    {
      get
      {
        return this.mGroupIndex;
      }
      set
      {
        this.mGroupIndex = value;
      }
    }

    public LoopGridViewItem GetItemByColumn(int column)
    {
      for (LoopGridViewItem loopGridViewItem = this.mFirst; Object.op_Inequality((Object) loopGridViewItem, (Object) null); loopGridViewItem = loopGridViewItem.NextItem)
      {
        if (loopGridViewItem.Column == column)
          return loopGridViewItem;
      }
      return (LoopGridViewItem) null;
    }

    public LoopGridViewItem GetItemByRow(int row)
    {
      for (LoopGridViewItem loopGridViewItem = this.mFirst; Object.op_Inequality((Object) loopGridViewItem, (Object) null); loopGridViewItem = loopGridViewItem.NextItem)
      {
        if (loopGridViewItem.Row == row)
          return loopGridViewItem;
      }
      return (LoopGridViewItem) null;
    }

    public void ReplaceItem(LoopGridViewItem curItem, LoopGridViewItem newItem)
    {
      newItem.PrevItem = curItem.PrevItem;
      newItem.NextItem = curItem.NextItem;
      if (Object.op_Inequality((Object) newItem.PrevItem, (Object) null))
        newItem.PrevItem.NextItem = newItem;
      if (Object.op_Inequality((Object) newItem.NextItem, (Object) null))
        newItem.NextItem.PrevItem = newItem;
      if (Object.op_Equality((Object) this.mFirst, (Object) curItem))
        this.mFirst = newItem;
      if (!Object.op_Equality((Object) this.mLast, (Object) curItem))
        return;
      this.mLast = newItem;
    }

    public void AddFirst(LoopGridViewItem newItem)
    {
      newItem.PrevItem = (LoopGridViewItem) null;
      newItem.NextItem = (LoopGridViewItem) null;
      if (Object.op_Equality((Object) this.mFirst, (Object) null))
      {
        this.mFirst = newItem;
        this.mLast = newItem;
        this.mFirst.PrevItem = (LoopGridViewItem) null;
        this.mFirst.NextItem = (LoopGridViewItem) null;
        ++this.mCount;
      }
      else
      {
        this.mFirst.PrevItem = newItem;
        newItem.PrevItem = (LoopGridViewItem) null;
        newItem.NextItem = this.mFirst;
        this.mFirst = newItem;
        ++this.mCount;
      }
    }

    public void AddLast(LoopGridViewItem newItem)
    {
      newItem.PrevItem = (LoopGridViewItem) null;
      newItem.NextItem = (LoopGridViewItem) null;
      if (Object.op_Equality((Object) this.mFirst, (Object) null))
      {
        this.mFirst = newItem;
        this.mLast = newItem;
        this.mFirst.PrevItem = (LoopGridViewItem) null;
        this.mFirst.NextItem = (LoopGridViewItem) null;
        ++this.mCount;
      }
      else
      {
        this.mLast.NextItem = newItem;
        newItem.PrevItem = this.mLast;
        newItem.NextItem = (LoopGridViewItem) null;
        this.mLast = newItem;
        ++this.mCount;
      }
    }

    public LoopGridViewItem RemoveFirst()
    {
      LoopGridViewItem mFirst = this.mFirst;
      if (Object.op_Equality((Object) this.mFirst, (Object) null))
        return mFirst;
      if (Object.op_Equality((Object) this.mFirst, (Object) this.mLast))
      {
        this.mFirst = (LoopGridViewItem) null;
        this.mLast = (LoopGridViewItem) null;
        --this.mCount;
        return mFirst;
      }
      this.mFirst = this.mFirst.NextItem;
      this.mFirst.PrevItem = (LoopGridViewItem) null;
      --this.mCount;
      return mFirst;
    }

    public LoopGridViewItem RemoveLast()
    {
      LoopGridViewItem mLast = this.mLast;
      if (Object.op_Equality((Object) this.mFirst, (Object) null))
        return mLast;
      if (Object.op_Equality((Object) this.mFirst, (Object) this.mLast))
      {
        this.mFirst = (LoopGridViewItem) null;
        this.mLast = (LoopGridViewItem) null;
        --this.mCount;
        return mLast;
      }
      this.mLast = this.mLast.PrevItem;
      this.mLast.NextItem = (LoopGridViewItem) null;
      --this.mCount;
      return mLast;
    }

    public void Clear()
    {
      for (LoopGridViewItem loopGridViewItem = this.mFirst; Object.op_Inequality((Object) loopGridViewItem, (Object) null); loopGridViewItem = loopGridViewItem.NextItem)
      {
        loopGridViewItem.PrevItem = (LoopGridViewItem) null;
        loopGridViewItem.NextItem = (LoopGridViewItem) null;
      }
      this.mFirst = (LoopGridViewItem) null;
      this.mLast = (LoopGridViewItem) null;
      this.mCount = 0;
    }
  }
}
