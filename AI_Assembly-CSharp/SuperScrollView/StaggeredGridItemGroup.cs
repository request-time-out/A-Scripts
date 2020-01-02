// Decompiled with JetBrains decompiler
// Type: SuperScrollView.StaggeredGridItemGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class StaggeredGridItemGroup
  {
    private List<LoopStaggeredGridViewItem> mItemList = new List<LoopStaggeredGridViewItem>();
    private List<int> mItemIndexMap = new List<int>();
    private Vector3[] mItemWorldCorners = new Vector3[4];
    private Vector3[] mViewPortRectLocalCorners = new Vector3[4];
    private bool mNeedCheckNextMinItem = true;
    private bool mNeedCheckNextMaxItem = true;
    private bool mSupportScrollBar = true;
    private Vector3 mLastFrameContainerPos = Vector3.get_zero();
    private LoopStaggeredGridView mParentGridView;
    private ListItemArrangeType mArrangeType;
    private RectTransform mContainerTrans;
    private ScrollRect mScrollRect;
    public int mGroupIndex;
    private GameObject mGameObject;
    private RectTransform mScrollRectTransform;
    private RectTransform mViewPortRectTransform;
    private float mItemDefaultWithPaddingSize;
    private int mItemTotalCount;
    private bool mIsVertList;
    private Func<int, int, LoopStaggeredGridViewItem> mOnGetItemByIndex;
    private int mCurReadyMinItemIndex;
    private int mCurReadyMaxItemIndex;
    private ItemPosMgr mItemPosMgr;
    private int mLastItemIndex;
    private float mLastItemPadding;
    private int mListUpdateCheckFrameCount;

    public void Init(
      LoopStaggeredGridView parent,
      int itemTotalCount,
      int groupIndex,
      Func<int, int, LoopStaggeredGridViewItem> onGetItemByIndex)
    {
      this.mGroupIndex = groupIndex;
      this.mParentGridView = parent;
      this.mArrangeType = this.mParentGridView.ArrangeType;
      this.mGameObject = ((Component) this.mParentGridView).get_gameObject();
      this.mScrollRect = (ScrollRect) this.mGameObject.GetComponent<ScrollRect>();
      this.mItemPosMgr = new ItemPosMgr(this.mItemDefaultWithPaddingSize);
      this.mScrollRectTransform = (RectTransform) ((Component) this.mScrollRect).GetComponent<RectTransform>();
      this.mContainerTrans = this.mScrollRect.get_content();
      this.mViewPortRectTransform = this.mScrollRect.get_viewport();
      if (Object.op_Equality((Object) this.mViewPortRectTransform, (Object) null))
        this.mViewPortRectTransform = this.mScrollRectTransform;
      this.mIsVertList = this.mArrangeType == ListItemArrangeType.TopToBottom || this.mArrangeType == ListItemArrangeType.BottomToTop;
      this.mOnGetItemByIndex = onGetItemByIndex;
      this.mItemTotalCount = itemTotalCount;
      this.mViewPortRectTransform.GetLocalCorners(this.mViewPortRectLocalCorners);
      if (this.mItemTotalCount < 0)
        this.mSupportScrollBar = false;
      if (this.mSupportScrollBar)
        this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
      else
        this.mItemPosMgr.SetItemMaxCount(0);
      this.mCurReadyMaxItemIndex = 0;
      this.mCurReadyMinItemIndex = 0;
      this.mNeedCheckNextMaxItem = true;
      this.mNeedCheckNextMinItem = true;
    }

    public List<int> ItemIndexMap
    {
      get
      {
        return this.mItemIndexMap;
      }
    }

    public void ResetListView()
    {
      this.mViewPortRectTransform.GetLocalCorners(this.mViewPortRectLocalCorners);
    }

    public LoopStaggeredGridViewItem GetShownItemByItemIndex(int itemIndex)
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return (LoopStaggeredGridViewItem) null;
      if (itemIndex < this.mItemList[0].ItemIndex || itemIndex > this.mItemList[count - 1].ItemIndex)
        return (LoopStaggeredGridViewItem) null;
      for (int index = 0; index < count; ++index)
      {
        LoopStaggeredGridViewItem mItem = this.mItemList[index];
        if (mItem.ItemIndex == itemIndex)
          return mItem;
      }
      return (LoopStaggeredGridViewItem) null;
    }

    public float ViewPortSize
    {
      get
      {
        if (this.mIsVertList)
        {
          Rect rect = this.mViewPortRectTransform.get_rect();
          return ((Rect) ref rect).get_height();
        }
        Rect rect1 = this.mViewPortRectTransform.get_rect();
        return ((Rect) ref rect1).get_width();
      }
    }

    public float ViewPortWidth
    {
      get
      {
        Rect rect = this.mViewPortRectTransform.get_rect();
        return ((Rect) ref rect).get_width();
      }
    }

    public float ViewPortHeight
    {
      get
      {
        Rect rect = this.mViewPortRectTransform.get_rect();
        return ((Rect) ref rect).get_height();
      }
    }

    private bool IsDraging
    {
      get
      {
        return this.mParentGridView.IsDraging;
      }
    }

    public LoopStaggeredGridViewItem GetShownItemByIndexInGroup(
      int indexInGroup)
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return (LoopStaggeredGridViewItem) null;
      return indexInGroup < this.mItemList[0].ItemIndexInGroup || indexInGroup > this.mItemList[count - 1].ItemIndexInGroup ? (LoopStaggeredGridViewItem) null : this.mItemList[indexInGroup - this.mItemList[0].ItemIndexInGroup];
    }

    public int GetIndexInShownItemList(LoopStaggeredGridViewItem item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return -1;
      int count = this.mItemList.Count;
      if (count == 0)
        return -1;
      for (int index = 0; index < count; ++index)
      {
        if (Object.op_Equality((Object) this.mItemList[index], (Object) item))
          return index;
      }
      return -1;
    }

    public void RefreshAllShownItem()
    {
      if (this.mItemList.Count == 0)
        return;
      this.RefreshAllShownItemWithFirstIndexInGroup(this.mItemList[0].ItemIndexInGroup);
    }

    public void OnItemSizeChanged(int indexInGroup)
    {
      LoopStaggeredGridViewItem itemByIndexInGroup = this.GetShownItemByIndexInGroup(indexInGroup);
      if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
        return;
      if (this.mSupportScrollBar)
      {
        if (this.mIsVertList)
        {
          int itemIndex = indexInGroup;
          Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double padding = (double) itemByIndexInGroup.Padding;
          this.SetItemSize(itemIndex, (float) height, (float) padding);
        }
        else
        {
          int itemIndex = indexInGroup;
          Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double padding = (double) itemByIndexInGroup.Padding;
          this.SetItemSize(itemIndex, (float) width, (float) padding);
        }
      }
      this.UpdateAllShownItemsPos();
    }

    public void RefreshItemByIndexInGroup(int indexInGroup)
    {
      int count = this.mItemList.Count;
      if (count == 0 || indexInGroup < this.mItemList[0].ItemIndexInGroup || indexInGroup > this.mItemList[count - 1].ItemIndexInGroup)
        return;
      int itemIndexInGroup = this.mItemList[0].ItemIndexInGroup;
      int index = indexInGroup - itemIndexInGroup;
      LoopStaggeredGridViewItem mItem = this.mItemList[index];
      Vector3 anchoredPosition3D = mItem.CachedRectTransform.get_anchoredPosition3D();
      this.RecycleItemTmp(mItem);
      LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
      if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
      {
        this.RefreshAllShownItemWithFirstIndexInGroup(itemIndexInGroup);
      }
      else
      {
        this.mItemList[index] = itemByIndexInGroup;
        if (this.mIsVertList)
          anchoredPosition3D.x = (__Null) (double) itemByIndexInGroup.StartPosOffset;
        else
          anchoredPosition3D.y = (__Null) (double) itemByIndexInGroup.StartPosOffset;
        itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
        this.OnItemSizeChanged(indexInGroup);
        this.ClearAllTmpRecycledItem();
      }
    }

    public void RefreshAllShownItemWithFirstIndexInGroup(int firstItemIndexInGroup)
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      Vector3 anchoredPosition3D = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
      this.RecycleAllItem();
      for (int index = 0; index < count; ++index)
      {
        int indexInGroup = firstItemIndexInGroup + index;
        LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
        if (!Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
        {
          if (this.mIsVertList)
            anchoredPosition3D.x = (__Null) (double) itemByIndexInGroup.StartPosOffset;
          else
            anchoredPosition3D.y = (__Null) (double) itemByIndexInGroup.StartPosOffset;
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
          if (this.mSupportScrollBar)
          {
            if (this.mIsVertList)
            {
              int itemIndex = indexInGroup;
              Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
              double height = (double) ((Rect) ref rect).get_height();
              double padding = (double) itemByIndexInGroup.Padding;
              this.SetItemSize(itemIndex, (float) height, (float) padding);
            }
            else
            {
              int itemIndex = indexInGroup;
              Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
              double width = (double) ((Rect) ref rect).get_width();
              double padding = (double) itemByIndexInGroup.Padding;
              this.SetItemSize(itemIndex, (float) width, (float) padding);
            }
          }
          this.mItemList.Add(itemByIndexInGroup);
        }
        else
          break;
      }
      this.UpdateAllShownItemsPos();
      this.ClearAllTmpRecycledItem();
    }

    public void RefreshAllShownItemWithFirstIndexAndPos(int firstItemIndexInGroup, Vector3 pos)
    {
      this.RecycleAllItem();
      LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(firstItemIndexInGroup);
      if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
        return;
      if (this.mIsVertList)
        pos.x = (__Null) (double) itemByIndexInGroup.StartPosOffset;
      else
        pos.y = (__Null) (double) itemByIndexInGroup.StartPosOffset;
      itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(pos);
      if (this.mSupportScrollBar)
      {
        if (this.mIsVertList)
        {
          int itemIndex = firstItemIndexInGroup;
          Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double padding = (double) itemByIndexInGroup.Padding;
          this.SetItemSize(itemIndex, (float) height, (float) padding);
        }
        else
        {
          int itemIndex = firstItemIndexInGroup;
          Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double padding = (double) itemByIndexInGroup.Padding;
          this.SetItemSize(itemIndex, (float) width, (float) padding);
        }
      }
      this.mItemList.Add(itemByIndexInGroup);
      this.UpdateAllShownItemsPos();
      this.mParentGridView.UpdateListViewWithDefault();
      this.ClearAllTmpRecycledItem();
    }

    private void SetItemSize(int itemIndex, float itemSize, float padding)
    {
      this.mItemPosMgr.SetItemSize(itemIndex, itemSize + padding);
      if (itemIndex < this.mLastItemIndex)
        return;
      this.mLastItemIndex = itemIndex;
      this.mLastItemPadding = padding;
    }

    private bool GetPlusItemIndexAndPosAtGivenPos(float pos, ref int index, ref float itemPos)
    {
      return this.mItemPosMgr.GetItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos);
    }

    public float GetItemPos(int itemIndex)
    {
      return this.mItemPosMgr.GetItemPos(itemIndex);
    }

    public Vector3 GetItemCornerPosInViewPort(
      LoopStaggeredGridViewItem item,
      ItemCornerEnum corner = ItemCornerEnum.LeftBottom)
    {
      item.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
      return ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[(int) corner]);
    }

    public void RecycleItemTmp(LoopStaggeredGridViewItem item)
    {
      this.mParentGridView.RecycleItemTmp(item);
    }

    public void RecycleAllItem()
    {
      foreach (LoopStaggeredGridViewItem mItem in this.mItemList)
        this.RecycleItemTmp(mItem);
      this.mItemList.Clear();
    }

    public void ClearAllTmpRecycledItem()
    {
      this.mParentGridView.ClearAllTmpRecycledItem();
    }

    private LoopStaggeredGridViewItem GetNewItemByIndexInGroup(
      int indexInGroup)
    {
      return this.mParentGridView.GetNewItemByGroupAndIndex(this.mGroupIndex, indexInGroup);
    }

    public int HadCreatedItemCount
    {
      get
      {
        return this.mItemIndexMap.Count;
      }
    }

    public void SetListItemCount(int itemCount)
    {
      if (itemCount == this.mItemTotalCount)
        return;
      int mItemTotalCount = this.mItemTotalCount;
      this.mItemTotalCount = itemCount;
      this.UpdateItemIndexMap(mItemTotalCount);
      if (mItemTotalCount < this.mItemTotalCount)
      {
        this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
      }
      else
      {
        this.mItemPosMgr.SetItemMaxCount(this.HadCreatedItemCount);
        this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
      }
      this.RecycleAllItem();
      if (this.mItemTotalCount == 0)
      {
        this.mCurReadyMaxItemIndex = 0;
        this.mCurReadyMinItemIndex = 0;
        this.mNeedCheckNextMaxItem = false;
        this.mNeedCheckNextMinItem = false;
        this.mItemIndexMap.Clear();
      }
      else
      {
        if (this.mCurReadyMaxItemIndex >= this.mItemTotalCount)
          this.mCurReadyMaxItemIndex = this.mItemTotalCount - 1;
        this.mNeedCheckNextMaxItem = true;
        this.mNeedCheckNextMinItem = true;
      }
    }

    private void UpdateItemIndexMap(int oldItemTotalCount)
    {
      int count = this.mItemIndexMap.Count;
      if (count == 0)
        return;
      if (this.mItemTotalCount == 0)
      {
        this.mItemIndexMap.Clear();
      }
      else
      {
        if (this.mItemTotalCount >= oldItemTotalCount)
          return;
        int itemTotalCount = this.mParentGridView.ItemTotalCount;
        if (this.mItemIndexMap[count - 1] < itemTotalCount)
          return;
        int num1 = 0;
        int num2 = count - 1;
        int num3 = 0;
        while (num1 <= num2)
        {
          int index = (num1 + num2) / 2;
          int mItemIndex = this.mItemIndexMap[index];
          if (mItemIndex == itemTotalCount)
          {
            num3 = index;
            break;
          }
          if (mItemIndex < itemTotalCount)
          {
            num1 = index + 1;
            num3 = num1;
          }
          else
            break;
        }
        int index1 = 0;
        for (int index2 = num3; index2 < count; ++index2)
        {
          if (this.mItemIndexMap[index2] >= itemTotalCount)
          {
            index1 = index2;
            break;
          }
        }
        this.mItemIndexMap.RemoveRange(index1, count - index1);
      }
    }

    public void UpdateListViewPart1(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      if (this.mSupportScrollBar)
        this.mItemPosMgr.Update(false);
      this.mListUpdateCheckFrameCount = this.mParentGridView.ListUpdateCheckFrameCount;
      bool flag = true;
      int num1 = 0;
      int num2 = 9999;
      for (; flag; flag = !this.mIsVertList ? this.UpdateForHorizontalListPart1(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1) : this.UpdateForVertListPart1(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1))
      {
        ++num1;
        if (num1 >= num2)
        {
          Debug.LogError((object) ("UpdateListViewPart1 while loop " + (object) num1 + " times! something is wrong!"));
          break;
        }
      }
      this.mLastFrameContainerPos = this.mContainerTrans.get_anchoredPosition3D();
    }

    public bool UpdateListViewPart2(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      return this.mIsVertList ? this.UpdateForVertListPart2(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1) : this.UpdateForHorizontalListPart2(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1);
    }

    public bool UpdateForVertListPart1(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      if (this.mItemTotalCount == 0)
      {
        if (this.mItemList.Count > 0)
          this.RecycleAllItem();
        return false;
      }
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        if (this.mItemList.Count == 0)
        {
          float pos = (float) this.mContainerTrans.get_anchoredPosition3D().y;
          if ((double) pos < 0.0)
            pos = 0.0f;
          int index = 0;
          float itemPos = -pos;
          if (this.mSupportScrollBar)
          {
            if (!this.GetPlusItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos))
              return false;
            itemPos = -itemPos;
          }
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, itemPos, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.IsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_2.y - this.mViewPortRectLocalCorners[1].y > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.IsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[0].y - vector3_3.y > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        if (vector3_1.y - this.mViewPortRectLocalCorners[1].y < (double) distanceForNew0)
        {
          if (mItem1.ItemIndexInGroup < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
            this.mNeedCheckNextMinItem = true;
          }
          int indexInGroup = mItem1.ItemIndexInGroup - 1;
          if (indexInGroup >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = indexInGroup;
                Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
                double height = (double) ((Rect) ref rect).get_height();
                double padding = (double) itemByIndexInGroup.Padding;
                this.SetItemSize(itemIndex, (float) height, (float) padding);
              }
              this.mItemList.Insert(0, itemByIndexInGroup);
              // ISSUE: variable of the null type
              __Null y = mItem1.CachedRectTransform.get_anchoredPosition3D().y;
              Rect rect1 = itemByIndexInGroup.CachedRectTransform.get_rect();
              double height1 = (double) ((Rect) ref rect1).get_height();
              float num = (float) (y + height1) + itemByIndexInGroup.Padding;
              itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, num, 0.0f));
              this.CheckIfNeedUpdateItemPos();
              if (indexInGroup < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = indexInGroup;
              return true;
            }
          }
        }
        if (this.mViewPortRectLocalCorners[0].y - vector3_4.y < (double) distanceForNew1)
        {
          if (mItem2.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem2.ItemIndexInGroup + 1;
          if (indexInGroup >= this.mItemIndexMap.Count || indexInGroup > this.mCurReadyMaxItemIndex && !this.mNeedCheckNextMaxItem)
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = false;
            this.CheckIfNeedUpdateItemPos();
            return false;
          }
          if (this.mSupportScrollBar)
          {
            int itemIndex = indexInGroup;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          // ISSUE: variable of the null type
          __Null y = mItem2.CachedRectTransform.get_anchoredPosition3D().y;
          Rect rect1 = mItem2.CachedRectTransform.get_rect();
          double height1 = (double) ((Rect) ref rect1).get_height();
          float num = (float) (y - height1) - mItem2.Padding;
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, num, 0.0f));
          this.CheckIfNeedUpdateItemPos();
          if (indexInGroup > this.mCurReadyMaxItemIndex)
            this.mCurReadyMaxItemIndex = indexInGroup;
          return true;
        }
      }
      else
      {
        if (this.mItemList.Count == 0)
        {
          float num = (float) this.mContainerTrans.get_anchoredPosition3D().y;
          if ((double) num > 0.0)
            num = 0.0f;
          int index = 0;
          float itemPos = -num;
          if (this.mSupportScrollBar && !this.GetPlusItemIndexAndPosAtGivenPos(-num, ref index, ref itemPos))
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, itemPos, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.IsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[0].y - vector3_1.y > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.IsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_4.y - this.mViewPortRectLocalCorners[1].y > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        if (this.mViewPortRectLocalCorners[0].y - vector3_2.y < (double) distanceForNew0)
        {
          if (mItem1.ItemIndexInGroup < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
            this.mNeedCheckNextMinItem = true;
          }
          int indexInGroup = mItem1.ItemIndexInGroup - 1;
          if (indexInGroup >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = indexInGroup;
                Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
                double height = (double) ((Rect) ref rect).get_height();
                double padding = (double) itemByIndexInGroup.Padding;
                this.SetItemSize(itemIndex, (float) height, (float) padding);
              }
              this.mItemList.Insert(0, itemByIndexInGroup);
              // ISSUE: variable of the null type
              __Null y = mItem1.CachedRectTransform.get_anchoredPosition3D().y;
              Rect rect1 = itemByIndexInGroup.CachedRectTransform.get_rect();
              double height1 = (double) ((Rect) ref rect1).get_height();
              float num = (float) (y - height1) - itemByIndexInGroup.Padding;
              itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, num, 0.0f));
              this.CheckIfNeedUpdateItemPos();
              if (indexInGroup < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = indexInGroup;
              return true;
            }
          }
        }
        if (vector3_3.y - this.mViewPortRectLocalCorners[1].y < (double) distanceForNew1)
        {
          if (mItem2.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem2.ItemIndexInGroup + 1;
          if (indexInGroup >= this.mItemIndexMap.Count || indexInGroup > this.mCurReadyMaxItemIndex && !this.mNeedCheckNextMaxItem)
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = false;
            this.CheckIfNeedUpdateItemPos();
            return false;
          }
          if (this.mSupportScrollBar)
          {
            int itemIndex = indexInGroup;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          // ISSUE: variable of the null type
          __Null y = mItem2.CachedRectTransform.get_anchoredPosition3D().y;
          Rect rect1 = mItem2.CachedRectTransform.get_rect();
          double height1 = (double) ((Rect) ref rect1).get_height();
          float num = (float) (y + height1) + mItem2.Padding;
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, num, 0.0f));
          this.CheckIfNeedUpdateItemPos();
          if (indexInGroup > this.mCurReadyMaxItemIndex)
            this.mCurReadyMaxItemIndex = indexInGroup;
          return true;
        }
      }
      return false;
    }

    public bool UpdateForVertListPart2(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      if (this.mItemTotalCount == 0)
      {
        if (this.mItemList.Count > 0)
          this.RecycleAllItem();
        return false;
      }
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        if (this.mItemList.Count == 0)
        {
          float pos = (float) this.mContainerTrans.get_anchoredPosition3D().y;
          if ((double) pos < 0.0)
            pos = 0.0f;
          int index = 0;
          float itemPos = -pos;
          if (this.mSupportScrollBar)
          {
            if (!this.GetPlusItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos))
              return false;
            itemPos = -itemPos;
          }
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, itemPos, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem = this.mItemList[this.mItemList.Count - 1];
        mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        if (this.mViewPortRectLocalCorners[0].y - ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]).y < (double) distanceForNew1)
        {
          if (mItem.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem.ItemIndexInGroup + 1;
          if (indexInGroup <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdateItemPos();
              return false;
            }
            if (this.mSupportScrollBar)
            {
              int itemIndex = indexInGroup;
              Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
              double height = (double) ((Rect) ref rect).get_height();
              double padding = (double) itemByIndexInGroup.Padding;
              this.SetItemSize(itemIndex, (float) height, (float) padding);
            }
            this.mItemList.Add(itemByIndexInGroup);
            // ISSUE: variable of the null type
            __Null y = mItem.CachedRectTransform.get_anchoredPosition3D().y;
            Rect rect1 = mItem.CachedRectTransform.get_rect();
            double height1 = (double) ((Rect) ref rect1).get_height();
            float num = (float) (y - height1) - mItem.Padding;
            itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, num, 0.0f));
            this.CheckIfNeedUpdateItemPos();
            if (indexInGroup > this.mCurReadyMaxItemIndex)
              this.mCurReadyMaxItemIndex = indexInGroup;
            return true;
          }
        }
      }
      else
      {
        if (this.mItemList.Count == 0)
        {
          float num = (float) this.mContainerTrans.get_anchoredPosition3D().y;
          if ((double) num > 0.0)
            num = 0.0f;
          int index = 0;
          float itemPos = -num;
          if (this.mSupportScrollBar && !this.GetPlusItemIndexAndPosAtGivenPos(-num, ref index, ref itemPos))
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, itemPos, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem = this.mItemList[this.mItemList.Count - 1];
        mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        if (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).y - this.mViewPortRectLocalCorners[1].y < (double) distanceForNew1)
        {
          if (mItem.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem.ItemIndexInGroup + 1;
          if (indexInGroup <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdateItemPos();
              return false;
            }
            if (this.mSupportScrollBar)
            {
              int itemIndex = indexInGroup;
              Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
              double height = (double) ((Rect) ref rect).get_height();
              double padding = (double) itemByIndexInGroup.Padding;
              this.SetItemSize(itemIndex, (float) height, (float) padding);
            }
            this.mItemList.Add(itemByIndexInGroup);
            // ISSUE: variable of the null type
            __Null y = mItem.CachedRectTransform.get_anchoredPosition3D().y;
            Rect rect1 = mItem.CachedRectTransform.get_rect();
            double height1 = (double) ((Rect) ref rect1).get_height();
            float num = (float) (y + height1) + mItem.Padding;
            itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemByIndexInGroup.StartPosOffset, num, 0.0f));
            this.CheckIfNeedUpdateItemPos();
            if (indexInGroup > this.mCurReadyMaxItemIndex)
              this.mCurReadyMaxItemIndex = indexInGroup;
            return true;
          }
        }
      }
      return false;
    }

    public bool UpdateForHorizontalListPart1(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      if (this.mItemTotalCount == 0)
      {
        if (this.mItemList.Count > 0)
          this.RecycleAllItem();
        return false;
      }
      if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        if (this.mItemList.Count == 0)
        {
          float num = (float) this.mContainerTrans.get_anchoredPosition3D().x;
          if ((double) num > 0.0)
            num = 0.0f;
          int index = 0;
          float itemPos = -num;
          if (this.mSupportScrollBar && !this.GetPlusItemIndexAndPosAtGivenPos(-num, ref index, ref itemPos))
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) width, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemPos, itemByIndexInGroup.StartPosOffset, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.IsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[1].x - vector3_2.x > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.IsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_3.x - this.mViewPortRectLocalCorners[2].x > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        if (this.mViewPortRectLocalCorners[1].x - vector3_1.x < (double) distanceForNew0)
        {
          if (mItem1.ItemIndexInGroup < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
            this.mNeedCheckNextMinItem = true;
          }
          int indexInGroup = mItem1.ItemIndexInGroup - 1;
          if (indexInGroup >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = indexInGroup;
                Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) itemByIndexInGroup.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Insert(0, itemByIndexInGroup);
              // ISSUE: variable of the null type
              __Null x = mItem1.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = itemByIndexInGroup.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x - width1) - itemByIndexInGroup.Padding;
              itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, itemByIndexInGroup.StartPosOffset, 0.0f));
              this.CheckIfNeedUpdateItemPos();
              if (indexInGroup < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = indexInGroup;
              return true;
            }
          }
        }
        if (vector3_4.x - this.mViewPortRectLocalCorners[2].x < (double) distanceForNew1)
        {
          if (mItem2.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem2.ItemIndexInGroup + 1;
          if (indexInGroup >= this.mItemIndexMap.Count || indexInGroup > this.mCurReadyMaxItemIndex && !this.mNeedCheckNextMaxItem)
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = false;
            this.CheckIfNeedUpdateItemPos();
          }
          else
          {
            if (this.mSupportScrollBar)
            {
              int itemIndex = indexInGroup;
              Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
              double width = (double) ((Rect) ref rect).get_width();
              double padding = (double) itemByIndexInGroup.Padding;
              this.SetItemSize(itemIndex, (float) width, (float) padding);
            }
            this.mItemList.Add(itemByIndexInGroup);
            // ISSUE: variable of the null type
            __Null x = mItem2.CachedRectTransform.get_anchoredPosition3D().x;
            Rect rect1 = mItem2.CachedRectTransform.get_rect();
            double width1 = (double) ((Rect) ref rect1).get_width();
            float num = (float) (x + width1) + mItem2.Padding;
            itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, itemByIndexInGroup.StartPosOffset, 0.0f));
            this.CheckIfNeedUpdateItemPos();
            if (indexInGroup > this.mCurReadyMaxItemIndex)
              this.mCurReadyMaxItemIndex = indexInGroup;
            return true;
          }
        }
      }
      else
      {
        if (this.mItemList.Count == 0)
        {
          float pos = (float) this.mContainerTrans.get_anchoredPosition3D().x;
          if ((double) pos < 0.0)
            pos = 0.0f;
          int index = 0;
          float itemPos = -pos;
          if (this.mSupportScrollBar)
          {
            if (!this.GetPlusItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos))
              return false;
            itemPos = -itemPos;
          }
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) width, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemPos, itemByIndexInGroup.StartPosOffset, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.IsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_1.x - this.mViewPortRectLocalCorners[2].x > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.IsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[1].x - vector3_4.x > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
            this.CheckIfNeedUpdateItemPos();
          return true;
        }
        if (vector3_2.x - this.mViewPortRectLocalCorners[2].x < (double) distanceForNew0)
        {
          if (mItem1.ItemIndexInGroup < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
            this.mNeedCheckNextMinItem = true;
          }
          int indexInGroup = mItem1.ItemIndexInGroup - 1;
          if (indexInGroup >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndexInGroup;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = indexInGroup;
                Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) itemByIndexInGroup.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Insert(0, itemByIndexInGroup);
              // ISSUE: variable of the null type
              __Null x = mItem1.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = itemByIndexInGroup.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x + width1) + itemByIndexInGroup.Padding;
              itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, itemByIndexInGroup.StartPosOffset, 0.0f));
              this.CheckIfNeedUpdateItemPos();
              if (indexInGroup < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = indexInGroup;
              return true;
            }
          }
        }
        if (this.mViewPortRectLocalCorners[1].x - vector3_3.x < (double) distanceForNew1)
        {
          if (mItem2.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem2.ItemIndexInGroup + 1;
          if (indexInGroup >= this.mItemIndexMap.Count || indexInGroup > this.mCurReadyMaxItemIndex && !this.mNeedCheckNextMaxItem)
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = false;
            this.CheckIfNeedUpdateItemPos();
          }
          else
          {
            if (this.mSupportScrollBar)
            {
              int itemIndex = indexInGroup;
              Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
              double width = (double) ((Rect) ref rect).get_width();
              double padding = (double) itemByIndexInGroup.Padding;
              this.SetItemSize(itemIndex, (float) width, (float) padding);
            }
            this.mItemList.Add(itemByIndexInGroup);
            // ISSUE: variable of the null type
            __Null x = mItem2.CachedRectTransform.get_anchoredPosition3D().x;
            Rect rect1 = mItem2.CachedRectTransform.get_rect();
            double width1 = (double) ((Rect) ref rect1).get_width();
            float num = (float) (x - width1) - mItem2.Padding;
            itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, itemByIndexInGroup.StartPosOffset, 0.0f));
            this.CheckIfNeedUpdateItemPos();
            if (indexInGroup > this.mCurReadyMaxItemIndex)
              this.mCurReadyMaxItemIndex = indexInGroup;
            return true;
          }
        }
      }
      return false;
    }

    public bool UpdateForHorizontalListPart2(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      if (this.mItemTotalCount == 0)
      {
        if (this.mItemList.Count > 0)
          this.RecycleAllItem();
        return false;
      }
      if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        if (this.mItemList.Count == 0)
        {
          float num = (float) this.mContainerTrans.get_anchoredPosition3D().x;
          if ((double) num > 0.0)
            num = 0.0f;
          int index = 0;
          float itemPos = -num;
          if (this.mSupportScrollBar && !this.GetPlusItemIndexAndPosAtGivenPos(-num, ref index, ref itemPos))
            return false;
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) width, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemPos, itemByIndexInGroup.StartPosOffset, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem = this.mItemList[this.mItemList.Count - 1];
        mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        if (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]).x - this.mViewPortRectLocalCorners[2].x < (double) distanceForNew1)
        {
          if (mItem.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem.ItemIndexInGroup + 1;
          if (indexInGroup <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdateItemPos();
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = indexInGroup;
                Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) itemByIndexInGroup.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Add(itemByIndexInGroup);
              // ISSUE: variable of the null type
              __Null x = mItem.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = mItem.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x + width1) + mItem.Padding;
              itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, itemByIndexInGroup.StartPosOffset, 0.0f));
              this.CheckIfNeedUpdateItemPos();
              if (indexInGroup > this.mCurReadyMaxItemIndex)
                this.mCurReadyMaxItemIndex = indexInGroup;
              return true;
            }
          }
        }
      }
      else
      {
        if (this.mItemList.Count == 0)
        {
          float pos = (float) this.mContainerTrans.get_anchoredPosition3D().x;
          if ((double) pos < 0.0)
            pos = 0.0f;
          int index = 0;
          float itemPos = -pos;
          if (this.mSupportScrollBar)
          {
            if (!this.GetPlusItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos))
              return false;
            itemPos = -itemPos;
          }
          LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(index);
          if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) itemByIndexInGroup.Padding;
            this.SetItemSize(itemIndex, (float) width, (float) padding);
          }
          this.mItemList.Add(itemByIndexInGroup);
          itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemPos, itemByIndexInGroup.StartPosOffset, 0.0f));
          return true;
        }
        LoopStaggeredGridViewItem mItem = this.mItemList[this.mItemList.Count - 1];
        mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        if (this.mViewPortRectLocalCorners[1].x - ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).x < (double) distanceForNew1)
        {
          if (mItem.ItemIndexInGroup > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
            this.mNeedCheckNextMaxItem = true;
          }
          int indexInGroup = mItem.ItemIndexInGroup + 1;
          if (indexInGroup <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopStaggeredGridViewItem itemByIndexInGroup = this.GetNewItemByIndexInGroup(indexInGroup);
            if (Object.op_Equality((Object) itemByIndexInGroup, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem.ItemIndexInGroup;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdateItemPos();
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = indexInGroup;
                Rect rect = itemByIndexInGroup.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) itemByIndexInGroup.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Add(itemByIndexInGroup);
              // ISSUE: variable of the null type
              __Null x = mItem.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = mItem.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x - width1) - mItem.Padding;
              itemByIndexInGroup.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, itemByIndexInGroup.StartPosOffset, 0.0f));
              this.CheckIfNeedUpdateItemPos();
              if (indexInGroup > this.mCurReadyMaxItemIndex)
                this.mCurReadyMaxItemIndex = indexInGroup;
              return true;
            }
          }
        }
      }
      return false;
    }

    public float GetContentPanelSize()
    {
      float num = (double) this.mItemPosMgr.mTotalSize <= 0.0 ? 0.0f : this.mItemPosMgr.mTotalSize - this.mLastItemPadding;
      if ((double) num < 0.0)
        num = 0.0f;
      return num;
    }

    public float GetShownItemPosMaxValue()
    {
      if (this.mItemList.Count == 0)
        return 0.0f;
      LoopStaggeredGridViewItem mItem = this.mItemList[this.mItemList.Count - 1];
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
        return Mathf.Abs(mItem.BottomY);
      if (this.mArrangeType == ListItemArrangeType.BottomToTop)
        return Mathf.Abs(mItem.TopY);
      if (this.mArrangeType == ListItemArrangeType.LeftToRight)
        return Mathf.Abs(mItem.RightX);
      return this.mArrangeType == ListItemArrangeType.RightToLeft ? Mathf.Abs(mItem.LeftX) : 0.0f;
    }

    public void CheckIfNeedUpdateItemPos()
    {
      if (this.mItemList.Count == 0)
        return;
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        if ((double) mItem1.TopY > 0.0 || mItem1.ItemIndexInGroup == this.mCurReadyMinItemIndex && (double) mItem1.TopY != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          float contentPanelSize = this.GetContentPanelSize();
          if (-(double) mItem2.BottomY <= (double) contentPanelSize && (mItem2.ItemIndexInGroup != this.mCurReadyMaxItemIndex || -(double) mItem2.BottomY == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        if ((double) mItem1.BottomY < 0.0 || mItem1.ItemIndexInGroup == this.mCurReadyMinItemIndex && (double) mItem1.BottomY != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          float contentPanelSize = this.GetContentPanelSize();
          if ((double) mItem2.TopY <= (double) contentPanelSize && (mItem2.ItemIndexInGroup != this.mCurReadyMaxItemIndex || (double) mItem2.TopY == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        if ((double) mItem1.LeftX < 0.0 || mItem1.ItemIndexInGroup == this.mCurReadyMinItemIndex && (double) mItem1.LeftX != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          float contentPanelSize = this.GetContentPanelSize();
          if ((double) mItem2.RightX <= (double) contentPanelSize && (mItem2.ItemIndexInGroup != this.mCurReadyMaxItemIndex || (double) mItem2.RightX == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
      else
      {
        if (this.mArrangeType != ListItemArrangeType.RightToLeft)
          return;
        LoopStaggeredGridViewItem mItem1 = this.mItemList[0];
        LoopStaggeredGridViewItem mItem2 = this.mItemList[this.mItemList.Count - 1];
        if ((double) mItem1.RightX > 0.0 || mItem1.ItemIndexInGroup == this.mCurReadyMinItemIndex && (double) mItem1.RightX != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          float contentPanelSize = this.GetContentPanelSize();
          if (-(double) mItem2.LeftX <= (double) contentPanelSize && (mItem2.ItemIndexInGroup != this.mCurReadyMaxItemIndex || -(double) mItem2.LeftX == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
    }

    public void UpdateAllShownItemsPos()
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = -this.GetItemPos(this.mItemList[0].ItemIndexInGroup);
        float num2 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopStaggeredGridViewItem mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(mItem.StartPosOffset, num2, 0.0f));
          double num3 = (double) num2;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          num2 = (float) (num3 - height) - mItem.Padding;
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = this.GetItemPos(this.mItemList[0].ItemIndexInGroup);
        float num2 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopStaggeredGridViewItem mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(mItem.StartPosOffset, num2, 0.0f));
          double num3 = (double) num2;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          num2 = (float) (num3 + height) + mItem.Padding;
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = this.GetItemPos(this.mItemList[0].ItemIndexInGroup);
        float num2 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopStaggeredGridViewItem mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(num2, mItem.StartPosOffset, 0.0f));
          double num3 = (double) num2;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          num2 = (float) (num3 + width) + mItem.Padding;
        }
      }
      else
      {
        if (this.mArrangeType != ListItemArrangeType.RightToLeft)
          return;
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = -this.GetItemPos(this.mItemList[0].ItemIndexInGroup);
        float num2 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopStaggeredGridViewItem mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(num2, mItem.StartPosOffset, 0.0f));
          double num3 = (double) num2;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          num2 = (float) (num3 - width) - mItem.Padding;
        }
      }
    }
  }
}
