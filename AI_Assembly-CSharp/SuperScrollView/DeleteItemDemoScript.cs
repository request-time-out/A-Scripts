// Decompiled with JetBrains decompiler
// Type: SuperScrollView.DeleteItemDemoScript
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
  public class DeleteItemDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    public Button mSelectAllButton;
    public Button mCancelAllButton;
    public Button mDeleteButton;
    public Button mBackButton;

    public DeleteItemDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      // ISSUE: method pointer
      ((UnityEvent) this.mSelectAllButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnSelectAllBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mCancelAllButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnCancelAllBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mDeleteButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnDeleteBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0 || index >= DataSourceMgr.Get.TotalItemCount)
        return (LoopListViewItem2) null;
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(index);
      if (itemDataByIndex == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem3 component = (ListItem3) ((Component) loopListViewItem2).GetComponent<ListItem3>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, index);
      return loopListViewItem2;
    }

    private void OnSelectAllBtnClicked()
    {
      DataSourceMgr.Get.CheckAllItem();
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnCancelAllBtnClicked()
    {
      DataSourceMgr.Get.UnCheckAllItem();
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnDeleteBtnClicked()
    {
      if (!DataSourceMgr.Get.DeleteAllCheckedItem())
        return;
      this.mLoopListView.SetListItemCount(DataSourceMgr.Get.TotalItemCount, false);
      this.mLoopListView.RefreshAllShownItem();
    }
  }
}
