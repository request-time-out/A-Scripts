// Decompiled with JetBrains decompiler
// Type: SuperScrollView.LoopStaggeredGridView
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
  public class LoopStaggeredGridView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
  {
    private Dictionary<string, StaggeredGridItemPool> mItemPoolDict;
    private List<StaggeredGridItemPool> mItemPoolList;
    [SerializeField]
    private List<StaggeredGridItemPrefabConfData> mItemPrefabDataList;
    [SerializeField]
    private ListItemArrangeType mArrangeType;
    private RectTransform mContainerTrans;
    private ScrollRect mScrollRect;
    private int mGroupCount;
    private List<StaggeredGridItemGroup> mItemGroupList;
    private List<ItemIndexData> mItemIndexDataList;
    private RectTransform mScrollRectTransform;
    private RectTransform mViewPortRectTransform;
    private float mItemDefaultWithPaddingSize;
    private int mItemTotalCount;
    private bool mIsVertList;
    private Func<LoopStaggeredGridView, int, LoopStaggeredGridViewItem> mOnGetItemByItemIndex;
    private Vector3[] mItemWorldCorners;
    private Vector3[] mViewPortRectLocalCorners;
    private float mDistanceForRecycle0;
    private float mDistanceForNew0;
    private float mDistanceForRecycle1;
    private float mDistanceForNew1;
    private bool mIsDraging;
    private PointerEventData mPointerEventData;
    public Action mOnBeginDragAction;
    public Action mOnDragingAction;
    public Action mOnEndDragAction;
    private Vector3 mLastFrameContainerPos;
    private bool mListViewInited;
    private int mListUpdateCheckFrameCount;
    private GridViewLayoutParam mLayoutParam;

    public LoopStaggeredGridView()
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

    public List<StaggeredGridItemPrefabConfData> ItemPrefabDataList
    {
      get
      {
        return this.mItemPrefabDataList;
      }
    }

    public int ListUpdateCheckFrameCount
    {
      get
      {
        return this.mListUpdateCheckFrameCount;
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

    public GridViewLayoutParam LayoutParam
    {
      get
      {
        return this.mLayoutParam;
      }
    }

    public bool IsInited
    {
      get
      {
        return this.mListViewInited;
      }
    }

    public StaggeredGridItemGroup GetItemGroupByIndex(int index)
    {
      int count = this.mItemGroupList.Count;
      return index < 0 || index >= count ? (StaggeredGridItemGroup) null : this.mItemGroupList[index];
    }

    public StaggeredGridItemPrefabConfData GetItemPrefabConfData(
      string prefabName)
    {
      foreach (StaggeredGridItemPrefabConfData mItemPrefabData in this.mItemPrefabDataList)
      {
        if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab, (Object) null))
          Debug.LogError((object) "A item prefab is null ");
        else if (prefabName == ((Object) mItemPrefabData.mItemPrefab).get_name())
          return mItemPrefabData;
      }
      return (StaggeredGridItemPrefabConfData) null;
    }

    public void InitListView(
      int itemTotalCount,
      GridViewLayoutParam layoutParam,
      Func<LoopStaggeredGridView, int, LoopStaggeredGridViewItem> onGetItemByItemIndex,
      StaggeredGridViewInitParam initParam = null)
    {
      this.mLayoutParam = layoutParam;
      if (this.mLayoutParam == null)
      {
        Debug.LogError((object) "layoutParam can not be null!");
      }
      else
      {
        if (!this.mLayoutParam.CheckParam())
          return;
        if (initParam != null)
        {
          this.mDistanceForRecycle0 = initParam.mDistanceForRecycle0;
          this.mDistanceForNew0 = initParam.mDistanceForNew0;
          this.mDistanceForRecycle1 = initParam.mDistanceForRecycle1;
          this.mDistanceForNew1 = initParam.mDistanceForNew1;
          this.mItemDefaultWithPaddingSize = initParam.mItemDefaultWithPaddingSize;
        }
        this.mScrollRect = (ScrollRect) ((Component) this).get_gameObject().GetComponent<ScrollRect>();
        if (Object.op_Equality((Object) this.mScrollRect, (Object) null))
        {
          Debug.LogError((object) "LoopStaggeredGridView Init Failed! ScrollRect component not found!");
        }
        else
        {
          if ((double) this.mDistanceForRecycle0 <= (double) this.mDistanceForNew0)
            Debug.LogError((object) "mDistanceForRecycle0 should be bigger than mDistanceForNew0");
          if ((double) this.mDistanceForRecycle1 <= (double) this.mDistanceForNew1)
            Debug.LogError((object) "mDistanceForRecycle1 should be bigger than mDistanceForNew1");
          this.mScrollRectTransform = (RectTransform) ((Component) this.mScrollRect).GetComponent<RectTransform>();
          this.mContainerTrans = this.mScrollRect.get_content();
          this.mViewPortRectTransform = this.mScrollRect.get_viewport();
          this.mGroupCount = this.mLayoutParam.mColumnOrRowCount;
          if (Object.op_Equality((Object) this.mViewPortRectTransform, (Object) null))
            this.mViewPortRectTransform = this.mScrollRectTransform;
          if (this.mScrollRect.get_horizontalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_horizontalScrollbar(), (Object) null))
            Debug.LogError((object) "ScrollRect.horizontalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
          if (this.mScrollRect.get_verticalScrollbarVisibility() == 2 && Object.op_Inequality((Object) this.mScrollRect.get_verticalScrollbar(), (Object) null))
            Debug.LogError((object) "ScrollRect.verticalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
          this.mIsVertList = this.mArrangeType == ListItemArrangeType.TopToBottom || this.mArrangeType == ListItemArrangeType.BottomToTop;
          this.mScrollRect.set_horizontal(!this.mIsVertList);
          this.mScrollRect.set_vertical(this.mIsVertList);
          this.AdjustPivot(this.mViewPortRectTransform);
          this.AdjustAnchor(this.mContainerTrans);
          this.AdjustContainerPivot(this.mContainerTrans);
          this.InitItemPool();
          this.mOnGetItemByItemIndex = onGetItemByItemIndex;
          if (this.mListViewInited)
            Debug.LogError((object) "LoopStaggeredGridView.InitListView method can be called only once.");
          this.mListViewInited = true;
          this.mViewPortRectTransform.GetLocalCorners(this.mViewPortRectLocalCorners);
          this.mContainerTrans.set_anchoredPosition3D(Vector3.get_zero());
          this.mItemTotalCount = itemTotalCount;
          this.UpdateLayoutParamAutoValue();
          this.mItemGroupList.Clear();
          for (int groupIndex = 0; groupIndex < this.mGroupCount; ++groupIndex)
          {
            StaggeredGridItemGroup staggeredGridItemGroup = new StaggeredGridItemGroup();
            staggeredGridItemGroup.Init(this, this.mItemTotalCount, groupIndex, new Func<int, int, LoopStaggeredGridViewItem>(this.GetNewItemByGroupAndIndex));
            this.mItemGroupList.Add(staggeredGridItemGroup);
          }
          this.UpdateContentSize();
        }
      }
    }

    public void ResetGridViewLayoutParam(int itemTotalCount, GridViewLayoutParam layoutParam)
    {
      if (!this.mListViewInited)
      {
        Debug.LogError((object) "ResetLayoutParam can not use before LoopStaggeredGridView.InitListView are called!");
      }
      else
      {
        this.mScrollRect.StopMovement();
        this.SetListItemCount(0, true);
        this.RecycleAllItem();
        this.ClearAllTmpRecycledItem();
        this.mLayoutParam = layoutParam;
        if (this.mLayoutParam == null)
        {
          Debug.LogError((object) "layoutParam can not be null!");
        }
        else
        {
          if (!this.mLayoutParam.CheckParam())
            return;
          this.mGroupCount = this.mLayoutParam.mColumnOrRowCount;
          this.mViewPortRectTransform.GetLocalCorners(this.mViewPortRectLocalCorners);
          this.mContainerTrans.set_anchoredPosition3D(Vector3.get_zero());
          this.mItemTotalCount = itemTotalCount;
          this.UpdateLayoutParamAutoValue();
          this.mItemGroupList.Clear();
          for (int groupIndex = 0; groupIndex < this.mGroupCount; ++groupIndex)
          {
            StaggeredGridItemGroup staggeredGridItemGroup = new StaggeredGridItemGroup();
            staggeredGridItemGroup.Init(this, this.mItemTotalCount, groupIndex, new Func<int, int, LoopStaggeredGridViewItem>(this.GetNewItemByGroupAndIndex));
            this.mItemGroupList.Add(staggeredGridItemGroup);
          }
          this.UpdateContentSize();
        }
      }
    }

    private void UpdateLayoutParamAutoValue()
    {
      if (this.mLayoutParam.mCustomColumnOrRowOffsetArray != null)
        return;
      this.mLayoutParam.mCustomColumnOrRowOffsetArray = new float[this.mGroupCount];
      float num1 = this.mLayoutParam.mItemWidthOrHeight * (float) this.mGroupCount;
      float num2 = !this.IsVertList ? (this.ViewPortHeight - this.mLayoutParam.mPadding1 - this.mLayoutParam.mPadding2 - num1) / (float) (this.mGroupCount - 1) : (this.ViewPortWidth - this.mLayoutParam.mPadding1 - this.mLayoutParam.mPadding2 - num1) / (float) (this.mGroupCount - 1);
      float num3 = this.mLayoutParam.mPadding1;
      for (int index = 0; index < this.mGroupCount; ++index)
      {
        this.mLayoutParam.mCustomColumnOrRowOffsetArray[index] = !this.IsVertList ? -num3 : num3;
        num3 = num3 + this.mLayoutParam.mItemWidthOrHeight + num2;
      }
    }

    public LoopStaggeredGridViewItem NewListViewItem(string itemPrefabName)
    {
      StaggeredGridItemPool staggeredGridItemPool = (StaggeredGridItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(itemPrefabName, out staggeredGridItemPool))
        return (LoopStaggeredGridViewItem) null;
      LoopStaggeredGridViewItem staggeredGridViewItem = staggeredGridItemPool.GetItem();
      RectTransform component = (RectTransform) ((Component) staggeredGridViewItem).GetComponent<RectTransform>();
      ((Transform) component).SetParent((Transform) this.mContainerTrans);
      ((Transform) component).set_localScale(Vector3.get_one());
      component.set_anchoredPosition3D(Vector3.get_zero());
      ((Transform) component).set_localEulerAngles(Vector3.get_zero());
      staggeredGridViewItem.ParentListView = this;
      return staggeredGridViewItem;
    }

    public void SetListItemCount(int itemCount, bool resetPos = true)
    {
      if (itemCount == this.mItemTotalCount)
        return;
      int count1 = this.mItemGroupList.Count;
      this.mItemTotalCount = itemCount;
      for (int index = 0; index < count1; ++index)
        this.mItemGroupList[index].SetListItemCount(this.mItemTotalCount);
      this.UpdateContentSize();
      if (this.mItemTotalCount == 0)
      {
        this.mItemIndexDataList.Clear();
        this.ClearAllTmpRecycledItem();
      }
      else
      {
        int count2 = this.mItemIndexDataList.Count;
        if (count2 > this.mItemTotalCount)
          this.mItemIndexDataList.RemoveRange(this.mItemTotalCount, count2 - this.mItemTotalCount);
        if (resetPos)
        {
          this.MovePanelToItemIndex(0, 0.0f);
        }
        else
        {
          if (count2 <= this.mItemTotalCount)
            return;
          this.MovePanelToItemIndex(this.mItemTotalCount - 1, 0.0f);
        }
      }
    }

    public void MovePanelToItemIndex(int itemIndex, float offset)
    {
      this.mScrollRect.StopMovement();
      if (this.mItemTotalCount == 0 || itemIndex < 0)
        return;
      this.CheckAllGroupIfNeedUpdateItemPos();
      this.UpdateContentSize();
      float viewPortSize = this.ViewPortSize;
      float contentSize1 = this.GetContentSize();
      if ((double) contentSize1 <= (double) viewPortSize)
      {
        if (this.IsVertList)
          this.SetAnchoredPositionY(this.mContainerTrans, 0.0f);
        else
          this.SetAnchoredPositionX(this.mContainerTrans, 0.0f);
      }
      else
      {
        if (itemIndex >= this.mItemTotalCount)
          itemIndex = this.mItemTotalCount - 1;
        float absPosByItemIndex = this.GetItemAbsPosByItemIndex(itemIndex);
        if ((double) absPosByItemIndex < 0.0)
          return;
        if (this.IsVertList)
        {
          float num1 = this.mArrangeType != ListItemArrangeType.TopToBottom ? -1f : 1f;
          float num2 = absPosByItemIndex + offset;
          if ((double) num2 < 0.0)
            num2 = 0.0f;
          if ((double) contentSize1 - (double) num2 >= (double) viewPortSize)
          {
            this.SetAnchoredPositionY(this.mContainerTrans, num1 * num2);
          }
          else
          {
            this.SetAnchoredPositionY(this.mContainerTrans, num1 * (contentSize1 - viewPortSize));
            this.UpdateListView(viewPortSize + 100f, viewPortSize + 100f, viewPortSize, viewPortSize);
            this.ClearAllTmpRecycledItem();
            this.UpdateContentSize();
            float contentSize2 = this.GetContentSize();
            if ((double) contentSize2 - (double) num2 >= (double) viewPortSize)
              this.SetAnchoredPositionY(this.mContainerTrans, num1 * num2);
            else
              this.SetAnchoredPositionY(this.mContainerTrans, num1 * (contentSize2 - viewPortSize));
          }
        }
        else
        {
          float num1 = this.mArrangeType != ListItemArrangeType.RightToLeft ? -1f : 1f;
          float num2 = absPosByItemIndex + offset;
          if ((double) num2 < 0.0)
            num2 = 0.0f;
          if ((double) contentSize1 - (double) num2 >= (double) viewPortSize)
          {
            this.SetAnchoredPositionX(this.mContainerTrans, num1 * num2);
          }
          else
          {
            this.SetAnchoredPositionX(this.mContainerTrans, num1 * (contentSize1 - viewPortSize));
            this.UpdateListView(viewPortSize + 100f, viewPortSize + 100f, viewPortSize, viewPortSize);
            this.ClearAllTmpRecycledItem();
            this.UpdateContentSize();
            float contentSize2 = this.GetContentSize();
            if ((double) contentSize2 - (double) num2 >= (double) viewPortSize)
              this.SetAnchoredPositionX(this.mContainerTrans, num1 * num2);
            else
              this.SetAnchoredPositionX(this.mContainerTrans, num1 * (contentSize2 - viewPortSize));
          }
        }
      }
    }

    public LoopStaggeredGridViewItem GetShownItemByItemIndex(int itemIndex)
    {
      ItemIndexData itemIndexData = this.GetItemIndexData(itemIndex);
      return itemIndexData == null ? (LoopStaggeredGridViewItem) null : this.GetItemGroupByIndex(itemIndexData.mGroupIndex).GetShownItemByIndexInGroup(itemIndexData.mIndexInGroup);
    }

    public void RefreshAllShownItem()
    {
      int count = this.mItemGroupList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemGroupList[index].RefreshAllShownItem();
    }

    public void OnItemSizeChanged(int itemIndex)
    {
      ItemIndexData itemIndexData = this.GetItemIndexData(itemIndex);
      if (itemIndexData == null)
        return;
      this.GetItemGroupByIndex(itemIndexData.mGroupIndex).OnItemSizeChanged(itemIndexData.mIndexInGroup);
    }

    public void RefreshItemByItemIndex(int itemIndex)
    {
      ItemIndexData itemIndexData = this.GetItemIndexData(itemIndex);
      if (itemIndexData == null)
        return;
      this.GetItemGroupByIndex(itemIndexData.mGroupIndex).RefreshItemByIndexInGroup(itemIndexData.mIndexInGroup);
    }

    public void ResetListView(bool resetPos = true)
    {
      this.mViewPortRectTransform.GetLocalCorners(this.mViewPortRectLocalCorners);
      if (!resetPos)
        return;
      this.mContainerTrans.set_anchoredPosition3D(Vector3.get_zero());
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

    public void RecycleAllItem()
    {
      int count = this.mItemGroupList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemGroupList[index].RecycleAllItem();
    }

    public void RecycleItemTmp(LoopStaggeredGridViewItem item)
    {
      if (Object.op_Equality((Object) item, (Object) null) || string.IsNullOrEmpty(item.ItemPrefabName))
        return;
      StaggeredGridItemPool staggeredGridItemPool = (StaggeredGridItemPool) null;
      if (!this.mItemPoolDict.TryGetValue(item.ItemPrefabName, out staggeredGridItemPool))
        return;
      staggeredGridItemPool.RecycleItem(item);
    }

    public void ClearAllTmpRecycledItem()
    {
      int count = this.mItemPoolList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemPoolList[index].ClearTmpRecycledItem();
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
      foreach (StaggeredGridItemPrefabConfData mItemPrefabData in this.mItemPrefabDataList)
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
              if (Object.op_Equality((Object) mItemPrefabData.mItemPrefab.GetComponent<LoopStaggeredGridViewItem>(), (Object) null))
                mItemPrefabData.mItemPrefab.AddComponent<LoopStaggeredGridViewItem>();
              StaggeredGridItemPool staggeredGridItemPool = new StaggeredGridItemPool();
              staggeredGridItemPool.Init(mItemPrefabData.mItemPrefab, mItemPrefabData.mPadding, mItemPrefabData.mInitCreateCount, this.mContainerTrans);
              this.mItemPoolDict.Add(name, staggeredGridItemPool);
              this.mItemPoolList.Add(staggeredGridItemPool);
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
      if (this.mOnEndDragAction == null)
        return;
      this.mOnEndDragAction();
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

    public int CurMaxCreatedItemIndexCount
    {
      get
      {
        return this.mItemIndexDataList.Count;
      }
    }

    private void SetAnchoredPositionX(RectTransform rtf, float x)
    {
      Vector3 anchoredPosition3D = rtf.get_anchoredPosition3D();
      anchoredPosition3D.x = (__Null) (double) x;
      rtf.set_anchoredPosition3D(anchoredPosition3D);
    }

    private void SetAnchoredPositionY(RectTransform rtf, float y)
    {
      Vector3 anchoredPosition3D = rtf.get_anchoredPosition3D();
      anchoredPosition3D.y = (__Null) (double) y;
      rtf.set_anchoredPosition3D(anchoredPosition3D);
    }

    public ItemIndexData GetItemIndexData(int itemIndex)
    {
      int count = this.mItemIndexDataList.Count;
      return itemIndex < 0 || itemIndex >= count ? (ItemIndexData) null : this.mItemIndexDataList[itemIndex];
    }

    public void UpdateAllGroupShownItemsPos()
    {
      int count = this.mItemGroupList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemGroupList[index].UpdateAllShownItemsPos();
    }

    private void CheckAllGroupIfNeedUpdateItemPos()
    {
      int count = this.mItemGroupList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemGroupList[index].CheckIfNeedUpdateItemPos();
    }

    public float GetItemAbsPosByItemIndex(int itemIndex)
    {
      if (itemIndex < 0 || itemIndex >= this.mItemIndexDataList.Count)
        return -1f;
      ItemIndexData mItemIndexData = this.mItemIndexDataList[itemIndex];
      return this.mItemGroupList[mItemIndexData.mGroupIndex].GetItemPos(mItemIndexData.mIndexInGroup);
    }

    public LoopStaggeredGridViewItem GetNewItemByGroupAndIndex(
      int groupIndex,
      int indexInGroup)
    {
      if (indexInGroup < 0)
        return (LoopStaggeredGridViewItem) null;
      if (this.mItemTotalCount == 0)
        return (LoopStaggeredGridViewItem) null;
      List<int> itemIndexMap = this.mItemGroupList[groupIndex].ItemIndexMap;
      int count1 = itemIndexMap.Count;
      if (count1 > indexInGroup)
      {
        int num = itemIndexMap[indexInGroup];
        LoopStaggeredGridViewItem staggeredGridViewItem = this.mOnGetItemByItemIndex(this, num);
        if (Object.op_Equality((Object) staggeredGridViewItem, (Object) null))
          return (LoopStaggeredGridViewItem) null;
        staggeredGridViewItem.StartPosOffset = this.mLayoutParam.mCustomColumnOrRowOffsetArray[groupIndex];
        staggeredGridViewItem.ItemIndexInGroup = indexInGroup;
        staggeredGridViewItem.ItemIndex = num;
        staggeredGridViewItem.ItemCreatedCheckFrameCount = this.mListUpdateCheckFrameCount;
        return staggeredGridViewItem;
      }
      if (count1 != indexInGroup)
        return (LoopStaggeredGridViewItem) null;
      int count2 = this.mItemIndexDataList.Count;
      if (count2 >= this.mItemTotalCount)
        return (LoopStaggeredGridViewItem) null;
      int num1 = count2;
      LoopStaggeredGridViewItem staggeredGridViewItem1 = this.mOnGetItemByItemIndex(this, num1);
      if (Object.op_Equality((Object) staggeredGridViewItem1, (Object) null))
        return (LoopStaggeredGridViewItem) null;
      itemIndexMap.Add(num1);
      this.mItemIndexDataList.Add(new ItemIndexData()
      {
        mGroupIndex = groupIndex,
        mIndexInGroup = indexInGroup
      });
      staggeredGridViewItem1.StartPosOffset = this.mLayoutParam.mCustomColumnOrRowOffsetArray[groupIndex];
      staggeredGridViewItem1.ItemIndexInGroup = indexInGroup;
      staggeredGridViewItem1.ItemIndex = num1;
      staggeredGridViewItem1.ItemCreatedCheckFrameCount = this.mListUpdateCheckFrameCount;
      return staggeredGridViewItem1;
    }

    private int GetCurShouldAddNewItemGroupIndex()
    {
      float num1 = float.MaxValue;
      int count = this.mItemGroupList.Count;
      int num2 = 0;
      for (int index = 0; index < count; ++index)
      {
        float shownItemPosMaxValue = this.mItemGroupList[index].GetShownItemPosMaxValue();
        if ((double) shownItemPosMaxValue < (double) num1)
        {
          num1 = shownItemPosMaxValue;
          num2 = index;
        }
      }
      return num2;
    }

    public void UpdateListViewWithDefault()
    {
      this.UpdateListView(this.mDistanceForRecycle0, this.mDistanceForRecycle1, this.mDistanceForNew0, this.mDistanceForNew1);
      this.UpdateContentSize();
    }

    private void Update()
    {
      if (!this.mListViewInited)
        return;
      this.UpdateListViewWithDefault();
      this.ClearAllTmpRecycledItem();
      this.mLastFrameContainerPos = this.mContainerTrans.get_anchoredPosition3D();
    }

    public void UpdateListView(
      float distanceForRecycle0,
      float distanceForRecycle1,
      float distanceForNew0,
      float distanceForNew1)
    {
      ++this.mListUpdateCheckFrameCount;
      bool flag = true;
      int num1 = 0;
      int num2 = 9999;
      int count = this.mItemGroupList.Count;
      for (int index = 0; index < count; ++index)
        this.mItemGroupList[index].UpdateListViewPart1(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1);
      for (; flag; flag = this.mItemGroupList[this.GetCurShouldAddNewItemGroupIndex()].UpdateListViewPart2(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1))
      {
        ++num1;
        if (num1 >= num2)
        {
          Debug.LogError((object) ("UpdateListView while loop " + (object) num1 + " times! something is wrong!"));
          break;
        }
      }
    }

    public float GetContentSize()
    {
      if (this.mIsVertList)
      {
        Rect rect = this.mContainerTrans.get_rect();
        return ((Rect) ref rect).get_height();
      }
      Rect rect1 = this.mContainerTrans.get_rect();
      return ((Rect) ref rect1).get_width();
    }

    public void UpdateContentSize()
    {
      int count = this.mItemGroupList.Count;
      float num = 0.0f;
      for (int index = 0; index < count; ++index)
      {
        float contentPanelSize = this.mItemGroupList[index].GetContentPanelSize();
        if ((double) contentPanelSize > (double) num)
          num = contentPanelSize;
      }
      if (this.mIsVertList)
      {
        Rect rect = this.mContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_height() == (double) num)
          return;
        this.mContainerTrans.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, num);
      }
      else
      {
        Rect rect = this.mContainerTrans.get_rect();
        if ((double) ((Rect) ref rect).get_width() == (double) num)
          return;
        this.mContainerTrans.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, num);
      }
    }
  }
}
