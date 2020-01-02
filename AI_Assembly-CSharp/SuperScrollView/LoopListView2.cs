// Decompiled with JetBrains decompiler
// Type: SuperScrollView.LoopListView2
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
  public class LoopListView2 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
  {
    private Dictionary<string, ItemPool> mItemPoolDict;
    private List<ItemPool> mItemPoolList;
    [SerializeField]
    private List<ItemPrefabConfData> mItemPrefabDataList;
    [SerializeField]
    private ListItemArrangeType mArrangeType;
    private List<LoopListViewItem2> mItemList;
    private RectTransform mContainerTrans;
    private ScrollRect mScrollRect;
    private RectTransform mScrollRectTransform;
    private RectTransform mViewPortRectTransform;
    private float mItemDefaultWithPaddingSize;
    private int mItemTotalCount;
    private bool mIsVertList;
    private Func<LoopListView2, int, LoopListViewItem2> mOnGetItemByIndex;
    private Vector3[] mItemWorldCorners;
    private Vector3[] mViewPortRectLocalCorners;
    private int mCurReadyMinItemIndex;
    private int mCurReadyMaxItemIndex;
    private bool mNeedCheckNextMinItem;
    private bool mNeedCheckNextMaxItem;
    private ItemPosMgr mItemPosMgr;
    private float mDistanceForRecycle0;
    private float mDistanceForNew0;
    private float mDistanceForRecycle1;
    private float mDistanceForNew1;
    [SerializeField]
    private bool mSupportScrollBar;
    private bool mIsDraging;
    private PointerEventData mPointerEventData;
    public Action mOnBeginDragAction;
    public Action mOnDragingAction;
    public Action mOnEndDragAction;
    private int mLastItemIndex;
    private float mLastItemPadding;
    private float mSmoothDumpVel;
    private float mSmoothDumpRate;
    private float mSnapFinishThreshold;
    private float mSnapVecThreshold;
    [SerializeField]
    private bool mItemSnapEnable;
    private Vector3 mLastFrameContainerPos;
    public Action<LoopListView2, LoopListViewItem2> mOnSnapItemFinished;
    public Action<LoopListView2, LoopListViewItem2> mOnSnapNearestChanged;
    private int mCurSnapNearestItemIndex;
    private Vector2 mAdjustedVec;
    private bool mNeedAdjustVec;
    private int mLeftSnapUpdateExtraCount;
    [SerializeField]
    private Vector2 mViewPortSnapPivot;
    [SerializeField]
    private Vector2 mItemSnapPivot;
    private ClickEventListener mScrollBarClickEventListener;
    private LoopListView2.SnapData mCurSnapData;
    private Vector3 mLastSnapCheckPos;
    private bool mListViewInited;
    private int mListUpdateCheckFrameCount;

    public LoopListView2()
    {
      base.\u002Ector();
    }

    public ListItemArrangeType ArrangeType
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

    public bool IsInit
    {
      get
      {
        return this.mListViewInited;
      }
    }

    public List<ItemPrefabConfData> ItemPrefabDataList
    {
      get
      {
        return this.mItemPrefabDataList;
      }
    }

    public List<LoopListViewItem2> ItemList
    {
      get
      {
        return this.mItemList;
      }
    }

    public bool IsVertList
    {
      get
      {
        return this.mIsVertList;
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

    public bool SupportScrollBar
    {
      get
      {
        return this.mSupportScrollBar;
      }
      set
      {
        this.mSupportScrollBar = value;
      }
    }

    public ItemPrefabConfData GetItemPrefabConfData(string prefabName)
    {
      foreach (ItemPrefabConfData mItemPrefabData in this.mItemPrefabDataList)
      {
        if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab, (Object) null))
          Debug.LogError((object) "A item prefab is null ");
        else if (prefabName == ((Object) mItemPrefabData.mItemPrefab).get_name())
          return mItemPrefabData;
      }
      return (ItemPrefabConfData) null;
    }

    public void OnItemPrefabChanged(string prefabName)
    {
      ItemPrefabConfData itemPrefabConfData = this.GetItemPrefabConfData(prefabName);
      if (itemPrefabConfData == null)
        return;
      ItemPool itemPool = (ItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(prefabName, out itemPool))
        return;
      int firstItemIndex = -1;
      Vector3 pos = Vector3.get_zero();
      if (this.mItemList.Count > 0)
      {
        firstItemIndex = this.mItemList[0].ItemIndex;
        pos = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
      }
      this.RecycleAllItem();
      this.ClearAllTmpRecycledItem();
      itemPool.DestroyAllItem();
      itemPool.Init(itemPrefabConfData.mItemPrefab, itemPrefabConfData.mPadding, itemPrefabConfData.mStartPosOffset, itemPrefabConfData.mInitCreateCount, this.mContainerTrans);
      if (firstItemIndex < 0)
        return;
      this.RefreshAllShownItemWithFirstIndexAndPos(firstItemIndex, pos);
    }

    public void InitListView(
      int itemTotalCount,
      Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex,
      LoopListViewInitParam initParam = null)
    {
      if (initParam != null)
      {
        this.mDistanceForRecycle0 = initParam.mDistanceForRecycle0;
        this.mDistanceForNew0 = initParam.mDistanceForNew0;
        this.mDistanceForRecycle1 = initParam.mDistanceForRecycle1;
        this.mDistanceForNew1 = initParam.mDistanceForNew1;
        this.mSmoothDumpRate = initParam.mSmoothDumpRate;
        this.mSnapFinishThreshold = initParam.mSnapFinishThreshold;
        this.mSnapVecThreshold = initParam.mSnapVecThreshold;
        this.mItemDefaultWithPaddingSize = initParam.mItemDefaultWithPaddingSize;
      }
      this.mScrollRect = (ScrollRect) ((Component) this).get_gameObject().GetComponent<ScrollRect>();
      if (Object.op_Equality((Object) this.mScrollRect, (Object) null))
      {
        Debug.LogError((object) "ListView Init Failed! ScrollRect component not found!");
      }
      else
      {
        if ((double) this.mDistanceForRecycle0 <= (double) this.mDistanceForNew0)
          Debug.LogError((object) "mDistanceForRecycle0 should be bigger than mDistanceForNew0");
        if ((double) this.mDistanceForRecycle1 <= (double) this.mDistanceForNew1)
          Debug.LogError((object) "mDistanceForRecycle1 should be bigger than mDistanceForNew1");
        this.mCurSnapData.Clear();
        this.mItemPosMgr = new ItemPosMgr(this.mItemDefaultWithPaddingSize);
        this.mScrollRectTransform = (RectTransform) ((Component) this.mScrollRect).GetComponent<RectTransform>();
        this.mContainerTrans = this.mScrollRect.get_content();
        this.mViewPortRectTransform = this.mScrollRect.get_viewport();
        if (Object.op_Equality((Object) this.mViewPortRectTransform, (Object) null))
          this.mViewPortRectTransform = this.mScrollRectTransform;
        if (this.mScrollRect.get_horizontalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_horizontalScrollbar(), (Object) null))
          Debug.LogError((object) "ScrollRect.horizontalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
        if (this.mScrollRect.get_verticalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_verticalScrollbar(), (Object) null))
          Debug.LogError((object) "ScrollRect.verticalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
        this.mIsVertList = this.mArrangeType == ListItemArrangeType.TopToBottom || this.mArrangeType == ListItemArrangeType.BottomToTop;
        this.mScrollRect.set_horizontal(!this.mIsVertList);
        this.mScrollRect.set_vertical(this.mIsVertList);
        this.SetScrollbarListener();
        this.AdjustPivot(this.mViewPortRectTransform);
        this.AdjustAnchor(this.mContainerTrans);
        this.AdjustContainerPivot(this.mContainerTrans);
        this.InitItemPool();
        this.mOnGetItemByIndex = onGetItemByIndex;
        if (this.mListViewInited)
          Debug.LogError((object) "LoopListView2.InitListView method can be called only once.");
        this.mListViewInited = true;
        this.ResetListView(true);
        this.mCurSnapData.Clear();
        this.mItemTotalCount = itemTotalCount;
        if (this.mItemTotalCount < 0)
          this.mSupportScrollBar = false;
        if (this.mSupportScrollBar)
          this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
        else
          this.mItemPosMgr.SetItemMaxCount(0);
        this.mCurReadyMaxItemIndex = 0;
        this.mCurReadyMinItemIndex = 0;
        this.mLeftSnapUpdateExtraCount = 1;
        this.mNeedCheckNextMaxItem = true;
        this.mNeedCheckNextMinItem = true;
        this.UpdateContentSize();
      }
    }

    public void InitListViewAndSize(
      int itemTotalCount,
      Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex)
    {
      this.mScrollRect = (ScrollRect) ((Component) this).get_gameObject().GetComponent<ScrollRect>();
      if (Object.op_Equality((Object) this.mScrollRect, (Object) null))
      {
        Debug.LogError((object) "ListView Init Failed! ScrollRect component not found!");
      }
      else
      {
        if ((double) this.mDistanceForRecycle0 <= (double) this.mDistanceForNew0)
          Debug.LogError((object) "mDistanceForRecycle0 should be bigger than mDistanceForNew0");
        if ((double) this.mDistanceForRecycle1 <= (double) this.mDistanceForNew1)
          Debug.LogError((object) "mDistanceForRecycle1 should be bigger than mDistanceForNew1");
        this.mCurSnapData.Clear();
        this.mItemPosMgr = new ItemPosMgr(this.mItemDefaultWithPaddingSize);
        this.mScrollRectTransform = (RectTransform) ((Component) this.mScrollRect).GetComponent<RectTransform>();
        this.mContainerTrans = this.mScrollRect.get_content();
        this.mViewPortRectTransform = this.mScrollRect.get_viewport();
        if (Object.op_Equality((Object) this.mViewPortRectTransform, (Object) null))
          this.mViewPortRectTransform = this.mScrollRectTransform;
        if (this.mScrollRect.get_horizontalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_horizontalScrollbar(), (Object) null))
          Debug.LogError((object) "ScrollRect.horizontalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
        if (this.mScrollRect.get_verticalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_verticalScrollbar(), (Object) null))
          Debug.LogError((object) "ScrollRect.verticalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
        this.mIsVertList = this.mArrangeType == ListItemArrangeType.TopToBottom || this.mArrangeType == ListItemArrangeType.BottomToTop;
        this.mScrollRect.set_horizontal(!this.mIsVertList);
        this.mScrollRect.set_vertical(this.mIsVertList);
        this.SetScrollbarListener();
        this.AdjustPivot(this.mViewPortRectTransform);
        this.AdjustAnchor(this.mContainerTrans);
        this.AdjustContainerPivot(this.mContainerTrans);
        this.InitItemPool();
        this.mOnGetItemByIndex = onGetItemByIndex;
        if (this.mListViewInited)
          Debug.LogError((object) "LoopListView2.InitListView method can be called only once.");
        this.mListViewInited = true;
        this.ResetListView(true);
        this.mCurSnapData.Clear();
        this.mItemTotalCount = itemTotalCount;
        if (this.mItemTotalCount < 0)
          this.mSupportScrollBar = false;
        if (this.mSupportScrollBar)
          this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
        else
          this.mItemPosMgr.SetItemMaxCount(0);
        this.mCurReadyMaxItemIndex = 0;
        this.mCurReadyMinItemIndex = 0;
        this.mLeftSnapUpdateExtraCount = 1;
        this.mNeedCheckNextMaxItem = true;
        this.mNeedCheckNextMinItem = true;
        this.MovePanelToItemIndex(0, 0.0f);
        this.SetSize(this.mItemTotalCount);
        this.UpdateContentSize();
      }
    }

    private void SetScrollbarListener()
    {
      this.mScrollBarClickEventListener = (ClickEventListener) null;
      Scrollbar scrollbar = (Scrollbar) null;
      if (this.mIsVertList && Object.op_Inequality((Object) this.mScrollRect.get_verticalScrollbar(), (Object) null))
        scrollbar = this.mScrollRect.get_verticalScrollbar();
      if (!this.mIsVertList && Object.op_Inequality((Object) this.mScrollRect.get_horizontalScrollbar(), (Object) null))
        scrollbar = this.mScrollRect.get_horizontalScrollbar();
      if (Object.op_Equality((Object) scrollbar, (Object) null))
        return;
      ClickEventListener clickEventListener = ClickEventListener.Get(((Component) scrollbar).get_gameObject());
      this.mScrollBarClickEventListener = clickEventListener;
      clickEventListener.SetPointerUpHandler(new Action<GameObject>(this.OnPointerUpInScrollBar));
      clickEventListener.SetPointerDownHandler(new Action<GameObject>(this.OnPointerDownInScrollBar));
    }

    private void OnPointerDownInScrollBar(GameObject obj)
    {
      this.mCurSnapData.Clear();
    }

    private void OnPointerUpInScrollBar(GameObject obj)
    {
      this.ForceSnapUpdateCheck();
    }

    public void ResetListView(bool resetPos = true)
    {
      this.mViewPortRectTransform.GetLocalCorners(this.mViewPortRectLocalCorners);
      if (resetPos)
        this.mContainerTrans.set_anchoredPosition3D(Vector3.get_zero());
      this.ForceSnapUpdateCheck();
    }

    public bool SetListItemCount(int itemCount, bool resetPos = true)
    {
      if (itemCount == this.mItemTotalCount)
        return false;
      this.mCurSnapData.Clear();
      this.mItemTotalCount = itemCount;
      if (this.mItemTotalCount < 0)
        this.mSupportScrollBar = false;
      if (this.mSupportScrollBar)
        this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
      else
        this.mItemPosMgr.SetItemMaxCount(0);
      if (this.mItemTotalCount == 0)
      {
        this.mCurReadyMaxItemIndex = 0;
        this.mCurReadyMinItemIndex = 0;
        this.mNeedCheckNextMaxItem = false;
        this.mNeedCheckNextMinItem = false;
        this.RecycleAllItem();
        this.ClearAllTmpRecycledItem();
        this.UpdateContentSize();
        return false;
      }
      if (this.mCurReadyMaxItemIndex >= this.mItemTotalCount)
        this.mCurReadyMaxItemIndex = this.mItemTotalCount - 1;
      this.mLeftSnapUpdateExtraCount = 1;
      this.mNeedCheckNextMaxItem = true;
      this.mNeedCheckNextMinItem = true;
      if (resetPos)
      {
        this.MovePanelToItemIndex(0, 0.0f);
        return false;
      }
      if (this.mItemList.Count == 0)
      {
        this.MovePanelToItemIndex(0, 0.0f);
        return false;
      }
      int itemIndex = this.mItemTotalCount - 1;
      if (this.mItemList[this.mItemList.Count - 1].ItemIndex <= itemIndex)
      {
        this.UpdateContentSize();
        this.UpdateAllShownItemsPos();
        return false;
      }
      this.MovePanelToItemIndex(itemIndex, 0.0f);
      return true;
    }

    public void ReSetListItemCount(int itemCount)
    {
      this.mCurSnapData.Clear();
      this.mItemTotalCount = itemCount;
      if (this.mItemTotalCount < 0)
        this.mSupportScrollBar = false;
      if (this.mSupportScrollBar)
        this.mItemPosMgr.SetItemMaxCount(this.mItemTotalCount);
      else
        this.mItemPosMgr.SetItemMaxCount(0);
      if (this.mItemTotalCount == 0)
      {
        this.mCurReadyMaxItemIndex = 0;
        this.mCurReadyMinItemIndex = 0;
        this.mNeedCheckNextMaxItem = false;
        this.mNeedCheckNextMinItem = false;
        this.RecycleAllItem();
        this.ClearAllTmpRecycledItem();
        this.UpdateContentSize();
      }
      else
      {
        if (this.mCurReadyMaxItemIndex >= this.mItemTotalCount)
          this.mCurReadyMaxItemIndex = this.mItemTotalCount - 1;
        this.mLeftSnapUpdateExtraCount = 1;
        this.mNeedCheckNextMaxItem = true;
        this.mNeedCheckNextMinItem = true;
        this.MovePanelToItemIndex(0, 0.0f);
        this.SetSize(this.mItemTotalCount);
        this.UpdateContentSize();
      }
    }

    private void SetSize(int loopCount)
    {
      LoopListViewItem2 shownItemByItemIndex = this.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null) || !this.mSupportScrollBar)
        return;
      for (int index = 0; index < loopCount; ++index)
      {
        if (this.mIsVertList)
        {
          int itemIndex = index;
          Rect rect = shownItemByItemIndex.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double padding = (double) shownItemByItemIndex.Padding;
          this.SetItemSize(itemIndex, (float) height, (float) padding);
        }
        else
        {
          int itemIndex = index;
          Rect rect = shownItemByItemIndex.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double padding = (double) shownItemByItemIndex.Padding;
          this.SetItemSize(itemIndex, (float) width, (float) padding);
        }
      }
    }

    public LoopListViewItem2 GetShownItemByItemIndex(int itemIndex)
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return (LoopListViewItem2) null;
      return itemIndex < this.mItemList[0].ItemIndex || itemIndex > this.mItemList[count - 1].ItemIndex ? (LoopListViewItem2) null : this.mItemList[itemIndex - this.mItemList[0].ItemIndex];
    }

    public int ShownItemCount
    {
      get
      {
        return this.mItemList.Count;
      }
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

    public LoopListViewItem2 GetShownItemByIndex(int index)
    {
      int count = this.mItemList.Count;
      return index < 0 || index >= count ? (LoopListViewItem2) null : this.mItemList[index];
    }

    public LoopListViewItem2 GetShownItemByIndexWithoutCheck(int index)
    {
      return this.mItemList[index];
    }

    public int GetIndexInShownItemList(LoopListViewItem2 item)
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

    public void DoActionForEachShownItem(Action<LoopListViewItem2, object> action, object param)
    {
      if (action == null)
        return;
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      for (int index = 0; index < count; ++index)
        action(this.mItemList[index], param);
    }

    public LoopListViewItem2 NewListViewItem(string itemPrefabName)
    {
      ItemPool itemPool = (ItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(itemPrefabName, out itemPool))
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = itemPool.GetItem();
      RectTransform component = (RectTransform) ((Component) loopListViewItem2).GetComponent<RectTransform>();
      ((Transform) component).SetParent((Transform) this.mContainerTrans);
      ((Transform) component).set_localScale(Vector3.get_one());
      component.set_anchoredPosition3D(Vector3.get_zero());
      ((Transform) component).set_localEulerAngles(Vector3.get_zero());
      loopListViewItem2.ParentListView = this;
      return loopListViewItem2;
    }

    public void OnItemSizeChanged(int itemIndex)
    {
      LoopListViewItem2 shownItemByItemIndex = this.GetShownItemByItemIndex(itemIndex);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      if (this.mSupportScrollBar)
      {
        if (this.mIsVertList)
        {
          int itemIndex1 = itemIndex;
          Rect rect = shownItemByItemIndex.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double padding = (double) shownItemByItemIndex.Padding;
          this.SetItemSize(itemIndex1, (float) height, (float) padding);
        }
        else
        {
          int itemIndex1 = itemIndex;
          Rect rect = shownItemByItemIndex.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double padding = (double) shownItemByItemIndex.Padding;
          this.SetItemSize(itemIndex1, (float) width, (float) padding);
        }
      }
      this.UpdateContentSize();
      this.UpdateAllShownItemsPos();
    }

    public void RefreshItemByItemIndex(int itemIndex)
    {
      int count = this.mItemList.Count;
      if (count == 0 || itemIndex < this.mItemList[0].ItemIndex || itemIndex > this.mItemList[count - 1].ItemIndex)
        return;
      int itemIndex1 = this.mItemList[0].ItemIndex;
      int index = itemIndex - itemIndex1;
      LoopListViewItem2 mItem = this.mItemList[index];
      Vector3 anchoredPosition3D = mItem.CachedRectTransform.get_anchoredPosition3D();
      this.RecycleItemTmp(mItem);
      LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(itemIndex);
      if (Object.op_Equality((Object) newItemByIndex, (Object) null))
      {
        this.RefreshAllShownItemWithFirstIndex(itemIndex1);
      }
      else
      {
        this.mItemList[index] = newItemByIndex;
        if (this.mIsVertList)
          anchoredPosition3D.x = (__Null) (double) newItemByIndex.StartPosOffset;
        else
          anchoredPosition3D.y = (__Null) (double) newItemByIndex.StartPosOffset;
        newItemByIndex.CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
        this.OnItemSizeChanged(itemIndex);
        this.ClearAllTmpRecycledItem();
      }
    }

    public void FinishSnapImmediately()
    {
      this.UpdateSnapMove(true, false);
    }

    public void MovePanelToItemIndex(int itemIndex, float offset)
    {
      this.mScrollRect.StopMovement();
      this.mCurSnapData.Clear();
      if (this.mItemTotalCount == 0 || itemIndex < 0 && this.mItemTotalCount > 0)
        return;
      if (this.mItemTotalCount > 0 && itemIndex >= this.mItemTotalCount)
        itemIndex = this.mItemTotalCount - 1;
      if ((double) offset < 0.0)
        offset = 0.0f;
      Vector3 zero = Vector3.get_zero();
      float viewPortSize = this.ViewPortSize;
      if ((double) offset > (double) viewPortSize)
        offset = viewPortSize;
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        float num = (float) this.mContainerTrans.get_anchoredPosition3D().y;
        if ((double) num < 0.0)
          num = 0.0f;
        zero.y = (__Null) (-(double) num - (double) offset);
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        float num = (float) this.mContainerTrans.get_anchoredPosition3D().y;
        if ((double) num > 0.0)
          num = 0.0f;
        zero.y = (__Null) (-(double) num + (double) offset);
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        float num = (float) this.mContainerTrans.get_anchoredPosition3D().x;
        if ((double) num > 0.0)
          num = 0.0f;
        zero.x = (__Null) (-(double) num + (double) offset);
      }
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
      {
        float num = (float) this.mContainerTrans.get_anchoredPosition3D().x;
        if ((double) num < 0.0)
          num = 0.0f;
        zero.x = (__Null) (-(double) num - (double) offset);
      }
      this.RecycleAllItem();
      LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(itemIndex);
      if (Object.op_Equality((Object) newItemByIndex, (Object) null))
      {
        this.ClearAllTmpRecycledItem();
      }
      else
      {
        if (this.mIsVertList)
          zero.x = (__Null) (double) newItemByIndex.StartPosOffset;
        else
          zero.y = (__Null) (double) newItemByIndex.StartPosOffset;
        newItemByIndex.CachedRectTransform.set_anchoredPosition3D(zero);
        if (this.mSupportScrollBar)
        {
          if (this.mIsVertList)
          {
            int itemIndex1 = itemIndex;
            Rect rect = newItemByIndex.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) newItemByIndex.Padding;
            this.SetItemSize(itemIndex1, (float) height, (float) padding);
          }
          else
          {
            int itemIndex1 = itemIndex;
            Rect rect = newItemByIndex.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) newItemByIndex.Padding;
            this.SetItemSize(itemIndex1, (float) width, (float) padding);
          }
        }
        this.mItemList.Add(newItemByIndex);
        this.UpdateContentSize();
        this.UpdateListView(viewPortSize + 100f, viewPortSize + 100f, viewPortSize, viewPortSize);
        this.AdjustPanelPos();
        this.ClearAllTmpRecycledItem();
        this.ForceSnapUpdateCheck();
        this.UpdateSnapMove(false, true);
      }
    }

    public void RefreshAllShownItem()
    {
      if (this.mItemList.Count == 0)
        return;
      this.RefreshAllShownItemWithFirstIndex(this.mItemList[0].ItemIndex);
    }

    public void RefreshAllShownItemWithFirstIndex(int firstItemIndex)
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      Vector3 anchoredPosition3D = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
      this.RecycleAllItem();
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = firstItemIndex + index1;
        LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index2);
        if (!Object.op_Equality((Object) newItemByIndex, (Object) null))
        {
          if (this.mIsVertList)
            anchoredPosition3D.x = (__Null) (double) newItemByIndex.StartPosOffset;
          else
            anchoredPosition3D.y = (__Null) (double) newItemByIndex.StartPosOffset;
          newItemByIndex.CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
          if (this.mSupportScrollBar)
          {
            if (this.mIsVertList)
            {
              int itemIndex = index2;
              Rect rect = newItemByIndex.CachedRectTransform.get_rect();
              double height = (double) ((Rect) ref rect).get_height();
              double padding = (double) newItemByIndex.Padding;
              this.SetItemSize(itemIndex, (float) height, (float) padding);
            }
            else
            {
              int itemIndex = index2;
              Rect rect = newItemByIndex.CachedRectTransform.get_rect();
              double width = (double) ((Rect) ref rect).get_width();
              double padding = (double) newItemByIndex.Padding;
              this.SetItemSize(itemIndex, (float) width, (float) padding);
            }
          }
          this.mItemList.Add(newItemByIndex);
        }
        else
          break;
      }
      this.UpdateContentSize();
      this.UpdateAllShownItemsPos();
      this.ClearAllTmpRecycledItem();
    }

    public void RefreshAllShownItemWithFirstIndexAndPos(int firstItemIndex, Vector3 pos)
    {
      this.RecycleAllItem();
      LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(firstItemIndex);
      if (Object.op_Equality((Object) newItemByIndex, (Object) null))
        return;
      if (this.mIsVertList)
        pos.x = (__Null) (double) newItemByIndex.StartPosOffset;
      else
        pos.y = (__Null) (double) newItemByIndex.StartPosOffset;
      newItemByIndex.CachedRectTransform.set_anchoredPosition3D(pos);
      if (this.mSupportScrollBar)
      {
        if (this.mIsVertList)
        {
          int itemIndex = firstItemIndex;
          Rect rect = newItemByIndex.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double padding = (double) newItemByIndex.Padding;
          this.SetItemSize(itemIndex, (float) height, (float) padding);
        }
        else
        {
          int itemIndex = firstItemIndex;
          Rect rect = newItemByIndex.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double padding = (double) newItemByIndex.Padding;
          this.SetItemSize(itemIndex, (float) width, (float) padding);
        }
      }
      this.mItemList.Add(newItemByIndex);
      this.UpdateContentSize();
      this.UpdateAllShownItemsPos();
      this.UpdateListView(this.mDistanceForRecycle0, this.mDistanceForRecycle1, this.mDistanceForNew0, this.mDistanceForNew1);
      this.ClearAllTmpRecycledItem();
    }

    private void RecycleItemTmp(LoopListViewItem2 item)
    {
      if (Object.op_Equality((Object) item, (Object) null) || string.IsNullOrEmpty(item.ItemPrefabName))
        return;
      ItemPool itemPool = (ItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(item.ItemPrefabName, out itemPool))
        return;
      itemPool.RecycleItem(item);
    }

    private void ClearAllTmpRecycledItem()
    {
      int count = this.mItemPoolList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemPoolList[index].ClearTmpRecycledItem();
    }

    private void RecycleAllItem()
    {
      foreach (LoopListViewItem2 mItem in this.mItemList)
        this.RecycleItemTmp(mItem);
      this.mItemList.Clear();
    }

    private void AdjustContainerPivot(RectTransform rtf)
    {
      Vector2 pivot = rtf.get_pivot();
      if (this.mArrangeType == ListItemArrangeType.BottomToTop)
        pivot.y = (__Null) 0.0;
      else if (this.mArrangeType == ListItemArrangeType.TopToBottom)
        pivot.y = (__Null) 1.0;
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
        pivot.x = (__Null) 0.0;
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
        pivot.x = (__Null) 1.0;
      rtf.set_pivot(pivot);
    }

    private void AdjustPivot(RectTransform rtf)
    {
      Vector2 pivot = rtf.get_pivot();
      if (this.mArrangeType == ListItemArrangeType.BottomToTop)
        pivot.y = (__Null) 0.0;
      else if (this.mArrangeType == ListItemArrangeType.TopToBottom)
        pivot.y = (__Null) 1.0;
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
        pivot.x = (__Null) 0.0;
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
        pivot.x = (__Null) 1.0;
      rtf.set_pivot(pivot);
    }

    private void AdjustContainerAnchor(RectTransform rtf)
    {
      Vector2 anchorMin = rtf.get_anchorMin();
      Vector2 anchorMax = rtf.get_anchorMax();
      if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        anchorMin.y = (__Null) 0.0;
        anchorMax.y = (__Null) 0.0;
      }
      else if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        anchorMin.y = (__Null) 1.0;
        anchorMax.y = (__Null) 1.0;
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        anchorMin.x = (__Null) 0.0;
        anchorMax.x = (__Null) 0.0;
      }
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
      {
        anchorMin.x = (__Null) 1.0;
        anchorMax.x = (__Null) 1.0;
      }
      rtf.set_anchorMin(anchorMin);
      rtf.set_anchorMax(anchorMax);
    }

    private void AdjustAnchor(RectTransform rtf)
    {
      Vector2 anchorMin = rtf.get_anchorMin();
      Vector2 anchorMax = rtf.get_anchorMax();
      if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        anchorMin.y = (__Null) 0.0;
        anchorMax.y = (__Null) 0.0;
      }
      else if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        anchorMin.y = (__Null) 1.0;
        anchorMax.y = (__Null) 1.0;
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        anchorMin.x = (__Null) 0.0;
        anchorMax.x = (__Null) 0.0;
      }
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
      {
        anchorMin.x = (__Null) 1.0;
        anchorMax.x = (__Null) 1.0;
      }
      rtf.set_anchorMin(anchorMin);
      rtf.set_anchorMax(anchorMax);
    }

    private void InitItemPool()
    {
      foreach (ItemPrefabConfData mItemPrefabData in this.mItemPrefabDataList)
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
              this.AdjustAnchor(component);
              this.AdjustPivot(component);
              if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab.GetComponent<LoopListViewItem2>(), (Object) null))
                mItemPrefabData.mItemPrefab.AddComponent<LoopListViewItem2>();
              ItemPool itemPool = new ItemPool();
              itemPool.Init(mItemPrefabData.mItemPrefab, mItemPrefabData.mPadding, mItemPrefabData.mStartPosOffset, mItemPrefabData.mInitCreateCount, this.mContainerTrans);
              this.mItemPoolDict.Add(name, itemPool);
              this.mItemPoolList.Add(itemPool);
            }
          }
        }
      }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      this.mIsDraging = true;
      this.CacheDragPointerEventData(eventData);
      this.mCurSnapData.Clear();
      if (this.mOnBeginDragAction == null)
        return;
      this.mOnBeginDragAction();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      this.mIsDraging = false;
      this.mPointerEventData = (PointerEventData) null;
      if (this.mOnEndDragAction != null)
        this.mOnEndDragAction();
      this.ForceSnapUpdateCheck();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      this.CacheDragPointerEventData(eventData);
      if (this.mOnDragingAction == null)
        return;
      this.mOnDragingAction();
    }

    private void CacheDragPointerEventData(PointerEventData eventData)
    {
      if (this.mPointerEventData == null)
        this.mPointerEventData = new PointerEventData(EventSystem.get_current());
      this.mPointerEventData.set_button(eventData.get_button());
      this.mPointerEventData.set_position(eventData.get_position());
      this.mPointerEventData.set_pointerPressRaycast(eventData.get_pointerPressRaycast());
      this.mPointerEventData.set_pointerCurrentRaycast(eventData.get_pointerCurrentRaycast());
    }

    private LoopListViewItem2 GetNewItemByIndex(int index)
    {
      if (this.mSupportScrollBar && index < 0)
        return (LoopListViewItem2) null;
      if (this.mItemTotalCount > 0 && index >= this.mItemTotalCount)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = this.mOnGetItemByIndex(this, index);
      if (Object.op_Equality((Object) loopListViewItem2, (Object) null))
        return (LoopListViewItem2) null;
      loopListViewItem2.ItemIndex = index;
      loopListViewItem2.ItemCreatedCheckFrameCount = this.mListUpdateCheckFrameCount;
      return loopListViewItem2;
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

    private float GetItemPos(int itemIndex)
    {
      return this.mItemPosMgr.GetItemPos(itemIndex);
    }

    public Vector3 GetItemCornerPosInViewPort(LoopListViewItem2 item, ItemCornerEnum corner = ItemCornerEnum.LeftBottom)
    {
      item.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
      return ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[(int) corner]);
    }

    private void AdjustPanelPos()
    {
      if (this.mItemList.Count == 0)
        return;
      this.UpdateAllShownItemsPos();
      float viewPortSize = this.ViewPortSize;
      float contentPanelSize = this.GetContentPanelSize();
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        if ((double) contentPanelSize <= (double) viewPortSize)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.y = (__Null) 0.0;
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
          this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(this.mItemList[0].StartPosOffset, 0.0f, 0.0f));
          this.UpdateAllShownItemsPos();
        }
        else
        {
          this.mItemList[0].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
          if (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).y < this.mViewPortRectLocalCorners[1].y)
          {
            Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
            anchoredPosition3D.y = (__Null) 0.0;
            this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(this.mItemList[0].StartPosOffset, 0.0f, 0.0f));
            this.UpdateAllShownItemsPos();
          }
          else
          {
            this.mItemList[this.mItemList.Count - 1].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
            float num = (float) (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]).y - this.mViewPortRectLocalCorners[0].y);
            if ((double) num <= 0.0)
              return;
            Vector3 anchoredPosition3D = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
            anchoredPosition3D.y = (__Null) (anchoredPosition3D.y - (double) num);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
            this.UpdateAllShownItemsPos();
          }
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        if ((double) contentPanelSize <= (double) viewPortSize)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.y = (__Null) 0.0;
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
          this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(this.mItemList[0].StartPosOffset, 0.0f, 0.0f));
          this.UpdateAllShownItemsPos();
        }
        else
        {
          this.mItemList[0].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
          if (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]).y > this.mViewPortRectLocalCorners[0].y)
          {
            Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
            anchoredPosition3D.y = (__Null) 0.0;
            this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(this.mItemList[0].StartPosOffset, 0.0f, 0.0f));
            this.UpdateAllShownItemsPos();
          }
          else
          {
            this.mItemList[this.mItemList.Count - 1].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
            float num = (float) (this.mViewPortRectLocalCorners[1].y - ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).y);
            if ((double) num <= 0.0)
              return;
            Vector3 anchoredPosition3D = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
            anchoredPosition3D.y = (__Null) (anchoredPosition3D.y + (double) num);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
            this.UpdateAllShownItemsPos();
          }
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        if ((double) contentPanelSize <= (double) viewPortSize)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.x = (__Null) 0.0;
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
          this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mItemList[0].StartPosOffset, 0.0f));
          this.UpdateAllShownItemsPos();
        }
        else
        {
          this.mItemList[0].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
          if (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).x > this.mViewPortRectLocalCorners[1].x)
          {
            Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
            anchoredPosition3D.x = (__Null) 0.0;
            this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mItemList[0].StartPosOffset, 0.0f));
            this.UpdateAllShownItemsPos();
          }
          else
          {
            this.mItemList[this.mItemList.Count - 1].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
            float num = (float) (this.mViewPortRectLocalCorners[2].x - ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]).x);
            if ((double) num <= 0.0)
              return;
            Vector3 anchoredPosition3D = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
            anchoredPosition3D.x = (__Null) (anchoredPosition3D.x + (double) num);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
            this.UpdateAllShownItemsPos();
          }
        }
      }
      else
      {
        if (this.mArrangeType != ListItemArrangeType.RightToLeft)
          return;
        if ((double) contentPanelSize <= (double) viewPortSize)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.x = (__Null) 0.0;
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
          this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mItemList[0].StartPosOffset, 0.0f));
          this.UpdateAllShownItemsPos();
        }
        else
        {
          this.mItemList[0].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
          if (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]).x < this.mViewPortRectLocalCorners[2].x)
          {
            Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
            anchoredPosition3D.x = (__Null) 0.0;
            this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mItemList[0].StartPosOffset, 0.0f));
            this.UpdateAllShownItemsPos();
          }
          else
          {
            this.mItemList[this.mItemList.Count - 1].CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
            float num = (float) (((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).x - this.mViewPortRectLocalCorners[1].x);
            if ((double) num <= 0.0)
              return;
            Vector3 anchoredPosition3D = this.mItemList[0].CachedRectTransform.get_anchoredPosition3D();
            anchoredPosition3D.x = (__Null) (anchoredPosition3D.x - (double) num);
            this.mItemList[0].CachedRectTransform.set_anchoredPosition3D(anchoredPosition3D);
            this.UpdateAllShownItemsPos();
          }
        }
      }
    }

    private void Update()
    {
      if (!this.mListViewInited)
        return;
      if (this.mNeedAdjustVec)
      {
        this.mNeedAdjustVec = false;
        if (this.mIsVertList)
        {
          if (this.mScrollRect.get_velocity().y * this.mAdjustedVec.y > 0.0)
            this.mScrollRect.set_velocity(this.mAdjustedVec);
        }
        else if (this.mScrollRect.get_velocity().x * this.mAdjustedVec.x > 0.0)
          this.mScrollRect.set_velocity(this.mAdjustedVec);
      }
      if (this.mSupportScrollBar)
        this.mItemPosMgr.Update(false);
      this.UpdateSnapMove(false, false);
      this.UpdateListView(this.mDistanceForRecycle0, this.mDistanceForRecycle1, this.mDistanceForNew0, this.mDistanceForNew1);
      this.ClearAllTmpRecycledItem();
      this.mLastFrameContainerPos = this.mContainerTrans.get_anchoredPosition3D();
    }

    private void UpdateSnapMove(bool immediate = false, bool forceSendEvent = false)
    {
      if (!this.mItemSnapEnable)
        return;
      if (this.mIsVertList)
        this.UpdateSnapVertical(immediate, forceSendEvent);
      else
        this.UpdateSnapHorizontal(immediate, forceSendEvent);
    }

    public void UpdateAllShownItemSnapData()
    {
      if (!this.mItemSnapEnable)
        return;
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      this.mContainerTrans.get_anchoredPosition3D();
      LoopListViewItem2 mItem = this.mItemList[0];
      mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        double num1 = -(1.0 - this.mViewPortSnapPivot.y);
        Rect rect = this.mViewPortRectTransform.get_rect();
        double height = (double) ((Rect) ref rect).get_height();
        float num2 = (float) (num1 * height);
        float y = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).y;
        float num3 = y - mItem.ItemSizeWithPadding;
        float num4 = y - mItem.ItemSize * (float) (1.0 - this.mItemSnapPivot.y);
        for (int index = 0; index < count; ++index)
        {
          this.mItemList[index].DistanceWithViewPortSnapCenter = num2 - num4;
          if (index + 1 < count)
          {
            float num5 = num3;
            num3 -= this.mItemList[index + 1].ItemSizeWithPadding;
            num4 = num5 - this.mItemList[index + 1].ItemSize * (float) (1.0 - this.mItemSnapPivot.y);
          }
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        // ISSUE: variable of the null type
        __Null y1 = this.mViewPortSnapPivot.y;
        Rect rect = this.mViewPortRectTransform.get_rect();
        double height = (double) ((Rect) ref rect).get_height();
        float num1 = (float) (y1 * height);
        float y2 = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]).y;
        float num2 = y2 + mItem.ItemSizeWithPadding;
        float num3 = y2 + mItem.ItemSize * (float) this.mItemSnapPivot.y;
        for (int index = 0; index < count; ++index)
        {
          this.mItemList[index].DistanceWithViewPortSnapCenter = num1 - num3;
          if (index + 1 < count)
          {
            float num4 = num2;
            num2 += this.mItemList[index + 1].ItemSizeWithPadding;
            num3 = num4 + this.mItemList[index + 1].ItemSize * (float) this.mItemSnapPivot.y;
          }
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
      {
        double num1 = -(1.0 - this.mViewPortSnapPivot.x);
        Rect rect = this.mViewPortRectTransform.get_rect();
        double width = (double) ((Rect) ref rect).get_width();
        float num2 = (float) (num1 * width);
        float x = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]).x;
        float num3 = x - mItem.ItemSizeWithPadding;
        float num4 = x - mItem.ItemSize * (float) (1.0 - this.mItemSnapPivot.x);
        for (int index = 0; index < count; ++index)
        {
          this.mItemList[index].DistanceWithViewPortSnapCenter = num2 - num4;
          if (index + 1 < count)
          {
            float num5 = num3;
            num3 -= this.mItemList[index + 1].ItemSizeWithPadding;
            num4 = num5 - this.mItemList[index + 1].ItemSize * (float) (1.0 - this.mItemSnapPivot.x);
          }
        }
      }
      else
      {
        if (this.mArrangeType != ListItemArrangeType.LeftToRight)
          return;
        // ISSUE: variable of the null type
        __Null x1 = this.mViewPortSnapPivot.x;
        Rect rect = this.mViewPortRectTransform.get_rect();
        double width = (double) ((Rect) ref rect).get_width();
        float num1 = (float) (x1 * width);
        float x2 = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).x;
        float num2 = x2 + mItem.ItemSizeWithPadding;
        float num3 = x2 + mItem.ItemSize * (float) this.mItemSnapPivot.x;
        for (int index = 0; index < count; ++index)
        {
          this.mItemList[index].DistanceWithViewPortSnapCenter = num1 - num3;
          if (index + 1 < count)
          {
            float num4 = num2;
            num2 += this.mItemList[index + 1].ItemSizeWithPadding;
            num3 = num4 + this.mItemList[index + 1].ItemSize * (float) this.mItemSnapPivot.x;
          }
        }
      }
    }

    private void UpdateSnapVertical(bool immediate = false, bool forceSendEvent = false)
    {
      if (!this.mItemSnapEnable)
        return;
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
      bool flag = anchoredPosition3D.y != this.mLastSnapCheckPos.y;
      this.mLastSnapCheckPos = anchoredPosition3D;
      if (!flag && this.mLeftSnapUpdateExtraCount > 0)
      {
        --this.mLeftSnapUpdateExtraCount;
        flag = true;
      }
      if (flag)
      {
        LoopListViewItem2 mItem = this.mItemList[0];
        mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        int index1 = -1;
        float num1 = float.MaxValue;
        if (this.mArrangeType == ListItemArrangeType.TopToBottom)
        {
          double num2 = -(1.0 - this.mViewPortSnapPivot.y);
          Rect rect = this.mViewPortRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          float num3 = (float) (num2 * height);
          float y = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).y;
          float num4 = y - mItem.ItemSizeWithPadding;
          float num5 = y - mItem.ItemSize * (float) (1.0 - this.mItemSnapPivot.y);
          for (int index2 = 0; index2 < count; ++index2)
          {
            float num6 = Mathf.Abs(num3 - num5);
            if ((double) num6 < (double) num1)
            {
              num1 = num6;
              index1 = index2;
              if (index2 + 1 < count)
              {
                float num7 = num4;
                num4 -= this.mItemList[index2 + 1].ItemSizeWithPadding;
                num5 = num7 - this.mItemList[index2 + 1].ItemSize * (float) (1.0 - this.mItemSnapPivot.y);
              }
            }
            else
              break;
          }
        }
        else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
        {
          // ISSUE: variable of the null type
          __Null y1 = this.mViewPortSnapPivot.y;
          Rect rect = this.mViewPortRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          float num2 = (float) (y1 * height);
          float y2 = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]).y;
          float num3 = y2 + mItem.ItemSizeWithPadding;
          float num4 = y2 + mItem.ItemSize * (float) this.mItemSnapPivot.y;
          for (int index2 = 0; index2 < count; ++index2)
          {
            float num5 = Mathf.Abs(num2 - num4);
            if ((double) num5 < (double) num1)
            {
              num1 = num5;
              index1 = index2;
              if (index2 + 1 < count)
              {
                float num6 = num3;
                num3 += this.mItemList[index2 + 1].ItemSizeWithPadding;
                num4 = num6 + this.mItemList[index2 + 1].ItemSize * (float) this.mItemSnapPivot.y;
              }
            }
            else
              break;
          }
        }
        if (index1 >= 0)
        {
          int nearestItemIndex = this.mCurSnapNearestItemIndex;
          this.mCurSnapNearestItemIndex = this.mItemList[index1].ItemIndex;
          if ((forceSendEvent || this.mItemList[index1].ItemIndex != nearestItemIndex) && this.mOnSnapNearestChanged != null)
            this.mOnSnapNearestChanged(this, this.mItemList[index1]);
        }
        else
          this.mCurSnapNearestItemIndex = -1;
      }
      if (!this.CanSnap())
      {
        this.ClearSnapData();
      }
      else
      {
        float num1 = Mathf.Abs((float) this.mScrollRect.get_velocity().y);
        this.UpdateCurSnapData();
        if (this.mCurSnapData.mSnapStatus != SnapStatus.SnapMoving)
          return;
        if ((double) num1 > 0.0)
          this.mScrollRect.StopMovement();
        float mCurSnapVal = this.mCurSnapData.mCurSnapVal;
        this.mCurSnapData.mCurSnapVal = Mathf.SmoothDamp(this.mCurSnapData.mCurSnapVal, this.mCurSnapData.mTargetSnapVal, ref this.mSmoothDumpVel, this.mSmoothDumpRate);
        float num2 = this.mCurSnapData.mCurSnapVal - mCurSnapVal;
        if (immediate || (double) Mathf.Abs(this.mCurSnapData.mTargetSnapVal - this.mCurSnapData.mCurSnapVal) < (double) this.mSnapFinishThreshold)
        {
          anchoredPosition3D.y = (__Null) (anchoredPosition3D.y + (double) this.mCurSnapData.mTargetSnapVal - (double) mCurSnapVal);
          this.mCurSnapData.mSnapStatus = SnapStatus.SnapMoveFinish;
          if (this.mOnSnapItemFinished != null)
          {
            LoopListViewItem2 shownItemByItemIndex = this.GetShownItemByItemIndex(this.mCurSnapNearestItemIndex);
            if (Object.op_Inequality((Object) shownItemByItemIndex, (Object) null))
              this.mOnSnapItemFinished(this, shownItemByItemIndex);
          }
        }
        else
          anchoredPosition3D.y = (__Null) (anchoredPosition3D.y + (double) num2);
        if (this.mArrangeType == ListItemArrangeType.TopToBottom)
        {
          // ISSUE: variable of the null type
          __Null y = this.mViewPortRectLocalCorners[0].y;
          Rect rect = this.mContainerTrans.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          float num3 = (float) (y + height);
          anchoredPosition3D.y = (__Null) (double) Mathf.Clamp((float) anchoredPosition3D.y, 0.0f, num3);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
        else
        {
          if (this.mArrangeType != ListItemArrangeType.BottomToTop)
            return;
          // ISSUE: variable of the null type
          __Null y = this.mViewPortRectLocalCorners[1].y;
          Rect rect = this.mContainerTrans.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          float num3 = (float) (y - height);
          anchoredPosition3D.y = (__Null) (double) Mathf.Clamp((float) anchoredPosition3D.y, num3, 0.0f);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
      }
    }

    private void UpdateCurSnapData()
    {
      if (this.mItemList.Count == 0)
      {
        this.mCurSnapData.Clear();
      }
      else
      {
        if (this.mCurSnapData.mSnapStatus == SnapStatus.SnapMoveFinish)
        {
          if (this.mCurSnapData.mSnapTargetIndex == this.mCurSnapNearestItemIndex)
            return;
          this.mCurSnapData.mSnapStatus = SnapStatus.NoTargetSet;
        }
        if (this.mCurSnapData.mSnapStatus == SnapStatus.SnapMoving)
        {
          if (this.mCurSnapData.mSnapTargetIndex == this.mCurSnapNearestItemIndex || this.mCurSnapData.mIsForceSnapTo)
            return;
          this.mCurSnapData.mSnapStatus = SnapStatus.NoTargetSet;
        }
        if (this.mCurSnapData.mSnapStatus == SnapStatus.NoTargetSet)
        {
          if (Object.op_Equality((Object) this.GetShownItemByItemIndex(this.mCurSnapNearestItemIndex), (Object) null))
            return;
          this.mCurSnapData.mSnapTargetIndex = this.mCurSnapNearestItemIndex;
          this.mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
          this.mCurSnapData.mIsForceSnapTo = false;
        }
        if (this.mCurSnapData.mSnapStatus != SnapStatus.TargetHasSet)
          return;
        LoopListViewItem2 shownItemByItemIndex = this.GetShownItemByItemIndex(this.mCurSnapData.mSnapTargetIndex);
        if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        {
          this.mCurSnapData.Clear();
        }
        else
        {
          this.UpdateAllShownItemSnapData();
          this.mCurSnapData.mTargetSnapVal = shownItemByItemIndex.DistanceWithViewPortSnapCenter;
          this.mCurSnapData.mCurSnapVal = 0.0f;
          this.mCurSnapData.mSnapStatus = SnapStatus.SnapMoving;
        }
      }
    }

    public void ClearSnapData()
    {
      this.mCurSnapData.Clear();
    }

    public void SetSnapTargetItemIndex(int itemIndex)
    {
      this.mCurSnapData.mSnapTargetIndex = itemIndex;
      this.mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
      this.mCurSnapData.mIsForceSnapTo = true;
    }

    public int CurSnapNearestItemIndex
    {
      get
      {
        return this.mCurSnapNearestItemIndex;
      }
    }

    public void ForceSnapUpdateCheck()
    {
      if (this.mLeftSnapUpdateExtraCount > 0)
        return;
      this.mLeftSnapUpdateExtraCount = 1;
    }

    private void UpdateSnapHorizontal(bool immediate = false, bool forceSendEvent = false)
    {
      if (!this.mItemSnapEnable)
        return;
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
      bool flag = anchoredPosition3D.x != this.mLastSnapCheckPos.x;
      this.mLastSnapCheckPos = anchoredPosition3D;
      if (!flag && this.mLeftSnapUpdateExtraCount > 0)
      {
        --this.mLeftSnapUpdateExtraCount;
        flag = true;
      }
      if (flag)
      {
        LoopListViewItem2 mItem = this.mItemList[0];
        mItem.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        int index1 = -1;
        float num1 = float.MaxValue;
        if (this.mArrangeType == ListItemArrangeType.RightToLeft)
        {
          double num2 = -(1.0 - this.mViewPortSnapPivot.x);
          Rect rect = this.mViewPortRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          float num3 = (float) (num2 * width);
          float x = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]).x;
          float num4 = x - mItem.ItemSizeWithPadding;
          float num5 = x - mItem.ItemSize * (float) (1.0 - this.mItemSnapPivot.x);
          for (int index2 = 0; index2 < count; ++index2)
          {
            float num6 = Mathf.Abs(num3 - num5);
            if ((double) num6 < (double) num1)
            {
              num1 = num6;
              index1 = index2;
              if (index2 + 1 < count)
              {
                float num7 = num4;
                num4 -= this.mItemList[index2 + 1].ItemSizeWithPadding;
                num5 = num7 - this.mItemList[index2 + 1].ItemSize * (float) (1.0 - this.mItemSnapPivot.x);
              }
            }
            else
              break;
          }
        }
        else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
        {
          // ISSUE: variable of the null type
          __Null x1 = this.mViewPortSnapPivot.x;
          Rect rect = this.mViewPortRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          float num2 = (float) (x1 * width);
          float x2 = (float) ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]).x;
          float num3 = x2 + mItem.ItemSizeWithPadding;
          float num4 = x2 + mItem.ItemSize * (float) this.mItemSnapPivot.x;
          for (int index2 = 0; index2 < count; ++index2)
          {
            float num5 = Mathf.Abs(num2 - num4);
            if ((double) num5 < (double) num1)
            {
              num1 = num5;
              index1 = index2;
              if (index2 + 1 < count)
              {
                float num6 = num3;
                num3 += this.mItemList[index2 + 1].ItemSizeWithPadding;
                num4 = num6 + this.mItemList[index2 + 1].ItemSize * (float) this.mItemSnapPivot.x;
              }
            }
            else
              break;
          }
        }
        if (index1 >= 0)
        {
          int nearestItemIndex = this.mCurSnapNearestItemIndex;
          this.mCurSnapNearestItemIndex = this.mItemList[index1].ItemIndex;
          if ((forceSendEvent || this.mItemList[index1].ItemIndex != nearestItemIndex) && this.mOnSnapNearestChanged != null)
            this.mOnSnapNearestChanged(this, this.mItemList[index1]);
        }
        else
          this.mCurSnapNearestItemIndex = -1;
      }
      if (!this.CanSnap())
      {
        this.ClearSnapData();
      }
      else
      {
        float num1 = Mathf.Abs((float) this.mScrollRect.get_velocity().x);
        this.UpdateCurSnapData();
        if (this.mCurSnapData.mSnapStatus != SnapStatus.SnapMoving)
          return;
        if ((double) num1 > 0.0)
          this.mScrollRect.StopMovement();
        float mCurSnapVal = this.mCurSnapData.mCurSnapVal;
        this.mCurSnapData.mCurSnapVal = Mathf.SmoothDamp(this.mCurSnapData.mCurSnapVal, this.mCurSnapData.mTargetSnapVal, ref this.mSmoothDumpVel, this.mSmoothDumpRate);
        float num2 = this.mCurSnapData.mCurSnapVal - mCurSnapVal;
        if (immediate || (double) Mathf.Abs(this.mCurSnapData.mTargetSnapVal - this.mCurSnapData.mCurSnapVal) < (double) this.mSnapFinishThreshold)
        {
          anchoredPosition3D.x = (__Null) (anchoredPosition3D.x + (double) this.mCurSnapData.mTargetSnapVal - (double) mCurSnapVal);
          this.mCurSnapData.mSnapStatus = SnapStatus.SnapMoveFinish;
          if (this.mOnSnapItemFinished != null)
          {
            LoopListViewItem2 shownItemByItemIndex = this.GetShownItemByItemIndex(this.mCurSnapNearestItemIndex);
            if (Object.op_Inequality((Object) shownItemByItemIndex, (Object) null))
              this.mOnSnapItemFinished(this, shownItemByItemIndex);
          }
        }
        else
          anchoredPosition3D.x = (__Null) (anchoredPosition3D.x + (double) num2);
        if (this.mArrangeType == ListItemArrangeType.LeftToRight)
        {
          // ISSUE: variable of the null type
          __Null x = this.mViewPortRectLocalCorners[2].x;
          Rect rect = this.mContainerTrans.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          float num3 = (float) (x - width);
          anchoredPosition3D.x = (__Null) (double) Mathf.Clamp((float) anchoredPosition3D.x, num3, 0.0f);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
        else
        {
          if (this.mArrangeType != ListItemArrangeType.RightToLeft)
            return;
          // ISSUE: variable of the null type
          __Null x = this.mViewPortRectLocalCorners[1].x;
          Rect rect = this.mContainerTrans.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          float num3 = (float) (x + width);
          anchoredPosition3D.x = (__Null) (double) Mathf.Clamp((float) anchoredPosition3D.x, 0.0f, num3);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
      }
    }

    private bool CanSnap()
    {
      if (this.mIsDraging || Object.op_Inequality((Object) this.mScrollBarClickEventListener, (Object) null) && this.mScrollBarClickEventListener.IsPressd)
        return false;
      if (this.mIsVertList)
      {
        Rect rect = this.mContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_height() <= (double) this.ViewPortHeight)
          return false;
      }
      else
      {
        Rect rect = this.mContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_width() <= (double) this.ViewPortWidth)
          return false;
      }
      float num1 = !this.mIsVertList ? Mathf.Abs((float) this.mScrollRect.get_velocity().x) : Mathf.Abs((float) this.mScrollRect.get_velocity().y);
      if ((double) num1 > (double) this.mSnapVecThreshold)
        return false;
      if ((double) num1 < 2.0)
        return true;
      float num2 = 3f;
      Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
      if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        // ISSUE: variable of the null type
        __Null x = this.mViewPortRectLocalCorners[2].x;
        Rect rect = this.mContainerTrans.get_rect();
        double width = (double) ((Rect) ref rect).get_width();
        float num3 = (float) (x - width);
        if (anchoredPosition3D.x < (double) num3 - (double) num2 || anchoredPosition3D.x > (double) num2)
          return false;
      }
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
      {
        // ISSUE: variable of the null type
        __Null x = this.mViewPortRectLocalCorners[1].x;
        Rect rect = this.mContainerTrans.get_rect();
        double width = (double) ((Rect) ref rect).get_width();
        float num3 = (float) (x + width);
        if (anchoredPosition3D.x > (double) num3 + (double) num2 || anchoredPosition3D.x < -(double) num2)
          return false;
      }
      else if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        // ISSUE: variable of the null type
        __Null y = this.mViewPortRectLocalCorners[0].y;
        Rect rect = this.mContainerTrans.get_rect();
        double height = (double) ((Rect) ref rect).get_height();
        float num3 = (float) (y + height);
        if (anchoredPosition3D.y > (double) num3 + (double) num2 || anchoredPosition3D.y < -(double) num2)
          return false;
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        // ISSUE: variable of the null type
        __Null y = this.mViewPortRectLocalCorners[1].y;
        Rect rect = this.mContainerTrans.get_rect();
        double height = (double) ((Rect) ref rect).get_height();
        float num3 = (float) (y - height);
        if (anchoredPosition3D.y < (double) num3 - (double) num2 || anchoredPosition3D.y > (double) num2)
          return false;
      }
      return true;
    }

    public void UpdateListView(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      ++this.mListUpdateCheckFrameCount;
      if (this.mIsVertList)
      {
        bool flag = true;
        int num1 = 0;
        int num2 = 9999;
        for (; flag; flag = this.UpdateForVertList(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1))
        {
          ++num1;
          if (num1 >= num2)
          {
            Debug.LogError((object) ("UpdateListView Vertical while loop " + (object) num1 + " times! something is wrong!"));
            break;
          }
        }
      }
      else
      {
        bool flag = true;
        int num1 = 0;
        int num2 = 9999;
        for (; flag; flag = this.UpdateForHorizontalList(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1))
        {
          ++num1;
          if (num1 >= num2)
          {
            Debug.LogError((object) ("UpdateListView  Horizontal while loop " + (object) num1 + " times! something is wrong!"));
            break;
          }
        }
      }
    }

    private bool UpdateForVertList(
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
          LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
          if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = newItemByIndex.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) newItemByIndex.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(newItemByIndex);
          newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(newItemByIndex.StartPosOffset, itemPos, 0.0f));
          this.UpdateContentSize();
          return true;
        }
        LoopListViewItem2 mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.mIsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_2.y - this.mViewPortRectLocalCorners[1].y > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.mIsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[0].y - vector3_3.y > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        if (this.mViewPortRectLocalCorners[0].y - vector3_4.y < (double) distanceForNew1)
        {
          if (mItem2.ItemIndex > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
            this.mNeedCheckNextMaxItem = true;
          }
          int index = mItem2.ItemIndex + 1;
          if (index <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdataItemPos();
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double height = (double) ((Rect) ref rect).get_height();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) height, (float) padding);
              }
              this.mItemList.Add(newItemByIndex);
              // ISSUE: variable of the null type
              __Null y = mItem2.CachedRectTransform.get_anchoredPosition3D().y;
              Rect rect1 = mItem2.CachedRectTransform.get_rect();
              double height1 = (double) ((Rect) ref rect1).get_height();
              float num = (float) (y - height1) - mItem2.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(newItemByIndex.StartPosOffset, num, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index > this.mCurReadyMaxItemIndex)
                this.mCurReadyMaxItemIndex = index;
              return true;
            }
          }
        }
        if (vector3_1.y - this.mViewPortRectLocalCorners[1].y < (double) distanceForNew0)
        {
          if (mItem1.ItemIndex < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndex;
            this.mNeedCheckNextMinItem = true;
          }
          int index = mItem1.ItemIndex - 1;
          if (index >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndex;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double height = (double) ((Rect) ref rect).get_height();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) height, (float) padding);
              }
              this.mItemList.Insert(0, newItemByIndex);
              // ISSUE: variable of the null type
              __Null y = mItem1.CachedRectTransform.get_anchoredPosition3D().y;
              Rect rect1 = newItemByIndex.CachedRectTransform.get_rect();
              double height1 = (double) ((Rect) ref rect1).get_height();
              float num = (float) (y + height1) + newItemByIndex.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(newItemByIndex.StartPosOffset, num, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = index;
              return true;
            }
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
          LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
          if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = newItemByIndex.CachedRectTransform.get_rect();
            double height = (double) ((Rect) ref rect).get_height();
            double padding = (double) newItemByIndex.Padding;
            this.SetItemSize(itemIndex, (float) height, (float) padding);
          }
          this.mItemList.Add(newItemByIndex);
          newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(newItemByIndex.StartPosOffset, itemPos, 0.0f));
          this.UpdateContentSize();
          return true;
        }
        LoopListViewItem2 mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.mIsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[0].y - vector3_1.y > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[0]);
        if (!this.mIsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_4.y - this.mViewPortRectLocalCorners[1].y > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        if (vector3_3.y - this.mViewPortRectLocalCorners[1].y < (double) distanceForNew1)
        {
          if (mItem2.ItemIndex > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
            this.mNeedCheckNextMaxItem = true;
          }
          int index = mItem2.ItemIndex + 1;
          if (index <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdataItemPos();
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double height = (double) ((Rect) ref rect).get_height();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) height, (float) padding);
              }
              this.mItemList.Add(newItemByIndex);
              // ISSUE: variable of the null type
              __Null y = mItem2.CachedRectTransform.get_anchoredPosition3D().y;
              Rect rect1 = mItem2.CachedRectTransform.get_rect();
              double height1 = (double) ((Rect) ref rect1).get_height();
              float num = (float) (y + height1) + mItem2.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(newItemByIndex.StartPosOffset, num, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index > this.mCurReadyMaxItemIndex)
                this.mCurReadyMaxItemIndex = index;
              return true;
            }
          }
        }
        if (this.mViewPortRectLocalCorners[0].y - vector3_2.y < (double) distanceForNew0)
        {
          if (mItem1.ItemIndex < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndex;
            this.mNeedCheckNextMinItem = true;
          }
          int index = mItem1.ItemIndex - 1;
          if (index >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mNeedCheckNextMinItem = false;
              return false;
            }
            if (this.mSupportScrollBar)
            {
              int itemIndex = index;
              Rect rect = newItemByIndex.CachedRectTransform.get_rect();
              double height = (double) ((Rect) ref rect).get_height();
              double padding = (double) newItemByIndex.Padding;
              this.SetItemSize(itemIndex, (float) height, (float) padding);
            }
            this.mItemList.Insert(0, newItemByIndex);
            // ISSUE: variable of the null type
            __Null y = mItem1.CachedRectTransform.get_anchoredPosition3D().y;
            Rect rect1 = newItemByIndex.CachedRectTransform.get_rect();
            double height1 = (double) ((Rect) ref rect1).get_height();
            float num = (float) (y - height1) - newItemByIndex.Padding;
            newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(newItemByIndex.StartPosOffset, num, 0.0f));
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
            if (index < this.mCurReadyMinItemIndex)
              this.mCurReadyMinItemIndex = index;
            return true;
          }
        }
      }
      return false;
    }

    private bool UpdateForHorizontalList(
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
          LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
          if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = newItemByIndex.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) newItemByIndex.Padding;
            this.SetItemSize(itemIndex, (float) width, (float) padding);
          }
          this.mItemList.Add(newItemByIndex);
          newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemPos, newItemByIndex.StartPosOffset, 0.0f));
          this.UpdateContentSize();
          return true;
        }
        LoopListViewItem2 mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.mIsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[1].x - vector3_2.x > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.mIsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_3.x - this.mViewPortRectLocalCorners[2].x > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        if (vector3_4.x - this.mViewPortRectLocalCorners[2].x < (double) distanceForNew1)
        {
          if (mItem2.ItemIndex > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
            this.mNeedCheckNextMaxItem = true;
          }
          int index = mItem2.ItemIndex + 1;
          if (index <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdataItemPos();
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Add(newItemByIndex);
              // ISSUE: variable of the null type
              __Null x = mItem2.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = mItem2.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x + width1) + mItem2.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, newItemByIndex.StartPosOffset, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index > this.mCurReadyMaxItemIndex)
                this.mCurReadyMaxItemIndex = index;
              return true;
            }
          }
        }
        if (this.mViewPortRectLocalCorners[1].x - vector3_1.x < (double) distanceForNew0)
        {
          if (mItem1.ItemIndex < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndex;
            this.mNeedCheckNextMinItem = true;
          }
          int index = mItem1.ItemIndex - 1;
          if (index >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndex;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Insert(0, newItemByIndex);
              // ISSUE: variable of the null type
              __Null x = mItem1.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = newItemByIndex.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x - width1) - newItemByIndex.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, newItemByIndex.StartPosOffset, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = index;
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
          LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
          if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            return false;
          if (this.mSupportScrollBar)
          {
            int itemIndex = index;
            Rect rect = newItemByIndex.CachedRectTransform.get_rect();
            double width = (double) ((Rect) ref rect).get_width();
            double padding = (double) newItemByIndex.Padding;
            this.SetItemSize(itemIndex, (float) width, (float) padding);
          }
          this.mItemList.Add(newItemByIndex);
          newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(itemPos, newItemByIndex.StartPosOffset, 0.0f));
          this.UpdateContentSize();
          return true;
        }
        LoopListViewItem2 mItem1 = this.mItemList[0];
        mItem1.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_1 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_2 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.mIsDraging && mItem1.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && vector3_1.x - this.mViewPortRectLocalCorners[2].x > (double) distanceForRecycle0)
        {
          this.mItemList.RemoveAt(0);
          this.RecycleItemTmp(mItem1);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        mItem2.CachedRectTransform.GetWorldCorners(this.mItemWorldCorners);
        Vector3 vector3_3 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[1]);
        Vector3 vector3_4 = ((Transform) this.mViewPortRectTransform).InverseTransformPoint(this.mItemWorldCorners[2]);
        if (!this.mIsDraging && mItem2.ItemCreatedCheckFrameCount != this.mListUpdateCheckFrameCount && this.mViewPortRectLocalCorners[1].x - vector3_4.x > (double) distanceForRecycle1)
        {
          this.mItemList.RemoveAt(this.mItemList.Count - 1);
          this.RecycleItemTmp(mItem2);
          if (!this.mSupportScrollBar)
          {
            this.UpdateContentSize();
            this.CheckIfNeedUpdataItemPos();
          }
          return true;
        }
        if (this.mViewPortRectLocalCorners[1].x - vector3_3.x < (double) distanceForNew1)
        {
          if (mItem2.ItemIndex > this.mCurReadyMaxItemIndex)
          {
            this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
            this.mNeedCheckNextMaxItem = true;
          }
          int index = mItem2.ItemIndex + 1;
          if (index <= this.mCurReadyMaxItemIndex || this.mNeedCheckNextMaxItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mCurReadyMaxItemIndex = mItem2.ItemIndex;
              this.mNeedCheckNextMaxItem = false;
              this.CheckIfNeedUpdataItemPos();
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Add(newItemByIndex);
              // ISSUE: variable of the null type
              __Null x = mItem2.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = mItem2.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x - width1) - mItem2.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, newItemByIndex.StartPosOffset, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index > this.mCurReadyMaxItemIndex)
                this.mCurReadyMaxItemIndex = index;
              return true;
            }
          }
        }
        if (vector3_2.x - this.mViewPortRectLocalCorners[2].x < (double) distanceForNew0)
        {
          if (mItem1.ItemIndex < this.mCurReadyMinItemIndex)
          {
            this.mCurReadyMinItemIndex = mItem1.ItemIndex;
            this.mNeedCheckNextMinItem = true;
          }
          int index = mItem1.ItemIndex - 1;
          if (index >= this.mCurReadyMinItemIndex || this.mNeedCheckNextMinItem)
          {
            LoopListViewItem2 newItemByIndex = this.GetNewItemByIndex(index);
            if (Object.op_Equality((Object) newItemByIndex, (Object) null))
            {
              this.mCurReadyMinItemIndex = mItem1.ItemIndex;
              this.mNeedCheckNextMinItem = false;
            }
            else
            {
              if (this.mSupportScrollBar)
              {
                int itemIndex = index;
                Rect rect = newItemByIndex.CachedRectTransform.get_rect();
                double width = (double) ((Rect) ref rect).get_width();
                double padding = (double) newItemByIndex.Padding;
                this.SetItemSize(itemIndex, (float) width, (float) padding);
              }
              this.mItemList.Insert(0, newItemByIndex);
              // ISSUE: variable of the null type
              __Null x = mItem1.CachedRectTransform.get_anchoredPosition3D().x;
              Rect rect1 = newItemByIndex.CachedRectTransform.get_rect();
              double width1 = (double) ((Rect) ref rect1).get_width();
              float num = (float) (x + width1) + newItemByIndex.Padding;
              newItemByIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(num, newItemByIndex.StartPosOffset, 0.0f));
              this.UpdateContentSize();
              this.CheckIfNeedUpdataItemPos();
              if (index < this.mCurReadyMinItemIndex)
                this.mCurReadyMinItemIndex = index;
              return true;
            }
          }
        }
      }
      return false;
    }

    private float GetContentPanelSize()
    {
      if (this.mSupportScrollBar)
      {
        float num = (double) this.mItemPosMgr.mTotalSize <= 0.0 ? 0.0f : this.mItemPosMgr.mTotalSize - this.mLastItemPadding;
        if ((double) num < 0.0)
          num = 0.0f;
        return num;
      }
      int count = this.mItemList.Count;
      switch (count)
      {
        case 0:
          return 0.0f;
        case 1:
          return this.mItemList[0].ItemSize;
        case 2:
          return this.mItemList[0].ItemSizeWithPadding + this.mItemList[1].ItemSize;
        default:
          float num1 = 0.0f;
          for (int index = 0; index < count - 1; ++index)
            num1 += this.mItemList[index].ItemSizeWithPadding;
          return num1 + this.mItemList[count - 1].ItemSize;
      }
    }

    private void CheckIfNeedUpdataItemPos()
    {
      if (this.mItemList.Count == 0)
        return;
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        LoopListViewItem2 mItem1 = this.mItemList[0];
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        float contentPanelSize = this.GetContentPanelSize();
        if ((double) mItem1.TopY > 0.0 || mItem1.ItemIndex == this.mCurReadyMinItemIndex && (double) mItem1.TopY != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          if (-(double) mItem2.BottomY <= (double) contentPanelSize && (mItem2.ItemIndex != this.mCurReadyMaxItemIndex || -(double) mItem2.BottomY == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        LoopListViewItem2 mItem1 = this.mItemList[0];
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        float contentPanelSize = this.GetContentPanelSize();
        if ((double) mItem1.BottomY < 0.0 || mItem1.ItemIndex == this.mCurReadyMinItemIndex && (double) mItem1.BottomY != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          if ((double) mItem2.TopY <= (double) contentPanelSize && (mItem2.ItemIndex != this.mCurReadyMaxItemIndex || (double) mItem2.TopY == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        LoopListViewItem2 mItem1 = this.mItemList[0];
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        float contentPanelSize = this.GetContentPanelSize();
        if ((double) mItem1.LeftX < 0.0 || mItem1.ItemIndex == this.mCurReadyMinItemIndex && (double) mItem1.LeftX != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          if ((double) mItem2.RightX <= (double) contentPanelSize && (mItem2.ItemIndex != this.mCurReadyMaxItemIndex || (double) mItem2.RightX == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
      else
      {
        if (this.mArrangeType != ListItemArrangeType.RightToLeft)
          return;
        LoopListViewItem2 mItem1 = this.mItemList[0];
        LoopListViewItem2 mItem2 = this.mItemList[this.mItemList.Count - 1];
        float contentPanelSize = this.GetContentPanelSize();
        if ((double) mItem1.RightX > 0.0 || mItem1.ItemIndex == this.mCurReadyMinItemIndex && (double) mItem1.RightX != 0.0)
        {
          this.UpdateAllShownItemsPos();
        }
        else
        {
          if (-(double) mItem2.LeftX <= (double) contentPanelSize && (mItem2.ItemIndex != this.mCurReadyMaxItemIndex || -(double) mItem2.LeftX == (double) contentPanelSize))
            return;
          this.UpdateAllShownItemsPos();
        }
      }
    }

    private void UpdateAllShownItemsPos()
    {
      int count = this.mItemList.Count;
      if (count == 0)
        return;
      this.mAdjustedVec = Vector2.op_Implicit(Vector3.op_Division(Vector3.op_Subtraction(this.mContainerTrans.get_anchoredPosition3D(), this.mLastFrameContainerPos), Time.get_deltaTime()));
      if (this.mArrangeType == ListItemArrangeType.TopToBottom)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = -this.GetItemPos(this.mItemList[0].ItemIndex);
        float y = (float) this.mItemList[0].CachedRectTransform.get_anchoredPosition3D().y;
        float num2 = num1 - y;
        float num3 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopListViewItem2 mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(mItem.StartPosOffset, num3, 0.0f));
          double num4 = (double) num3;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          num3 = (float) (num4 - height) - mItem.Padding;
        }
        if ((double) num2 != 0.0)
        {
          Vector2 vector2 = Vector2.op_Implicit(this.mContainerTrans.get_anchoredPosition3D());
          vector2.y = (__Null) (vector2.y - (double) num2);
          this.mContainerTrans.set_anchoredPosition3D(Vector2.op_Implicit(vector2));
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.BottomToTop)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = this.GetItemPos(this.mItemList[0].ItemIndex);
        float y = (float) this.mItemList[0].CachedRectTransform.get_anchoredPosition3D().y;
        float num2 = num1 - y;
        float num3 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopListViewItem2 mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(mItem.StartPosOffset, num3, 0.0f));
          double num4 = (double) num3;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          num3 = (float) (num4 + height) + mItem.Padding;
        }
        if ((double) num2 != 0.0)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.y = (__Null) (anchoredPosition3D.y - (double) num2);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.LeftToRight)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = this.GetItemPos(this.mItemList[0].ItemIndex);
        float x = (float) this.mItemList[0].CachedRectTransform.get_anchoredPosition3D().x;
        float num2 = num1 - x;
        float num3 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopListViewItem2 mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(num3, mItem.StartPosOffset, 0.0f));
          double num4 = (double) num3;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          num3 = (float) (num4 + width) + mItem.Padding;
        }
        if ((double) num2 != 0.0)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.x = (__Null) (anchoredPosition3D.x - (double) num2);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
      }
      else if (this.mArrangeType == ListItemArrangeType.RightToLeft)
      {
        float num1 = 0.0f;
        if (this.mSupportScrollBar)
          num1 = -this.GetItemPos(this.mItemList[0].ItemIndex);
        float x = (float) this.mItemList[0].CachedRectTransform.get_anchoredPosition3D().x;
        float num2 = num1 - x;
        float num3 = num1;
        for (int index = 0; index < count; ++index)
        {
          LoopListViewItem2 mItem = this.mItemList[index];
          mItem.CachedRectTransform.set_anchoredPosition3D(new Vector3(num3, mItem.StartPosOffset, 0.0f));
          double num4 = (double) num3;
          Rect rect = mItem.CachedRectTransform.get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          num3 = (float) (num4 - width) - mItem.Padding;
        }
        if ((double) num2 != 0.0)
        {
          Vector3 anchoredPosition3D = this.mContainerTrans.get_anchoredPosition3D();
          anchoredPosition3D.x = (__Null) (anchoredPosition3D.x - (double) num2);
          this.mContainerTrans.set_anchoredPosition3D(anchoredPosition3D);
        }
      }
      if (!this.mIsDraging)
        return;
      this.mScrollRect.OnBeginDrag(this.mPointerEventData);
      this.mScrollRect.Rebuild((CanvasUpdate) 2);
      this.mScrollRect.set_velocity(this.mAdjustedVec);
      this.mNeedAdjustVec = true;
    }

    private void UpdateContentSize()
    {
      float contentPanelSize = this.GetContentPanelSize();
      if (this.mIsVertList)
      {
        Rect rect = this.mContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_height() == (double) contentPanelSize)
          return;
        this.mContainerTrans.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, contentPanelSize);
      }
      else
      {
        Rect rect = this.mContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_width() == (double) contentPanelSize)
          return;
        this.mContainerTrans.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, contentPanelSize);
      }
    }

    private class SnapData
    {
      public SnapStatus mSnapStatus;
      public int mSnapTargetIndex;
      public float mTargetSnapVal;
      public float mCurSnapVal;
      public bool mIsForceSnapTo;

      public void Clear()
      {
        this.mSnapStatus = SnapStatus.NoTargetSet;
        this.mIsForceSnapTo = false;
      }
    }
  }
}
