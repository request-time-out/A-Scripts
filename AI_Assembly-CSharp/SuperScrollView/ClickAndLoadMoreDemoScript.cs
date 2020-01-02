// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ClickAndLoadMoreDemoScript
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
  public class ClickAndLoadMoreDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private LoadingTipStatus mLoadingTipStatus;
    private int mLoadMoreCount;
    private Button mScrollToButton;
    private InputField mScrollToInput;
    private Button mBackButton;

    public ClickAndLoadMoreDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount + 1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
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
      if (index == DataSourceMgr.Get.TotalItemCount)
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab0");
        if (!loopListViewItem2.IsInitHandlerCalled)
        {
          loopListViewItem2.IsInitHandlerCalled = true;
          // ISSUE: method pointer
          ((UnityEvent) ((ListItem11) ((Component) loopListViewItem2).GetComponent<ListItem11>()).mRootButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnLoadMoreBtnClicked)));
        }
        this.UpdateLoadingTip(loopListViewItem2);
        return loopListViewItem2;
      }
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(index);
      if (itemDataByIndex == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2_1 = listView.NewListViewItem("ItemPrefab1");
      ListItem2 component = (ListItem2) ((Component) loopListViewItem2_1).GetComponent<ListItem2>();
      if (!loopListViewItem2_1.IsInitHandlerCalled)
      {
        loopListViewItem2_1.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, index);
      return loopListViewItem2_1;
    }

    private void UpdateLoadingTip(LoopListViewItem2 item)
    {
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      ListItem11 component = (ListItem11) ((Component) item).GetComponent<ListItem11>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      if (this.mLoadingTipStatus == LoadingTipStatus.None)
      {
        component.mText.set_text("Click to Load More");
        component.mWaitingIcon.SetActive(false);
      }
      else
      {
        if (this.mLoadingTipStatus != LoadingTipStatus.WaitLoad)
          return;
        component.mWaitingIcon.SetActive(true);
        component.mText.set_text("Loading ...");
      }
    }

    private void OnLoadMoreBtnClicked()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus != LoadingTipStatus.None)
        return;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        return;
      this.mLoadingTipStatus = LoadingTipStatus.WaitLoad;
      this.UpdateLoadingTip(shownItemByItemIndex);
      DataSourceMgr.Get.RequestLoadMoreDataList(this.mLoadMoreCount, new Action(this.OnDataSourceLoadMoreFinished));
    }

    private void OnDataSourceLoadMoreFinished()
    {
      if (this.mLoopListView.ShownItemCount == 0 || this.mLoadingTipStatus != LoadingTipStatus.WaitLoad)
        return;
      this.mLoadingTipStatus = LoadingTipStatus.None;
      this.mLoopListView.SetListItemCount(DataSourceMgr.Get.TotalItemCount + 1, false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result) || result < 0)
        return;
      this.mLoopListView.MovePanelToItemIndex(result, 0.0f);
    }
  }
}
