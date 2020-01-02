// Decompiled with JetBrains decompiler
// Type: SuperScrollView.LoopGridView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class LoopGridView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
  {
    private Dictionary<string, GridItemPool> mItemPoolDict;
    private List<GridItemPool> mItemPoolList;
    [SerializeField]
    private List<GridViewItemPrefabConfData> mItemPrefabDataList;
    [SerializeField]
    private GridItemArrangeType mArrangeType;
    private RectTransform mContainerTrans;
    private ScrollRect mScrollRect;
    private RectTransform mScrollRectTransform;
    private RectTransform mViewPortRectTransform;
    private int mItemTotalCount;
    [SerializeField]
    private int mFixedRowOrColumnCount;
    [SerializeField]
    private RectOffset mPadding;
    [SerializeField]
    private Vector2 mItemPadding;
    [SerializeField]
    private Vector2 mItemSize;
    [SerializeField]
    private Vector2 mItemRecycleDistance;
    private Vector2 mItemSizeWithPadding;
    private Vector2 mStartPadding;
    private Vector2 mEndPadding;
    private Func<LoopGridView, int, int, int, LoopGridViewItem> mOnGetItemByRowColumn;
    private List<GridItemGroup> mItemGroupObjPool;
    private List<GridItemGroup> mItemGroupList;
    private bool mIsDraging;
    private int mRowCount;
    private int mColumnCount;
    public Action<PointerEventData> mOnBeginDragAction;
    public Action<PointerEventData> mOnDragingAction;
    public Action<PointerEventData> mOnEndDragAction;
    private float mSmoothDumpVel;
    private float mSmoothDumpRate;
    private float mSnapFinishThreshold;
    private float mSnapVecThreshold;
    [SerializeField]
    private bool mItemSnapEnable;
    [SerializeField]
    private GridFixedType mGridFixedType;
    public Action<LoopGridView, LoopGridViewItem> mOnSnapItemFinished;
    public Action<LoopGridView> mOnSnapNearestChanged;
    private int mLeftSnapUpdateExtraCount;
    [SerializeField]
    private Vector2 mViewPortSnapPivot;
    [SerializeField]
    private Vector2 mItemSnapPivot;
    private LoopGridView.SnapData mCurSnapData;
    private Vector3 mLastSnapCheckPos;
    private bool mListViewInited;
    private int mListUpdateCheckFrameCount;
    private LoopGridView.ItemRangeData mCurFrameItemRangeData;
    private int mNeedCheckContentPosLeftCount;
    private ClickEventListener mScrollBarClickEventListener1;
    private ClickEventListener mScrollBarClickEventListener2;
    private RowColumnPair mCurSnapNearestItemRowColumn;

    public LoopGridView()
    {
      base.\u002Ector();
    }

    public GridItemArrangeType ArrangeType
    {
      get
      {
        return this.mArrangeType;
      }
      set
      {
        this.mArrangeType = value;
      }
    }

    public List<GridViewItemPrefabConfData> ItemPrefabDataList
    {
      get
      {
        return this.mItemPrefabDataList;
      }
    }

    public int ItemTotalCount
    {
      get
      {
        return this.mItemTotalCount;
      }
    }

    public RectTransform ContainerTrans
    {
      get
      {
        return this.mContainerTrans;
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

    public ScrollRect ScrollRect
    {
      get
      {
        return this.mScrollRect;
      }
    }

    public bool IsDraging
    {
      get
      {
        return this.mIsDraging;
      }
    }

    public bool ItemSnapEnable
    {
      get
      {
        return this.mItemSnapEnable;
      }
      set
      {
        this.mItemSnapEnable = value;
      }
    }

    public Vector2 ItemSize
    {
      get
      {
        return this.mItemSize;
      }
      set
      {
        this.SetItemSize(value);
      }
    }

    public Vector2 ItemPadding
    {
      get
      {
        return this.mItemPadding;
      }
      set
      {
        this.SetItemPadding(value);
      }
    }

    public Vector2 ItemSizeWithPadding
    {
      get
      {
        return this.mItemSizeWithPadding;
      }
    }

    public RectOffset Padding
    {
      get
      {
        return this.mPadding;
      }
      set
      {
        this.SetPadding(value);
      }
    }

    public GridViewItemPrefabConfData GetItemPrefabConfData(
      string prefabName)
    {
      foreach (GridViewItemPrefabConfData mItemPrefabData in this.mItemPrefabDataList)
      {
        if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab, (Object) null))
          Debug.LogError((object) "A item prefab is null ");
        else if (prefabName == ((Object) mItemPrefabData.mItemPrefab).get_name())
          return mItemPrefabData;
      }
      return (GridViewItemPrefabConfData) null;
    }

    public void InitGridView(
      int itemTotalCount,
      Func<LoopGridView, int, int, int, LoopGridViewItem> onGetItemByRowColumn,
      LoopGridViewSettingParam settingParam = null,
      LoopGridViewInitParam initParam = null)
    {
      if (this.mListViewInited)
      {
        Debug.LogError((object) "LoopGridView.InitListView method can be called only once.");
      }
      else
      {
        this.mListViewInited = true;
        if (itemTotalCount < 0)
        {
          Debug.LogError((object) "itemTotalCount is  < 0");
          itemTotalCount = 0;
        }
        if (settingParam != null)
          this.UpdateFromSettingParam(settingParam);
        if (initParam != null)
        {
          this.mSmoothDumpRate = initParam.mSmoothDumpRate;
          this.mSnapFinishThreshold = initParam.mSnapFinishThreshold;
          this.mSnapVecThreshold = initParam.mSnapVecThreshold;
        }
        this.mScrollRect = (ScrollRect) ((Component) this).get_gameObject().GetComponent<ScrollRect>();
        if (Object.op_Equality((Object) this.mScrollRect, (Object) null))
        {
          Debug.LogError((object) "ListView Init Failed! ScrollRect component not found!");
        }
        else
        {
          this.mCurSnapData.Clear();
          this.mScrollRectTransform = (RectTransform) ((Component) this.mScrollRect).GetComponent<RectTransform>();
          this.mContainerTrans = this.mScrollRect.get_content();
          this.mViewPortRectTransform = this.mScrollRect.get_viewport();
          if (Object.op_Equality((Object) this.mViewPortRectTransform, (Object) null))
            this.mViewPortRectTransform = this.mScrollRectTransform;
          if (this.mScrollRect.get_horizontalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_horizontalScrollbar(), (Object) null))
            Debug.LogError((object) "ScrollRect.horizontalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
          if (this.mScrollRect.get_verticalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_verticalScrollbar(), (Object) null))
            Debug.LogError((object) "ScrollRect.verticalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
          this.SetScrollbarListener();
          this.AdjustViewPortPivot();
          this.AdjustContainerAnchorAndPivot();
          this.InitItemPool();
          this.mOnGetItemByRowColumn = onGetItemByRowColumn;
          this.mNeedCheckContentPosLeftCount = 4;
          this.mCurSnapData.Clear();
          this.mItemTotalCount = itemTotalCount;
          this.UpdateAllGridSetting();
        }
      }
    }

    public void SetListItemCount(int itemCount, bool resetPos = true)
    {
      if (itemCount < 0 || itemCount == this.mItemTotalCount)
        return;
      this.mCurSnapData.Clear();
      this.mItemTotalCount = itemCount;
      this.UpdateColumnRowCount();
      this.UpdateContentSize();
      this.ForceToCheckContentPos();
      if (this.mItemTotalCount == 0)
      {
        this.RecycleAllItem();
        this.ClearAllTmpRecycledItem();
      }
      else
      {
        this.VaildAndSetContainerPos();
        this.UpdateGridViewContent();
        this.ClearAllTmpRecycledItem();
        if (!resetPos)
          return;
        this.MovePanelToItemByRowColumn(0, 0, 0.0f, 0.0f);
      }
    }

    public LoopGridViewItem NewListViewItem(string itemPrefabName)
    {
      GridItemPool gridItemPool = (GridItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(itemPrefabName, out gridItemPool))
        return (LoopGridViewItem) null;
      LoopGridViewItem loopGridViewItem = gridItemPool.GetItem();
      RectTransform component = (RectTransform) ((Component) loopGridViewItem).GetComponent<RectTransform>();
      ((Transform) component).SetParent((Transform) this.mContainerTrans);
      ((Transform) component).set_localScale(Vector3.get_one());
      component.set_anchoredPosition3D(Vector3.get_zero());
      ((Transform) component).set_localEulerAngles(Vector3.get_zero());
      loopGridViewItem.ParentGridView = this;
      return loopGridViewItem;
    }

    public void RefreshItemByItemIndex(int itemIndex)
    {
      if (itemIndex < 0 || itemIndex >= this.ItemTotalCount || this.mItemGroupList.Count == 0)
        return;
      RowColumnPair columnByItemIndex = this.GetRowColumnByItemIndex(itemIndex);
      this.RefreshItemByRowColumn(columnByItemIndex.mRow, columnByItemIndex.mColumn);
    }

    public void RefreshItemByRowColumn(int row, int column)
    {
      if (this.mItemGroupList.Count == 0)
        return;
      if (this.mGridFixedType == GridFixedType.ColumnCountFixed)
      {
        GridItemGroup shownGroup = this.GetShownGroup(row);
        if (shownGroup == null)
          return;
        LoopGridViewItem itemByColumn = shownGroup.GetItemByColumn(column);
        if (Object.op_Equality((Object) itemByColumn, (Object) null))
          return;
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(row, column);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          return;
        Vector3 anchoredPosition3D = itemByColumn.CachedRectTransform.get_anchoredPosition3D();
        shownGroup.ReplaceItem(itemByColumn, newItemByRowColumn);
        this.RecycleItemTmp(itemByColumn);
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
        this.ClearAllTmpRecycledItem();
      }
      else
      {
        GridItemGroup shownGroup = this.GetShownGroup(column);
        if (shownGroup == null)
          return;
        LoopGridViewItem itemByRow = shownGroup.GetItemByRow(row);
        if (Object.op_Equality((Object) itemByRow, (Object) null))
          return;
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(row, column);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          return;
        Vector3 anchoredPosition3D = itemByRow.CachedRectTransform.get_anchoredPosition3D();
        shownGroup.ReplaceItem(itemByRow, newItemByRowColumn);
        this.RecycleItemTmp(itemByRow);
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
        this.ClearAllTmpRecycledItem();
      }
    }

    public void ClearSnapData()
    {
      this.mCurSnapData.Clear();
    }

    public void SetSnapTargetItemRowColumn(int row, int column)
    {
      if (row < 0)
        row = 0;
      if (column < 0)
        column = 0;
      this.mCurSnapData.mSnapTarget.mRow = row;
      this.mCurSnapData.mSnapTarget.mColumn = column;
      this.mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
      this.mCurSnapData.mIsForceSnapTo = true;
    }

    public RowColumnPair CurSnapNearestItemRowColumn
    {
      get
      {
        return this.mCurSnapNearestItemRowColumn;
      }
    }

    public void ForceSnapUpdateCheck()
    {
      if (this.mLeftSnapUpdateExtraCount > 0)
        return;
      this.mLeftSnapUpdateExtraCount = 1;
    }

    public void ForceToCheckContentPos()
    {
      if (this.mNeedCheckContentPosLeftCount > 0)
        return;
      this.mNeedCheckContentPosLeftCount = 1;
    }

    public void MovePanelToItemByIndex(int itemIndex, float offsetX = 0.0f, float offsetY = 0.0f)
    {
      if (this.ItemTotalCount == 0)
        return;
      if (itemIndex >= this.ItemTotalCount)
        itemIndex = this.ItemTotalCount - 1;
      if (itemIndex < 0)
        itemIndex = 0;
      RowColumnPair columnByItemIndex = this.GetRowColumnByItemIndex(itemIndex);
      this.MovePanelToItemByRowColumn(columnByItemIndex.mRow, columnByItemIndex.mColumn, offsetX, offsetY);
    }

    public void MovePanelToItemByRowColumn(int row, int column, float offsetX = 0.0f, float offsetY = 0.0f)
    {
      this.mScrollRect.StopMovement();
      this.mCurSnapData.Clear();
      if (this.mItemTotalCount == 0)
        return;
      Vector2 itemPos = this.GetItemPos(row, column);
      Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
      if (this.mScrollRect.get_horizontal())
      {
        Rect rect = this.ContainerTrans.get_rect();
        float num1 = Mathf.Max(((Rect) ref rect).get_width() - this.ViewPortWidth, 0.0f);
        if ((double) num1 > 0.0)
        {
          float num2 = (float) -itemPos.x + offsetX;
          float num3 = Mathf.Min(Mathf.Abs(num2), num1) * Mathf.Sign(num2);
          anchoredPosition3D.x = (__Null) (double) num3;
        }
      }
      if (this.mScrollRect.get_vertical())
      {
        Rect rect = this.ContainerTrans.get_rect();
        float num1 = Mathf.Max(((Rect) ref rect).get_height() - this.ViewPortHeight, 0.0f);
        if ((double) num1 > 0.0)
        {
          float num2 = (float) -itemPos.y + offsetY;
          float num3 = Mathf.Min(Mathf.Abs(num2), num1) * Mathf.Sign(num2);
          anchoredPosition3D.y = (__Null) (double) num3;
        }
      }
      if (Vector3.op_Inequality(anchoredPosition3D, this.mContainerTrans.get_anchoredPosition3D()))
        this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
      this.VaildAndSetContainerPos();
      this.ForceToCheckContentPos();
    }

    public void RefreshAllShownItem()
    {
      if (this.mItemGroupList.Count == 0)
        return;
      this.ForceToCheckContentPos();
      this.RecycleAllItem();
      this.UpdateGridViewContent();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      this.mCurSnapData.Clear();
      this.mIsDraging = true;
      if (this.mOnBeginDragAction == null)
        return;
      this.mOnBeginDragAction(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      this.mIsDraging = false;
      this.ForceSnapUpdateCheck();
      if (this.mOnEndDragAction == null)
        return;
      this.mOnEndDragAction(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
      if (eventData.get_button() != null || this.mOnDragingAction == null)
        return;
      this.mOnDragingAction(eventData);
    }

    public int GetItemIndexByRowColumn(int row, int column)
    {
      return this.mGridFixedType == GridFixedType.ColumnCountFixed ? row * this.mFixedRowOrColumnCount + column : column * this.mFixedRowOrColumnCount + row;
    }

    public RowColumnPair GetRowColumnByItemIndex(int itemIndex)
    {
      if (itemIndex < 0)
        itemIndex = 0;
      if (this.mGridFixedType == GridFixedType.ColumnCountFixed)
        return new RowColumnPair(itemIndex / this.mFixedRowOrColumnCount, itemIndex % this.mFixedRowOrColumnCount);
      int column1 = itemIndex / this.mFixedRowOrColumnCount;
      return new RowColumnPair(itemIndex % this.mFixedRowOrColumnCount, column1);
    }

    public Vector2 GetItemAbsPos(int row, int column)
    {
      return new Vector2((float) (this.mStartPadding.x + (double) column * this.mItemSizeWithPadding.x), (float) (this.mStartPadding.y + (double) row * this.mItemSizeWithPadding.y));
    }

    public Vector2 GetItemPos(int row, int column)
    {
      Vector2 itemAbsPos = this.GetItemAbsPos(row, column);
      float x = (float) itemAbsPos.x;
      float y = (float) itemAbsPos.y;
      if (this.ArrangeType == GridItemArrangeType.TopLeftToBottomRight)
        return new Vector2(x, -y);
      if (this.ArrangeType == GridItemArrangeType.BottomLeftToTopRight)
        return new Vector2(x, y);
      if (this.ArrangeType == GridItemArrangeType.TopRightToBottomLeft)
        return new Vector2(-x, -y);
      return this.ArrangeType == GridItemArrangeType.BottomRightToTopLeft ? new Vector2(-x, y) : Vector2.get_zero();
    }

    public LoopGridViewItem GetShownItemByItemIndex(int itemIndex)
    {
      if (itemIndex < 0 || itemIndex >= this.ItemTotalCount)
        return (LoopGridViewItem) null;
      if (this.mItemGroupList.Count == 0)
        return (LoopGridViewItem) null;
      RowColumnPair columnByItemIndex = this.GetRowColumnByItemIndex(itemIndex);
      return this.GetShownItemByRowColumn(columnByItemIndex.mRow, columnByItemIndex.mColumn);
    }

    public LoopGridViewItem GetShownItemByRowColumn(int row, int column)
    {
      if (this.mItemGroupList.Count == 0)
        return (LoopGridViewItem) null;
      return this.mGridFixedType == GridFixedType.ColumnCountFixed ? this.GetShownGroup(row)?.GetItemByColumn(column) : this.GetShownGroup(column)?.GetItemByRow(row);
    }

    public void UpdateAllGridSetting()
    {
      this.UpdateStartEndPadding();
      this.UpdateItemSize();
      this.UpdateColumnRowCount();
      this.UpdateContentSize();
      this.ForceSnapUpdateCheck();
      this.ForceToCheckContentPos();
    }

    public void SetGridFixedGroupCount(GridFixedType fixedType, int count)
    {
      if (this.mGridFixedType == fixedType && this.mFixedRowOrColumnCount == count)
        return;
      this.mGridFixedType = fixedType;
      this.mFixedRowOrColumnCount = count;
      this.UpdateColumnRowCount();
      this.UpdateContentSize();
      if (this.mItemGroupList.Count == 0)
        return;
      this.RecycleAllItem();
      this.ForceSnapUpdateCheck();
      this.ForceToCheckContentPos();
    }

    public void SetItemSize(Vector2 newSize)
    {
      if (Vector2.op_Equality(newSize, this.mItemSize))
        return;
      this.mItemSize = newSize;
      this.UpdateItemSize();
      this.UpdateContentSize();
      if (this.mItemGroupList.Count == 0)
        return;
      this.RecycleAllItem();
      this.ForceSnapUpdateCheck();
      this.ForceToCheckContentPos();
    }

    public void SetItemPadding(Vector2 newPadding)
    {
      if (Vector2.op_Equality(newPadding, this.mItemPadding))
        return;
      this.mItemPadding = newPadding;
      this.UpdateItemSize();
      this.UpdateContentSize();
      if (this.mItemGroupList.Count == 0)
        return;
      this.RecycleAllItem();
      this.ForceSnapUpdateCheck();
      this.ForceToCheckContentPos();
    }

    public void SetPadding(RectOffset newPadding)
    {
      if (newPadding == this.mPadding)
        return;
      this.mPadding = newPadding;
      this.UpdateStartEndPadding();
      this.UpdateContentSize();
      if (this.mItemGroupList.Count == 0)
        return;
      this.RecycleAllItem();
      this.ForceSnapUpdateCheck();
      this.ForceToCheckContentPos();
    }

    public void UpdateContentSize()
    {
      float num1 = (float) (this.mStartPadding.x + (double) this.mColumnCount * this.mItemSizeWithPadding.x - this.mItemPadding.x + this.mEndPadding.x);
      float num2 = (float) (this.mStartPadding.y + (double) this.mRowCount * this.mItemSizeWithPadding.y - this.mItemPadding.y + this.mEndPadding.y);
      Rect rect1 = this.mContainerTrans.get_rect();
      if ((double) ((Rect) ref rect1).get_height() != (double) num2)
        this.mContainerTrans.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, num2);
      Rect rect2 = this.mContainerTrans.get_rect();
      if ((double) ((Rect) ref rect2).get_width() == (double) num1)
        return;
      this.mContainerTrans.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, num1);
    }

    public void VaildAndSetContainerPos()
    {
      Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
      this.mContainerTrans.set_anchoredPosition3D(Vector2.op_Implicit(this.GetContainerVaildPos((float) anchoredPosition3D.x, (float) anchoredPosition3D.y)));
    }

    public void ClearAllTmpRecycledItem()
    {
      int count = this.mItemPoolList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemPoolList[index].ClearTmpRecycledItem();
    }

    public void RecycleAllItem()
    {
      foreach (GridItemGroup mItemGroup in this.mItemGroupList)
        this.RecycleItemGroupTmp(mItemGroup);
      this.mItemGroupList.Clear();
    }

    public void UpdateGridViewContent()
    {
      ++this.mListUpdateCheckFrameCount;
      if (this.mItemTotalCount == 0)
      {
        if (this.mItemGroupList.Count <= 0)
          return;
        this.RecycleAllItem();
      }
      else
      {
        this.UpdateCurFrameItemRangeData();
        if (this.mGridFixedType == GridFixedType.ColumnCountFixed)
        {
          int count1 = this.mItemGroupList.Count;
          int mMinRow = this.mCurFrameItemRangeData.mMinRow;
          int mMaxRow = this.mCurFrameItemRangeData.mMaxRow;
          for (int index = count1 - 1; index >= 0; --index)
          {
            GridItemGroup mItemGroup = this.mItemGroupList[index];
            if (mItemGroup.GroupIndex < mMinRow || mItemGroup.GroupIndex > mMaxRow)
            {
              this.RecycleItemGroupTmp(mItemGroup);
              this.mItemGroupList.RemoveAt(index);
            }
          }
          if (this.mItemGroupList.Count == 0)
            this.mItemGroupList.Add(this.CreateItemGroup(mMinRow));
          while (this.mItemGroupList[0].GroupIndex > mMinRow)
            this.mItemGroupList.Insert(0, this.CreateItemGroup(this.mItemGroupList[0].GroupIndex - 1));
          while (this.mItemGroupList[this.mItemGroupList.Count - 1].GroupIndex < mMaxRow)
            this.mItemGroupList.Add(this.CreateItemGroup(this.mItemGroupList[this.mItemGroupList.Count - 1].GroupIndex + 1));
          int count2 = this.mItemGroupList.Count;
          for (int index = 0; index < count2; ++index)
            this.UpdateRowItemGroupForRecycleAndNew(this.mItemGroupList[index]);
        }
        else
        {
          int count1 = this.mItemGroupList.Count;
          int mMinColumn = this.mCurFrameItemRangeData.mMinColumn;
          int mMaxColumn = this.mCurFrameItemRangeData.mMaxColumn;
          for (int index = count1 - 1; index >= 0; --index)
          {
            GridItemGroup mItemGroup = this.mItemGroupList[index];
            if (mItemGroup.GroupIndex < mMinColumn || mItemGroup.GroupIndex > mMaxColumn)
            {
              this.RecycleItemGroupTmp(mItemGroup);
              this.mItemGroupList.RemoveAt(index);
            }
          }
          if (this.mItemGroupList.Count == 0)
            this.mItemGroupList.Add(this.CreateItemGroup(mMinColumn));
          while (this.mItemGroupList[0].GroupIndex > mMinColumn)
            this.mItemGroupList.Insert(0, this.CreateItemGroup(this.mItemGroupList[0].GroupIndex - 1));
          while (this.mItemGroupList[this.mItemGroupList.Count - 1].GroupIndex < mMaxColumn)
            this.mItemGroupList.Add(this.CreateItemGroup(this.mItemGroupList[this.mItemGroupList.Count - 1].GroupIndex + 1));
          int count2 = this.mItemGroupList.Count;
          for (int index = 0; index < count2; ++index)
            this.UpdateColumnItemGroupForRecycleAndNew(this.mItemGroupList[index]);
        }
      }
    }

    public void UpdateStartEndPadding()
    {
      if (this.ArrangeType == GridItemArrangeType.TopLeftToBottomRight)
      {
        this.mStartPadding.x = (__Null) (double) this.mPadding.get_left();
        this.mStartPadding.y = (__Null) (double) this.mPadding.get_top();
        this.mEndPadding.x = (__Null) (double) this.mPadding.get_right();
        this.mEndPadding.y = (__Null) (double) this.mPadding.get_bottom();
      }
      else if (this.ArrangeType == GridItemArrangeType.BottomLeftToTopRight)
      {
        this.mStartPadding.x = (__Null) (double) this.mPadding.get_left();
        this.mStartPadding.y = (__Null) (double) this.mPadding.get_bottom();
        this.mEndPadding.x = (__Null) (double) this.mPadding.get_right();
        this.mEndPadding.y = (__Null) (double) this.mPadding.get_top();
      }
      else if (this.ArrangeType == GridItemArrangeType.TopRightToBottomLeft)
      {
        this.mStartPadding.x = (__Null) (double) this.mPadding.get_right();
        this.mStartPadding.y = (__Null) (double) this.mPadding.get_top();
        this.mEndPadding.x = (__Null) (double) this.mPadding.get_left();
        this.mEndPadding.y = (__Null) (double) this.mPadding.get_bottom();
      }
      else
      {
        if (this.ArrangeType != GridItemArrangeType.BottomRightToTopLeft)
          return;
        this.mStartPadding.x = (__Null) (double) this.mPadding.get_right();
        this.mStartPadding.y = (__Null) (double) this.mPadding.get_bottom();
        this.mEndPadding.x = (__Null) (double) this.mPadding.get_left();
        this.mEndPadding.y = (__Null) (double) this.mPadding.get_top();
      }
    }

    public void UpdateItemSize()
    {
      if (this.mItemSize.x > 0.0 && this.mItemSize.y > 0.0)
      {
        this.mItemSizeWithPadding = Vector2.op_Addition(this.mItemSize, this.mItemPadding);
      }
      else
      {
        if (this.mItemPrefabDataList.Count != 0)
        {
          GameObject mItemPrefab = this.mItemPrefabDataList[0].mItemPrefab;
          if (!Object.op_Equality((Object) mItemPrefab, (Object) null))
          {
            RectTransform component = (RectTransform) mItemPrefab.GetComponent<RectTransform>();
            if (!Object.op_Equality((Object) component, (Object) null))
            {
              Rect rect = component.get_rect();
              this.mItemSize = ((Rect) ref rect).get_size();
              this.mItemSizeWithPadding = Vector2.op_Addition(this.mItemSize, this.mItemPadding);
            }
          }
        }
        if (this.mItemSize.x > 0.0 && this.mItemSize.y > 0.0)
          return;
        Debug.LogError((object) "Error, ItemSize is invaild.");
      }
    }

    public void UpdateColumnRowCount()
    {
      if (this.mGridFixedType == GridFixedType.ColumnCountFixed)
      {
        this.mColumnCount = this.mFixedRowOrColumnCount;
        this.mRowCount = this.mItemTotalCount / this.mColumnCount;
        if (this.mItemTotalCount % this.mColumnCount > 0)
          ++this.mRowCount;
        if (this.mItemTotalCount > this.mColumnCount)
          return;
        this.mColumnCount = this.mItemTotalCount;
      }
      else
      {
        this.mRowCount = this.mFixedRowOrColumnCount;
        this.mColumnCount = this.mItemTotalCount / this.mRowCount;
        if (this.mItemTotalCount % this.mRowCount > 0)
          ++this.mColumnCount;
        if (this.mItemTotalCount > this.mRowCount)
          return;
        this.mRowCount = this.mItemTotalCount;
      }
    }

    private bool IsContainerTransCanMove()
    {
      if (this.mItemTotalCount == 0)
        return false;
      if (this.mScrollRect.get_horizontal())
      {
        Rect rect = this.ContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_width() > (double) this.ViewPortWidth)
          return true;
      }
      if (this.mScrollRect.get_vertical())
      {
        Rect rect = this.ContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_height() > (double) this.ViewPortHeight)
          return true;
      }
      return false;
    }

    private void RecycleItemGroupTmp(GridItemGroup group)
    {
      if (group == null)
        return;
      while (Object.op_Inequality((Object) group.First, (Object) null))
        this.RecycleItemTmp(group.RemoveFirst());
      group.Clear();
      this.RecycleOneItemGroupObj(group);
    }

    private void RecycleItemTmp(LoopGridViewItem item)
    {
      if (Object.op_Equality((Object) item, (Object) null) || string.IsNullOrEmpty(item.ItemPrefabName))
        return;
      GridItemPool gridItemPool = (GridItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(item.ItemPrefabName, out gridItemPool))
        return;
      gridItemPool.RecycleItem(item);
    }

    private void AdjustViewPortPivot()
    {
      RectTransform portRectTransform = this.mViewPortRectTransform;
      if (this.ArrangeType == GridItemArrangeType.TopLeftToBottomRight)
        portRectTransform.set_pivot(new Vector2(0.0f, 1f));
      else if (this.ArrangeType == GridItemArrangeType.BottomLeftToTopRight)
        portRectTransform.set_pivot(new Vector2(0.0f, 0.0f));
      else if (this.ArrangeType == GridItemArrangeType.TopRightToBottomLeft)
      {
        portRectTransform.set_pivot(new Vector2(1f, 1f));
      }
      else
      {
        if (this.ArrangeType != GridItemArrangeType.BottomRightToTopLeft)
          return;
        portRectTransform.set_pivot(new Vector2(1f, 0.0f));
      }
    }

    private void AdjustContainerAnchorAndPivot()
    {
      RectTransform containerTrans = this.ContainerTrans;
      if (this.ArrangeType == GridItemArrangeType.TopLeftToBottomRight)
      {
        containerTrans.set_anchorMin(new Vector2(0.0f, 1f));
        containerTrans.set_anchorMax(new Vector2(0.0f, 1f));
        containerTrans.set_pivot(new Vector2(0.0f, 1f));
      }
      else if (this.ArrangeType == GridItemArrangeType.BottomLeftToTopRight)
      {
        containerTrans.set_anchorMin(new Vector2(0.0f, 0.0f));
        containerTrans.set_anchorMax(new Vector2(0.0f, 0.0f));
        containerTrans.set_pivot(new Vector2(0.0f, 0.0f));
      }
      else if (this.ArrangeType == GridItemArrangeType.TopRightToBottomLeft)
      {
        containerTrans.set_anchorMin(new Vector2(1f, 1f));
        containerTrans.set_anchorMax(new Vector2(1f, 1f));
        containerTrans.set_pivot(new Vector2(1f, 1f));
      }
      else
      {
        if (this.ArrangeType != GridItemArrangeType.BottomRightToTopLeft)
          return;
        containerTrans.set_anchorMin(new Vector2(1f, 0.0f));
        containerTrans.set_anchorMax(new Vector2(1f, 0.0f));
        containerTrans.set_pivot(new Vector2(1f, 0.0f));
      }
    }

    private void AdjustItemAnchorAndPivot(RectTransform rtf)
    {
      if (this.ArrangeType == GridItemArrangeType.TopLeftToBottomRight)
      {
        rtf.set_anchorMin(new Vector2(0.0f, 1f));
        rtf.set_anchorMax(new Vector2(0.0f, 1f));
        rtf.set_pivot(new Vector2(0.0f, 1f));
      }
      else if (this.ArrangeType == GridItemArrangeType.BottomLeftToTopRight)
      {
        rtf.set_anchorMin(new Vector2(0.0f, 0.0f));
        rtf.set_anchorMax(new Vector2(0.0f, 0.0f));
        rtf.set_pivot(new Vector2(0.0f, 0.0f));
      }
      else if (this.ArrangeType == GridItemArrangeType.TopRightToBottomLeft)
      {
        rtf.set_anchorMin(new Vector2(1f, 1f));
        rtf.set_anchorMax(new Vector2(1f, 1f));
        rtf.set_pivot(new Vector2(1f, 1f));
      }
      else
      {
        if (this.ArrangeType != GridItemArrangeType.BottomRightToTopLeft)
          return;
        rtf.set_anchorMin(new Vector2(1f, 0.0f));
        rtf.set_anchorMax(new Vector2(1f, 0.0f));
        rtf.set_pivot(new Vector2(1f, 0.0f));
      }
    }

    private void InitItemPool()
    {
      foreach (GridViewItemPrefabConfData mItemPrefabData in this.mItemPrefabDataList)
      {
        if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab, (Object) null))
        {
          Debug.LogError((object) "A item prefab is null ");
        }
        else
        {
          string name = ((Object) mItemPrefabData.mItemPrefab).get_name();
          if (this.mItemPoolDict.ContainsKey(name))
          {
            Debug.LogError((object) ("A item prefab with name " + name + " has existed!"));
          }
          else
          {
            RectTransform component = (RectTransform) mItemPrefabData.mItemPrefab.GetComponent<RectTransform>();
            if (Object.op_Equality((Object) component, (Object) null))
            {
              Debug.LogError((object) ("RectTransform component is not found in the prefab " + name));
            }
            else
            {
              this.AdjustItemAnchorAndPivot(component);
              if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab.GetComponent<LoopGridViewItem>(), (Object) null))
                mItemPrefabData.mItemPrefab.AddComponent<LoopGridViewItem>();
              GridItemPool gridItemPool = new GridItemPool();
              gridItemPool.Init(mItemPrefabData.mItemPrefab, mItemPrefabData.mInitCreateCount, this.mContainerTrans);
              this.mItemPoolDict.Add(name, gridItemPool);
              this.mItemPoolList.Add(gridItemPool);
            }
          }
        }
      }
    }

    private LoopGridViewItem GetNewItemByRowColumn(int row, int column)
    {
      int indexByRowColumn = this.GetItemIndexByRowColumn(row, column);
      if (indexByRowColumn < 0 || indexByRowColumn >= this.ItemTotalCount)
        return (LoopGridViewItem) null;
      LoopGridViewItem loopGridViewItem = this.mOnGetItemByRowColumn(this, indexByRowColumn, row, column);
      if (Object.op_Equality((Object) loopGridViewItem, (Object) null))
        return (LoopGridViewItem) null;
      loopGridViewItem.NextItem = (LoopGridViewItem) null;
      loopGridViewItem.PrevItem = (LoopGridViewItem) null;
      loopGridViewItem.Row = row;
      loopGridViewItem.Column = column;
      loopGridViewItem.ItemIndex = indexByRowColumn;
      loopGridViewItem.ItemCreatedCheckFrameCount = this.mListUpdateCheckFrameCount;
      return loopGridViewItem;
    }

    private RowColumnPair GetCeilItemRowColumnAtGivenAbsPos(float ax, float ay)
    {
      ax = Mathf.Abs(ax);
      ay = Mathf.Abs(ay);
      int row1 = Mathf.CeilToInt((float) (((double) ay - this.mStartPadding.y) / this.mItemSizeWithPadding.y)) - 1;
      int column1 = Mathf.CeilToInt((float) (((double) ax - this.mStartPadding.x) / this.mItemSizeWithPadding.x)) - 1;
      if (row1 < 0)
        row1 = 0;
      if (row1 >= this.mRowCount)
        row1 = this.mRowCount - 1;
      if (column1 < 0)
        column1 = 0;
      if (column1 >= this.mColumnCount)
        column1 = this.mColumnCount - 1;
      return new RowColumnPair(row1, column1);
    }

    private void Update()
    {
      if (!this.mListViewInited)
        return;
      this.UpdateSnapMove(false, false);
      this.UpdateGridViewContent();
      this.ClearAllTmpRecycledItem();
    }

    private GridItemGroup CreateItemGroup(int groupIndex)
    {
      GridItemGroup oneItemGroupObj = this.GetOneItemGroupObj();
      oneItemGroupObj.GroupIndex = groupIndex;
      return oneItemGroupObj;
    }

    private Vector2 GetContainerMovedDistance()
    {
      Vector2 containerVaildPos = this.GetContainerVaildPos((float) this.ContainerTrans.get_anchoredPosition3D().x, (float) this.ContainerTrans.get_anchoredPosition3D().y);
      return new Vector2(Mathf.Abs((float) containerVaildPos.x), Mathf.Abs((float) containerVaildPos.y));
    }

    private Vector2 GetContainerVaildPos(float curX, float curY)
    {
      Rect rect1 = this.ContainerTrans.get_rect();
      float num1 = Mathf.Max(((Rect) ref rect1).get_width() - this.ViewPortWidth, 0.0f);
      Rect rect2 = this.ContainerTrans.get_rect();
      float num2 = Mathf.Max(((Rect) ref rect2).get_height() - this.ViewPortHeight, 0.0f);
      if (this.mArrangeType == GridItemArrangeType.TopLeftToBottomRight)
      {
        curX = Mathf.Clamp(curX, -num1, 0.0f);
        curY = Mathf.Clamp(curY, 0.0f, num2);
      }
      else if (this.mArrangeType == GridItemArrangeType.BottomLeftToTopRight)
      {
        curX = Mathf.Clamp(curX, -num1, 0.0f);
        curY = Mathf.Clamp(curY, -num2, 0.0f);
      }
      else if (this.mArrangeType == GridItemArrangeType.BottomRightToTopLeft)
      {
        curX = Mathf.Clamp(curX, 0.0f, num1);
        curY = Mathf.Clamp(curY, -num2, 0.0f);
      }
      else if (this.mArrangeType == GridItemArrangeType.TopRightToBottomLeft)
      {
        curX = Mathf.Clamp(curX, 0.0f, num1);
        curY = Mathf.Clamp(curY, 0.0f, num2);
      }
      return new Vector2(curX, curY);
    }

    private void UpdateCurFrameItemRangeData()
    {
      Vector2 containerMovedDistance = this.GetContainerMovedDistance();
      if (this.mNeedCheckContentPosLeftCount <= 0 && Vector2.op_Equality(this.mCurFrameItemRangeData.mCheckedPosition, containerMovedDistance))
        return;
      if (this.mNeedCheckContentPosLeftCount > 0)
        --this.mNeedCheckContentPosLeftCount;
      float ax = (float) (containerMovedDistance.x - this.mItemRecycleDistance.x);
      float ay = (float) (containerMovedDistance.y - this.mItemRecycleDistance.y);
      if ((double) ax < 0.0)
        ax = 0.0f;
      if ((double) ay < 0.0)
        ay = 0.0f;
      RowColumnPair columnAtGivenAbsPos = this.GetCeilItemRowColumnAtGivenAbsPos(ax, ay);
      this.mCurFrameItemRangeData.mMinColumn = columnAtGivenAbsPos.mColumn;
      this.mCurFrameItemRangeData.mMinRow = columnAtGivenAbsPos.mRow;
      columnAtGivenAbsPos = this.GetCeilItemRowColumnAtGivenAbsPos((float) (containerMovedDistance.x + this.mItemRecycleDistance.x) + this.ViewPortWidth, (float) (containerMovedDistance.y + this.mItemRecycleDistance.y) + this.ViewPortHeight);
      this.mCurFrameItemRangeData.mMaxColumn = columnAtGivenAbsPos.mColumn;
      this.mCurFrameItemRangeData.mMaxRow = columnAtGivenAbsPos.mRow;
      this.mCurFrameItemRangeData.mCheckedPosition = containerMovedDistance;
    }

    private void UpdateRowItemGroupForRecycleAndNew(GridItemGroup group)
    {
      int mMinColumn = this.mCurFrameItemRangeData.mMinColumn;
      int mMaxColumn = this.mCurFrameItemRangeData.mMaxColumn;
      int groupIndex = group.GroupIndex;
      while (Object.op_Inequality((Object) group.First, (Object) null) && group.First.Column < mMinColumn)
        this.RecycleItemTmp(group.RemoveFirst());
      while (Object.op_Inequality((Object) group.Last, (Object) null) && group.Last.Column > mMaxColumn)
        this.RecycleItemTmp(group.RemoveLast());
      if (Object.op_Equality((Object) group.First, (Object) null))
      {
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(groupIndex, mMinColumn);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          return;
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(Vector2.op_Implicit(this.GetItemPos(newItemByRowColumn.Row, newItemByRowColumn.Column)));
        group.AddFirst(newItemByRowColumn);
      }
      while (group.First.Column > mMinColumn)
      {
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(groupIndex, group.First.Column - 1);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          return;
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(Vector2.op_Implicit(this.GetItemPos(newItemByRowColumn.Row, newItemByRowColumn.Column)));
        group.AddFirst(newItemByRowColumn);
      }
      while (group.Last.Column < mMaxColumn)
      {
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(groupIndex, group.Last.Column + 1);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          break;
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(Vector2.op_Implicit(this.GetItemPos(newItemByRowColumn.Row, newItemByRowColumn.Column)));
        group.AddLast(newItemByRowColumn);
      }
    }

    private void UpdateColumnItemGroupForRecycleAndNew(GridItemGroup group)
    {
      int mMinRow = this.mCurFrameItemRangeData.mMinRow;
      int mMaxRow = this.mCurFrameItemRangeData.mMaxRow;
      int groupIndex = group.GroupIndex;
      while (Object.op_Inequality((Object) group.First, (Object) null) && group.First.Row < mMinRow)
        this.RecycleItemTmp(group.RemoveFirst());
      while (Object.op_Inequality((Object) group.Last, (Object) null) && group.Last.Row > mMaxRow)
        this.RecycleItemTmp(group.RemoveLast());
      if (Object.op_Equality((Object) group.First, (Object) null))
      {
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(mMinRow, groupIndex);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          return;
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(Vector2.op_Implicit(this.GetItemPos(newItemByRowColumn.Row, newItemByRowColumn.Column)));
        group.AddFirst(newItemByRowColumn);
      }
      while (group.First.Row > mMinRow)
      {
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(group.First.Row - 1, groupIndex);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          return;
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(Vector2.op_Implicit(this.GetItemPos(newItemByRowColumn.Row, newItemByRowColumn.Column)));
        group.AddFirst(newItemByRowColumn);
      }
      while (group.Last.Row < mMaxRow)
      {
        LoopGridViewItem newItemByRowColumn = this.GetNewItemByRowColumn(group.Last.Row + 1, groupIndex);
        if (Object.op_Equality((Object) newItemByRowColumn, (Object) null))
          break;
        newItemByRowColumn.CachedRectTransform.set_anchoredPosition3D(Vector2.op_Implicit(this.GetItemPos(newItemByRowColumn.Row, newItemByRowColumn.Column)));
        group.AddLast(newItemByRowColumn);
      }
    }

    private void SetScrollbarListener()
    {
      if (!this.ItemSnapEnable)
        return;
      this.mScrollBarClickEventListener1 = (ClickEventListener) null;
      this.mScrollBarClickEventListener2 = (ClickEventListener) null;
      Scrollbar scrollbar1 = (Scrollbar) null;
      Scrollbar scrollbar2 = (Scrollbar) null;
      if (this.mScrollRect.get_vertical() && Object.op_Inequality((Object) this.mScrollRect.get_verticalScrollbar(), (Object) null))
        scrollbar1 = this.mScrollRect.get_verticalScrollbar();
      if (this.mScrollRect.get_horizontal() && Object.op_Inequality((Object) this.mScrollRect.get_horizontalScrollbar(), (Object) null))
        scrollbar2 = this.mScrollRect.get_horizontalScrollbar();
      if (Object.op_Inequality((Object) scrollbar1, (Object) null))
      {
        ClickEventListener clickEventListener = ClickEventListener.Get(((Component) scrollbar1).get_gameObject());
        this.mScrollBarClickEventListener1 = clickEventListener;
        clickEventListener.SetPointerUpHandler(new Action<GameObject>(this.OnPointerUpInScrollBar));
        clickEventListener.SetPointerDownHandler(new Action<GameObject>(this.OnPointerDownInScrollBar));
      }
      if (!Object.op_Inequality((Object) scrollbar2, (Object) null))
        return;
      ClickEventListener clickEventListener1 = ClickEventListener.Get(((Component) scrollbar2).get_gameObject());
      this.mScrollBarClickEventListener2 = clickEventListener1;
      clickEventListener1.SetPointerUpHandler(new Action<GameObject>(this.OnPointerUpInScrollBar));
      clickEventListener1.SetPointerDownHandler(new Action<GameObject>(this.OnPointerDownInScrollBar));
    }

    private void OnPointerDownInScrollBar(GameObject obj)
    {
      this.mCurSnapData.Clear();
    }

    private void OnPointerUpInScrollBar(GameObject obj)
    {
      this.ForceSnapUpdateCheck();
    }

    private RowColumnPair FindNearestItemWithLocalPos(float x, float y)
    {
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector(x, y);
      RowColumnPair columnAtGivenAbsPos = this.GetCeilItemRowColumnAtGivenAbsPos((float) vector2_1.x, (float) vector2_1.y);
      int mRow = columnAtGivenAbsPos.mRow;
      int mColumn = columnAtGivenAbsPos.mColumn;
      RowColumnPair rowColumnPair = new RowColumnPair(-1, -1);
      Vector2.get_zero();
      float num = float.MaxValue;
      for (int row = mRow - 1; row <= mRow + 1; ++row)
      {
        for (int column = mColumn - 1; column <= mColumn + 1; ++column)
        {
          if (row >= 0 && row < this.mRowCount && (column >= 0 && column < this.mColumnCount))
          {
            Vector2 vector2_2 = Vector2.op_Subtraction(this.GetItemSnapPivotLocalPos(row, column), vector2_1);
            float sqrMagnitude = ((Vector2) ref vector2_2).get_sqrMagnitude();
            if ((double) sqrMagnitude < (double) num)
            {
              num = sqrMagnitude;
              rowColumnPair.mRow = row;
              rowColumnPair.mColumn = column;
            }
          }
        }
      }
      return rowColumnPair;
    }

    private Vector2 GetItemSnapPivotLocalPos(int row, int column)
    {
      Vector2 itemAbsPos = this.GetItemAbsPos(row, column);
      if (this.mArrangeType == GridItemArrangeType.TopLeftToBottomRight)
        return new Vector2((float) (itemAbsPos.x + this.mItemSize.x * this.mItemSnapPivot.x), (float) (-itemAbsPos.y - this.mItemSize.y * (1.0 - this.mItemSnapPivot.y)));
      if (this.mArrangeType == GridItemArrangeType.BottomLeftToTopRight)
        return new Vector2((float) (itemAbsPos.x + this.mItemSize.x * this.mItemSnapPivot.x), (float) (itemAbsPos.y + this.mItemSize.y * this.mItemSnapPivot.y));
      if (this.mArrangeType == GridItemArrangeType.TopRightToBottomLeft)
        return new Vector2((float) (-itemAbsPos.x - this.mItemSize.x * (1.0 - this.mItemSnapPivot.x)), (float) (-itemAbsPos.y - this.mItemSize.y * (1.0 - this.mItemSnapPivot.y)));
      return this.mArrangeType == GridItemArrangeType.BottomRightToTopLeft ? new Vector2((float) (-itemAbsPos.x - this.mItemSize.x * (1.0 - this.mItemSnapPivot.x)), (float) (itemAbsPos.y + this.mItemSize.y * this.mItemSnapPivot.y)) : Vector2.get_zero();
    }

    private Vector2 GetViewPortSnapPivotLocalPos(Vector2 pos)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (this.mArrangeType == GridItemArrangeType.TopLeftToBottomRight)
      {
        num1 = (float) (-pos.x + (double) this.ViewPortWidth * this.mViewPortSnapPivot.x);
        num2 = (float) (-pos.y - (double) this.ViewPortHeight * (1.0 - this.mViewPortSnapPivot.y));
      }
      else if (this.mArrangeType == GridItemArrangeType.BottomLeftToTopRight)
      {
        num1 = (float) (-pos.x + (double) this.ViewPortWidth * this.mViewPortSnapPivot.x);
        num2 = (float) (-pos.y + (double) this.ViewPortHeight * this.mViewPortSnapPivot.y);
      }
      else if (this.mArrangeType == GridItemArrangeType.TopRightToBottomLeft)
      {
        num1 = (float) (-pos.x - (double) this.ViewPortWidth * (1.0 - this.mViewPortSnapPivot.x));
        num2 = (float) (-pos.y - (double) this.ViewPortHeight * (1.0 - this.mViewPortSnapPivot.y));
      }
      else if (this.mArrangeType == GridItemArrangeType.BottomRightToTopLeft)
      {
        num1 = (float) (-pos.x - (double) this.ViewPortWidth * (1.0 - this.mViewPortSnapPivot.x));
        num2 = (float) (-pos.y + (double) this.ViewPortHeight * this.mViewPortSnapPivot.y);
      }
      return new Vector2(num1, num2);
    }

    private void UpdateNearestSnapItem(bool forceSendEvent)
    {
      if (!this.mItemSnapEnable || this.mItemGroupList.Count == 0 || !this.IsContainerTransCanMove())
        return;
      Vector2 containerVaildPos = this.GetContainerVaildPos((float) this.ContainerTrans.get_anchoredPosition3D().x, (float) this.ContainerTrans.get_anchoredPosition3D().y);
      bool flag = containerVaildPos.y != this.mLastSnapCheckPos.y || containerVaildPos.x != this.mLastSnapCheckPos.x;
      this.mLastSnapCheckPos = Vector2.op_Implicit(containerVaildPos);
      if (!flag && this.mLeftSnapUpdateExtraCount > 0)
      {
        --this.mLeftSnapUpdateExtraCount;
        flag = true;
      }
      if (!flag)
        return;
      RowColumnPair rowColumnPair = new RowColumnPair(-1, -1);
      Vector2 snapPivotLocalPos = this.GetViewPortSnapPivotLocalPos(containerVaildPos);
      RowColumnPair itemWithLocalPos = this.FindNearestItemWithLocalPos((float) snapPivotLocalPos.x, (float) snapPivotLocalPos.y);
      if (itemWithLocalPos.mRow >= 0)
      {
        RowColumnPair nearestItemRowColumn = this.mCurSnapNearestItemRowColumn;
        this.mCurSnapNearestItemRowColumn = itemWithLocalPos;
        if (!forceSendEvent && !(nearestItemRowColumn != this.mCurSnapNearestItemRowColumn) || this.mOnSnapNearestChanged == null)
          return;
        this.mOnSnapNearestChanged(this);
      }
      else
      {
        this.mCurSnapNearestItemRowColumn.mRow = -1;
        this.mCurSnapNearestItemRowColumn.mColumn = -1;
      }
    }

    private void UpdateFromSettingParam(LoopGridViewSettingParam param)
    {
      if (param == null)
        return;
      if (param.mItemSize != null)
        this.mItemSize = (Vector2) param.mItemSize;
      if (param.mItemPadding != null)
        this.mItemPadding = (Vector2) param.mItemPadding;
      if (param.mPadding != null)
        this.mPadding = (RectOffset) param.mPadding;
      if (param.mGridFixedType != null)
        this.mGridFixedType = (GridFixedType) param.mGridFixedType;
      if (param.mFixedRowOrColumnCount == null)
        return;
      this.mFixedRowOrColumnCount = (int) param.mFixedRowOrColumnCount;
    }

    public void FinishSnapImmediately()
    {
      this.UpdateSnapMove(true, false);
    }

    private void UpdateSnapMove(bool immediate = false, bool forceSendEvent = false)
    {
      if (!this.mItemSnapEnable)
        return;
      this.UpdateNearestSnapItem(false);
      Vector2 vector2_1 = Vector2.op_Implicit(this.mContainerTrans.get_anchoredPosition3D());
      if (!this.CanSnap())
      {
        this.ClearSnapData();
      }
      else
      {
        this.UpdateCurSnapData();
        if (this.mCurSnapData.mSnapStatus != SnapStatus.SnapMoving)
          return;
        if ((double) Mathf.Abs((float) this.mScrollRect.get_velocity().x) + (double) Mathf.Abs((float) this.mScrollRect.get_velocity().y) > 0.0)
          this.mScrollRect.StopMovement();
        float mCurSnapVal = this.mCurSnapData.mCurSnapVal;
        this.mCurSnapData.mCurSnapVal = Mathf.SmoothDamp(this.mCurSnapData.mCurSnapVal, this.mCurSnapData.mTargetSnapVal, ref this.mSmoothDumpVel, this.mSmoothDumpRate);
        float num = this.mCurSnapData.mCurSnapVal - mCurSnapVal;
        Vector2 vector2_2;
        if (immediate || (double) Mathf.Abs(this.mCurSnapData.mTargetSnapVal - this.mCurSnapData.mCurSnapVal) < (double) this.mSnapFinishThreshold)
        {
          vector2_2 = Vector2.op_Addition(vector2_1, Vector2.op_Multiply(this.mCurSnapData.mTargetSnapVal - mCurSnapVal, this.mCurSnapData.mSnapNeedMoveDir));
          this.mCurSnapData.mSnapStatus = SnapStatus.SnapMoveFinish;
          if (this.mOnSnapItemFinished != null)
          {
            LoopGridViewItem shownItemByRowColumn = this.GetShownItemByRowColumn(this.mCurSnapNearestItemRowColumn.mRow, this.mCurSnapNearestItemRowColumn.mColumn);
            if (Object.op_Inequality((Object) shownItemByRowColumn, (Object) null))
              this.mOnSnapItemFinished(this, shownItemByRowColumn);
          }
        }
        else
          vector2_2 = Vector2.op_Addition(vector2_1, Vector2.op_Multiply(num, this.mCurSnapData.mSnapNeedMoveDir));
        this.mContainerTrans.set_anchoredPosition3D(Vector2.op_Implicit(this.GetContainerVaildPos((float) vector2_2.x, (float) vector2_2.y)));
      }
    }

    private GridItemGroup GetShownGroup(int groupIndex)
    {
      if (groupIndex < 0)
        return (GridItemGroup) null;
      int count = this.mItemGroupList.Count;
      if (count == 0)
        return (GridItemGroup) null;
      return groupIndex < this.mItemGroupList[0].GroupIndex || groupIndex > this.mItemGroupList[count - 1].GroupIndex ? (GridItemGroup) null : this.mItemGroupList[groupIndex - this.mItemGroupList[0].GroupIndex];
    }

    private void FillCurSnapData(int row, int column)
    {
      Vector2 vector2 = Vector2.op_Subtraction(this.GetViewPortSnapPivotLocalPos(this.GetContainerVaildPos((float) this.ContainerTrans.get_anchoredPosition3D().x, (float) this.ContainerTrans.get_anchoredPosition3D().y)), this.GetItemSnapPivotLocalPos(row, column));
      if (!this.mScrollRect.get_horizontal())
        vector2.x = (__Null) 0.0;
      if (!this.mScrollRect.get_vertical())
        vector2.y = (__Null) 0.0;
      this.mCurSnapData.mTargetSnapVal = ((Vector2) ref vector2).get_magnitude();
      this.mCurSnapData.mCurSnapVal = 0.0f;
      this.mCurSnapData.mSnapNeedMoveDir = ((Vector2) ref vector2).get_normalized();
    }

    private void UpdateCurSnapData()
    {
      if (this.mItemGroupList.Count == 0)
      {
        this.mCurSnapData.Clear();
      }
      else
      {
        if (this.mCurSnapData.mSnapStatus == SnapStatus.SnapMoveFinish)
        {
          if (this.mCurSnapData.mSnapTarget == this.mCurSnapNearestItemRowColumn)
            return;
          this.mCurSnapData.mSnapStatus = SnapStatus.NoTargetSet;
        }
        if (this.mCurSnapData.mSnapStatus == SnapStatus.SnapMoving)
        {
          if (this.mCurSnapData.mSnapTarget == this.mCurSnapNearestItemRowColumn || this.mCurSnapData.mIsForceSnapTo)
            return;
          this.mCurSnapData.mSnapStatus = SnapStatus.NoTargetSet;
        }
        if (this.mCurSnapData.mSnapStatus == SnapStatus.NoTargetSet)
        {
          if (Object.op_Equality((Object) this.GetShownItemByRowColumn(this.mCurSnapNearestItemRowColumn.mRow, this.mCurSnapNearestItemRowColumn.mColumn), (Object) null))
            return;
          this.mCurSnapData.mSnapTarget = this.mCurSnapNearestItemRowColumn;
          this.mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
          this.mCurSnapData.mIsForceSnapTo = false;
        }
        if (this.mCurSnapData.mSnapStatus != SnapStatus.TargetHasSet)
          return;
        LoopGridViewItem shownItemByRowColumn = this.GetShownItemByRowColumn(this.mCurSnapData.mSnapTarget.mRow, this.mCurSnapData.mSnapTarget.mColumn);
        if (Object.op_Equality((Object) shownItemByRowColumn, (Object) null))
        {
          this.mCurSnapData.Clear();
        }
        else
        {
          this.FillCurSnapData(shownItemByRowColumn.Row, shownItemByRowColumn.Column);
          this.mCurSnapData.mSnapStatus = SnapStatus.SnapMoving;
        }
      }
    }

    private bool CanSnap()
    {
      if (this.mIsDraging || Object.op_Inequality((Object) this.mScrollBarClickEventListener1, (Object) null) && this.mScrollBarClickEventListener1.IsPressd || (Object.op_Inequality((Object) this.mScrollBarClickEventListener2, (Object) null) && this.mScrollBarClickEventListener2.IsPressd || !this.IsContainerTransCanMove()) || (double) Mathf.Abs((float) this.mScrollRect.get_velocity().x) > (double) this.mSnapVecThreshold || (double) Mathf.Abs((float) this.mScrollRect.get_velocity().y) > (double) this.mSnapVecThreshold)
        return false;
      Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
      Vector2 containerVaildPos = this.GetContainerVaildPos((float) anchoredPosition3D.x, (float) anchoredPosition3D.y);
      return (double) Mathf.Abs((float) (anchoredPosition3D.x - containerVaildPos.x)) <= 3.0 && (double) Mathf.Abs((float) (anchoredPosition3D.y - containerVaildPos.y)) <= 3.0;
    }

    private GridItemGroup GetOneItemGroupObj()
    {
      int count = this.mItemGroupObjPool.Count;
      if (count == 0)
        return new GridItemGroup();
      GridItemGroup gridItemGroup = this.mItemGroupObjPool[count - 1];
      this.mItemGroupObjPool.RemoveAt(count - 1);
      return gridItemGroup;
    }

    private void RecycleOneItemGroupObj(GridItemGroup obj)
    {
      this.mItemGroupObjPool.Add(obj);
    }

    private class SnapData
    {
      public SnapStatus mSnapStatus;
      public RowColumnPair mSnapTarget;
      public Vector2 mSnapNeedMoveDir;
      public float mTargetSnapVal;
      public float mCurSnapVal;
      public bool mIsForceSnapTo;

      public void Clear()
      {
        this.mSnapStatus = SnapStatus.NoTargetSet;
        this.mIsForceSnapTo = false;
      }
    }

    private class ItemRangeData
    {
      public int mMaxRow;
      public int mMinRow;
      public int mMaxColumn;
      public int mMinColumn;
      public Vector2 mCheckedPosition;
    }
  }
}
