// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ResponsiveGridViewDemoScript2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ResponsiveGridViewDemoScript2 : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private LoadingTipStatus mLoadingTipStatus1;
    private LoadingTipStatus mLoadingTipStatus2;
    private float mDataLoadedTipShowLeftTime;
    private float mLoadingTipItemHeight1;
    private float mLoadingTipItemHeight2;
    private int mLoadMoreCount;
    private Button mScrollToButton;
    private InputField mScrollToInput;
    private Button mBackButton;
    private int mItemCountPerRow;
    public DragChangSizeScript mDragChangSizeScript;

    public ResponsiveGridViewDemoScript2()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(this.GetMaxRowCount() + 2, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mDragChangSizeScript.mOnDragEndAction = new Action(this.OnViewPortSizeChanged);
      this.mLoopListView.mOnDragingAction = new Action(this.OnDraging);
      this.mLoopListView.mOnEndDragAction = new Action(this.OnEndDrag);
      this.OnViewPortSizeChanged();
      this.mScrollToButton = (Button) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
      this.mScrollToInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
      // ISSUE: method pointer
      ((UnityEvent) this.mScrollToButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnJumpBtnClicked)));
      this.mBackButton = (Button) GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private void UpdateItemPrefab()
    {
      GameObject mItemPrefab = this.mLoopListView.GetItemPrefabConfData("ItemPrefab2").mItemPrefab;
      RectTransform component1 = (RectTransform) mItemPrefab.GetComponent<RectTransform>();
      ListItem6 component2 = (ListItem6) mItemPrefab.GetComponent<ListItem6>();
      float viewPortWidth = this.mLoopListView.ViewPortWidth;
      int count = component2.mItemList.Count;
      GameObject gameObject1 = ((Component) component2.mItemList[0]).get_gameObject();
      Rect rect = ((RectTransform) gameObject1.GetComponent<RectTransform>()).get_rect();
      float width = ((Rect) ref rect).get_width();
      int num1 = Mathf.FloorToInt(viewPortWidth / width);
      if (num1 == 0)
        num1 = 1;
      this.mItemCountPerRow = num1;
      float num2 = (viewPortWidth - width * (float) num1) / (float) (num1 + 1);
      if ((double) num2 < 0.0)
        num2 = 0.0f;
      component1.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, viewPortWidth);
      if (num1 > count)
      {
        int num3 = num1 - count;
        for (int index = 0; index < num3; ++index)
        {
          GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, Vector3.get_zero(), Quaternion.get_identity(), (Transform) component1);
          RectTransform component3 = (RectTransform) gameObject2.GetComponent<RectTransform>();
          ((Transform) component3).set_localScale(Vector3.get_one());
          component3.set_anchoredPosition3D(Vector3.get_zero());
          ((Transform) component3).set_rotation(Quaternion.get_identity());
          ListItem5 component4 = (ListItem5) gameObject2.GetComponent<ListItem5>();
          component2.mItemList.Add(component4);
        }
      }
      else if (num1 < count)
      {
        int num3 = count - num1;
        for (int index = 0; index < num3; ++index)
        {
          ListItem5 mItem = component2.mItemList[component2.mItemList.Count - 1];
          component2.mItemList.RemoveAt(component2.mItemList.Count - 1);
          Object.DestroyImmediate((Object) ((Component) mItem).get_gameObject());
        }
      }
      float num4 = num2;
      for (int index = 0; index < component2.mItemList.Count; ++index)
      {
        ((RectTransform) ((Component) component2.mItemList[index]).get_gameObject().GetComponent<RectTransform>()).set_anchoredPosition3D(new Vector3(num4, 0.0f, 0.0f));
        num4 = num4 + width + num2;
      }
      this.mLoopListView.OnItemPrefabChanged("ItemPrefab2");
    }

    private void OnViewPortSizeChanged()
    {
      this.UpdateItemPrefab();
      this.mLoopListView.SetListItemCount(this.GetMaxRowCount() + 2, false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private int GetMaxRowCount()
    {
      int num = DataSourceMgr.Get.TotalItemCount / this.mItemCountPerRow;
      if (DataSourceMgr.Get.TotalItemCount % this.mItemCountPerRow > 0)
        ++num;
      return num;
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int row)
    {
      if (row < 0)
        return (LoopListViewItem2) null;
      if (row == 0)
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab0");
        this.UpdateLoadingTip1(loopListViewItem2);
        return loopListViewItem2;
      }
      if (row == this.GetMaxRowCount() + 1)
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
        this.UpdateLoadingTip2(loopListViewItem2);
        return loopListViewItem2;
      }
      int num1 = row - 1;
      LoopListViewItem2 loopListViewItem2_1 = listView.NewListViewItem("ItemPrefab2");
      ListItem6 component = (ListItem6) ((Component) loopListViewItem2_1).GetComponent<ListItem6>();
      if (!loopListViewItem2_1.IsInitHandlerCalled)
      {
        loopListViewItem2_1.IsInitHandlerCalled = true;
        component.Init();
      }
      for (int index = 0; index < this.mItemCountPerRow; ++index)
      {
        int num2 = num1 * this.mItemCountPerRow + index;
        if (num2 >= DataSourceMgr.Get.TotalItemCount)
        {
          ((Component) component.mItemList[index]).get_gameObject().SetActive(false);
        }
        else
        {
          ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num2);
          if (itemDataByIndex != null)
          {
            ((Component) component.mItemList[index]).get_gameObject().SetActive(true);
            component.mItemList[index].SetItemData(itemDataByIndex, num2);
          }
          else
            ((Component) component.mItemList[index]).get_gameObject().SetActive(false);
        }
      }
      return loopListViewItem2_1;
    }

    private void UpdateLoadingTip1(LoopListViewItem2 item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, this.mLoopListView.ViewPortWidth);
      ListItem17 component = (ListItem17) ((Component) item).GetComponent<ListItem17>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      if (this.mLoadingTipStatus1 == LoadingTipStatus.None)
      {
        component.mRoot1.SetActive(false);
        component.mRoot.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 0.0f);
      }
      else if (this.mLoadingTipStatus1 == LoadingTipStatus.WaitContinureDrag)
      {
        component.mRoot1.SetActive(true);
        component.mRoot.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 0.0f);
      }
      else if (this.mLoadingTipStatus1 == LoadingTipStatus.WaitRelease)
      {
        component.mRoot1.SetActive(false);
        component.mRoot.SetActive(true);
        component.mText.set_text("Release to Refresh");
        component.mArrow.SetActive(true);
        component.mWaitingIcon.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight1);
      }
      else if (this.mLoadingTipStatus1 == LoadingTipStatus.WaitLoad)
      {
        component.mRoot1.SetActive(false);
        component.mRoot.SetActive(true);
        component.mArrow.SetActive(false);
        component.mWaitingIcon.SetActive(true);
        component.mText.set_text("Loading ...");
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight1);
      }
      else
      {
        if (this.mLoadingTipStatus1 != LoadingTipStatus.Loaded)
          return;
        component.mRoot1.SetActive(false);
        component.mRoot.SetActive(true);
        component.mArrow.SetActive(false);
        component.mWaitingIcon.SetActive(false);
        component.mText.set_text("Refreshed Success");
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight1);
      }
    }

    private void OnDraging()
    {
      this.OnDraging1();
      this.OnDraging2();
    }

    private void OnEndDrag()
    {
      this.OnEndDrag1();
      this.OnEndDrag2();
    }

    private void OnDraging1()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus1 != LoadingTipStatus.None && this.mLoadingTipStatus1 != LoadingTipStatus.WaitRelease && this.mLoadingTipStatus1 != LoadingTipStatus.WaitContinureDrag)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      Vector3 anchoredPosition3D = this.mLoopListView.ScrollRect.get_content().get_anchoredPosition3D();
      if (anchoredPosition3D.y >= 0.0)
      {
        if (this.mLoadingTipStatus1 != LoadingTipStatus.WaitContinureDrag)
          return;
        this.mLoadingTipStatus1 = LoadingTipStatus.None;
        this.UpdateLoadingTip1(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, 0.0f, 0.0f));
      }
      else if (anchoredPosition3D.y < 0.0 && anchoredPosition3D.y > -(double) this.mLoadingTipItemHeight1)
      {
        if (this.mLoadingTipStatus1 != LoadingTipStatus.None && this.mLoadingTipStatus1 != LoadingTipStatus.WaitRelease)
          return;
        this.mLoadingTipStatus1 = LoadingTipStatus.WaitContinureDrag;
        this.UpdateLoadingTip1(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, 0.0f, 0.0f));
      }
      else
      {
        if (anchoredPosition3D.y > -(double) this.mLoadingTipItemHeight1 || this.mLoadingTipStatus1 != LoadingTipStatus.WaitContinureDrag)
          return;
        this.mLoadingTipStatus1 = LoadingTipStatus.WaitRelease;
        this.UpdateLoadingTip1(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mLoadingTipItemHeight1, 0.0f));
      }
    }

    private void OnEndDrag1()
    {
      if (this.mLoopListView.ShownItemCount == 0)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.mLoopListView.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
      if (this.mLoadingTipStatus1 != LoadingTipStatus.WaitRelease)
        return;
      this.mLoadingTipStatus1 = LoadingTipStatus.WaitLoad;
      this.UpdateLoadingTip1(shownItemByItemIndex);
      DataSourceMgr.Get.RequestRefreshDataList(new Action(this.OnDataSourceRefreshFinished));
    }

    private void OnDataSourceRefreshFinished()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus1 != LoadingTipStatus.WaitLoad)
        return;
      this.mLoadingTipStatus1 = LoadingTipStatus.Loaded;
      this.mDataLoadedTipShowLeftTime = 0.7f;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.UpdateLoadingTip1(shownItemByItemIndex);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void Update()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus1 != LoadingTipStatus.Loaded)
        return;
      this.mDataLoadedTipShowLeftTime -= Time.get_deltaTime();
      if ((double) this.mDataLoadedTipShowLeftTime > 0.0)
        return;
      this.mLoadingTipStatus1 = LoadingTipStatus.None;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.UpdateLoadingTip1(shownItemByItemIndex);
      shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, -this.mLoadingTipItemHeight1, 0.0f));
      this.mLoopListView.OnItemSizeChanged(0);
    }

    private void UpdateLoadingTip2(LoopListViewItem2 item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, this.mLoopListView.ViewPortWidth);
      ListItem0 component = (ListItem0) ((Component) item).GetComponent<ListItem0>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      if (this.mLoadingTipStatus2 == LoadingTipStatus.None)
      {
        component.mRoot.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 0.0f);
      }
      else if (this.mLoadingTipStatus2 == LoadingTipStatus.WaitRelease)
      {
        component.mRoot.SetActive(true);
        component.mText.set_text("Release to Load More");
        component.mArrow.SetActive(true);
        component.mWaitingIcon.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight2);
      }
      else
      {
        if (this.mLoadingTipStatus2 != LoadingTipStatus.WaitLoad)
          return;
        component.mRoot.SetActive(true);
        component.mArrow.SetActive(false);
        component.mWaitingIcon.SetActive(true);
        component.mText.set_text("Loading ...");
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight2);
      }
    }

    private void OnDraging2()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus2 != LoadingTipStatus.None && this.mLoadingTipStatus2 != LoadingTipStatus.WaitRelease)
        return;
      LoopListViewItem2 shownItemByItemIndex1 = this.mLoopListView.GetShownItemByItemIndex(this.GetMaxRowCount() + 1);
      if (Object.op_Equality((Object) shownItemByItemIndex1, (Object) null))
        return;
      LoopListViewItem2 shownItemByItemIndex2 = this.mLoopListView.GetShownItemByItemIndex(this.GetMaxRowCount());
      if (Object.op_Equality((Object) shownItemByItemIndex2, (Object) null))
        return;
      if ((double) (float) this.mLoopListView.GetItemCornerPosInViewPort(shownItemByItemIndex2, ItemCornerEnum.LeftBottom).y + (double) this.mLoopListView.ViewPortSize >= (double) this.mLoadingTipItemHeight2)
      {
        if (this.mLoadingTipStatus2 != LoadingTipStatus.None)
          return;
        this.mLoadingTipStatus2 = LoadingTipStatus.WaitRelease;
        this.UpdateLoadingTip2(shownItemByItemIndex1);
      }
      else
      {
        if (this.mLoadingTipStatus2 != LoadingTipStatus.WaitRelease)
          return;
        this.mLoadingTipStatus2 = LoadingTipStatus.None;
        this.UpdateLoadingTip2(shownItemByItemIndex1);
      }
    }

    private void OnEndDrag2()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus2 != LoadingTipStatus.None && this.mLoadingTipStatus2 != LoadingTipStatus.WaitRelease)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(this.GetMaxRowCount() + 1);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.mLoopListView.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
      if (this.mLoadingTipStatus2 != LoadingTipStatus.WaitRelease)
        return;
      this.mLoadingTipStatus2 = LoadingTipStatus.WaitLoad;
      this.UpdateLoadingTip2(shownItemByItemIndex);
      DataSourceMgr.Get.RequestLoadMoreDataList(this.mLoadMoreCount, new Action(this.OnDataSourceLoadMoreFinished));
    }

    private void OnDataSourceLoadMoreFinished()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus2 != LoadingTipStatus.WaitLoad)
        return;
      this.mLoadingTipStatus2 = LoadingTipStatus.None;
      this.mLoopListView.SetListItemCount(this.GetMaxRowCount() + 2, false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      if (result < 0)
        result = 0;
      int num1 = result + 1;
      int num2 = num1 / this.mItemCountPerRow;
      if (num1 % this.mItemCountPerRow > 0)
        ++num2;
      if (num2 > 0)
        --num2;
      this.mLoopListView.MovePanelToItemIndex(num2 + 1, 0.0f);
    }
  }
}
