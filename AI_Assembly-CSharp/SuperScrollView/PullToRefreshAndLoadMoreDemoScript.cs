// Decompiled with JetBrains decompiler
// Type: SuperScrollView.PullToRefreshAndLoadMoreDemoScript
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
  public class PullToRefreshAndLoadMoreDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private LoadingTipStatus mLoadingTipStatus1;
    private LoadingTipStatus mLoadingTipStatus2;
    private float mDataLoadedTipShowLeftTime;
    private float mLoadingTipItemHeight1;
    private float mLoadingTipItemHeight2;
    private int mLoadMoreCount;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;

    public PullToRefreshAndLoadMoreDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount + 2, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mLoopListView.mOnDragingAction = new Action(this.OnDraging);
      this.mLoopListView.mOnEndDragAction = new Action(this.OnEndDrag);
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

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0)
        return (LoopListViewItem2) null;
      if (index == 0)
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab0");
        this.UpdateLoadingTip1(loopListViewItem2);
        return loopListViewItem2;
      }
      if (index == DataSourceMgr.Get.TotalItemCount + 1)
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
        this.UpdateLoadingTip2(loopListViewItem2);
        return loopListViewItem2;
      }
      int num = index - 1;
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num);
      if (itemDataByIndex == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2_1 = listView.NewListViewItem("ItemPrefab2");
      ListItem2 component = (ListItem2) ((Component) loopListViewItem2_1).GetComponent<ListItem2>();
      if (!loopListViewItem2_1.IsInitHandlerCalled)
      {
        loopListViewItem2_1.IsInitHandlerCalled = true;
        component.Init();
      }
      if (index == DataSourceMgr.Get.TotalItemCount)
        loopListViewItem2_1.Padding = 0.0f;
      component.SetItemData(itemDataByIndex, num);
      return loopListViewItem2_1;
    }

    private void UpdateLoadingTip1(LoopListViewItem2 item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      ListItem0 component = (ListItem0) ((Component) item).GetComponent<ListItem0>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      if (this.mLoadingTipStatus1 == LoadingTipStatus.None)
      {
        component.mRoot.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 0.0f);
      }
      else if (this.mLoadingTipStatus1 == LoadingTipStatus.WaitRelease)
      {
        component.mRoot.SetActive(true);
        component.mText.set_text("Release to Refresh");
        component.mArrow.SetActive(true);
        component.mWaitingIcon.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight1);
      }
      else if (this.mLoadingTipStatus1 == LoadingTipStatus.WaitLoad)
      {
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
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus1 != LoadingTipStatus.None && this.mLoadingTipStatus1 != LoadingTipStatus.WaitRelease)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      if (this.mLoopListView.ScrollRect.get_content().get_anchoredPosition3D().y < -(double) this.mLoadingTipItemHeight1)
      {
        if (this.mLoadingTipStatus1 != LoadingTipStatus.None)
          return;
        this.mLoadingTipStatus1 = LoadingTipStatus.WaitRelease;
        this.UpdateLoadingTip1(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mLoadingTipItemHeight1, 0.0f));
      }
      else
      {
        if (this.mLoadingTipStatus1 != LoadingTipStatus.WaitRelease)
          return;
        this.mLoadingTipStatus1 = LoadingTipStatus.None;
        this.UpdateLoadingTip1(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, 0.0f, 0.0f));
      }
    }

    private void OnEndDrag1()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus1 != LoadingTipStatus.None && this.mLoadingTipStatus1 != LoadingTipStatus.WaitRelease)
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
      LoopListViewItem2 shownItemByItemIndex1 = this.mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount + 1);
      if (Object.op_Equality((Object) shownItemByItemIndex1, (Object) null))
        return;
      LoopListViewItem2 shownItemByItemIndex2 = this.mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount);
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
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount + 1);
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
      this.mLoopListView.SetListItemCount(DataSourceMgr.Get.TotalItemCount + 2, false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result) || result < 0)
        return;
      this.mLoopListView.MovePanelToItemIndex(result + 1, 0.0f);
    }
  }
}
