// Decompiled with JetBrains decompiler
// Type: SuperScrollView.PullToRefreshDemoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class PullToRefreshDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private LoadingTipStatus mLoadingTipStatus;
    private float mDataLoadedTipShowLeftTime;
    private float mLoadingTipItemHeight;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;

    public PullToRefreshDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount + 1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mLoopListView.mOnBeginDragAction = new Action(this.OnBeginDrag);
      this.mLoopListView.mOnDragingAction = new Action(this.OnDraging);
      this.mLoopListView.mOnEndDragAction = new Action(this.OnEndDrag);
      this.mSetCountButton = (Button) GameObject.Find("ButtonPanel/buttonGroup1/SetCountButton").GetComponent<Button>();
      this.mScrollToButton = (Button) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
      this.mAddItemButton = (Button) GameObject.Find("ButtonPanel/buttonGroup3/AddItemButton").GetComponent<Button>();
      this.mSetCountInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup1/SetCountInputField").GetComponent<InputField>();
      this.mScrollToInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
      this.mAddItemInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup3/AddItemInputField").GetComponent<InputField>();
      // ISSUE: method pointer
      ((UnityEvent) this.mScrollToButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnJumpBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mAddItemButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnAddItemBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mSetCountButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnSetItemCountBtnClicked)));
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
      if (index < 0 || index > DataSourceMgr.Get.TotalItemCount)
        return (LoopListViewItem2) null;
      if (index == 0)
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab0");
        this.UpdateLoadingTip(loopListViewItem2);
        return loopListViewItem2;
      }
      int num = index - 1;
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num);
      if (itemDataByIndex == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2_1 = listView.NewListViewItem("ItemPrefab1");
      ListItem2 component = (ListItem2) ((Component) loopListViewItem2_1).GetComponent<ListItem2>();
      if (!loopListViewItem2_1.IsInitHandlerCalled)
      {
        loopListViewItem2_1.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, num);
      return loopListViewItem2_1;
    }

    private void UpdateLoadingTip(LoopListViewItem2 item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      ListItem0 component = (ListItem0) ((Component) item).GetComponent<ListItem0>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      if (this.mLoadingTipStatus == LoadingTipStatus.None)
      {
        component.mRoot.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 0.0f);
      }
      else if (this.mLoadingTipStatus == LoadingTipStatus.WaitRelease)
      {
        component.mRoot.SetActive(true);
        component.mText.set_text("Release to Refresh");
        component.mArrow.SetActive(true);
        component.mWaitingIcon.SetActive(false);
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight);
      }
      else if (this.mLoadingTipStatus == LoadingTipStatus.WaitLoad)
      {
        component.mRoot.SetActive(true);
        component.mArrow.SetActive(false);
        component.mWaitingIcon.SetActive(true);
        component.mText.set_text("Loading ...");
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight);
      }
      else
      {
        if (this.mLoadingTipStatus != LoadingTipStatus.Loaded)
          return;
        component.mRoot.SetActive(true);
        component.mArrow.SetActive(false);
        component.mWaitingIcon.SetActive(false);
        component.mText.set_text("Refreshed Success");
        item.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.mLoadingTipItemHeight);
      }
    }

    private void OnBeginDrag()
    {
    }

    private void OnDraging()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus != LoadingTipStatus.None && this.mLoadingTipStatus != LoadingTipStatus.WaitRelease)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      if (this.mLoopListView.ScrollRect.get_content().get_anchoredPosition3D().y < -(double) this.mLoadingTipItemHeight)
      {
        if (this.mLoadingTipStatus != LoadingTipStatus.None)
          return;
        this.mLoadingTipStatus = LoadingTipStatus.WaitRelease;
        this.UpdateLoadingTip(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, this.mLoadingTipItemHeight, 0.0f));
      }
      else
      {
        if (this.mLoadingTipStatus != LoadingTipStatus.WaitRelease)
          return;
        this.mLoadingTipStatus = LoadingTipStatus.None;
        this.UpdateLoadingTip(shownItemByItemIndex);
        shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, 0.0f, 0.0f));
      }
    }

    private void OnEndDrag()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus != LoadingTipStatus.None && this.mLoadingTipStatus != LoadingTipStatus.WaitRelease)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.mLoopListView.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
      if (this.mLoadingTipStatus != LoadingTipStatus.WaitRelease)
        return;
      this.mLoadingTipStatus = LoadingTipStatus.WaitLoad;
      this.UpdateLoadingTip(shownItemByItemIndex);
      DataSourceMgr.Get.RequestRefreshDataList(new Action(this.OnDataSourceRefreshFinished));
    }

    private void OnDataSourceRefreshFinished()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus != LoadingTipStatus.WaitLoad)
        return;
      this.mLoadingTipStatus = LoadingTipStatus.Loaded;
      this.mDataLoadedTipShowLeftTime = 0.7f;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.UpdateLoadingTip(shownItemByItemIndex);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void Update()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus != LoadingTipStatus.Loaded)
        return;
      this.mDataLoadedTipShowLeftTime -= Time.get_deltaTime();
      if ((double) this.mDataLoadedTipShowLeftTime > 0.0)
        return;
      this.mLoadingTipStatus = LoadingTipStatus.None;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(0);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.UpdateLoadingTip(shownItemByItemIndex);
      shownItemByItemIndex.CachedRectTransform.set_anchoredPosition3D(new Vector3(0.0f, -this.mLoadingTipItemHeight, 0.0f));
      this.mLoopListView.OnItemSizeChanged(0);
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result) || result < 0)
        return;
      this.mLoopListView.MovePanelToItemIndex(result + 1, 0.0f);
    }

    private void OnAddItemBtnClicked()
    {
      if (this.mLoopListView.ItemTotalCount < 0)
        return;
      int result = 0;
      if (!int.TryParse(this.mAddItemInput.get_text(), out result))
        return;
      result = this.mLoopListView.ItemTotalCount + result;
      if (result < 0 || result > DataSourceMgr.Get.TotalItemCount + 1)
        return;
      this.mLoopListView.SetListItemCount(result, false);
    }

    private void OnSetItemCountBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mSetCountInput.get_text(), out result) || result < 0 || result > DataSourceMgr.Get.TotalItemCount)
        return;
      this.mLoopListView.SetListItemCount(result + 1, false);
    }
  }
}
