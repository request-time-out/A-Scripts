// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridViewDeleteItemDemoScript2
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
  public class GridViewDeleteItemDemoScript2 : MonoBehaviour
  {
    public LoopGridView mLoopGridView;
    public Button mSelectAllButton;
    public Button mCancelAllButton;
    public Button mDeleteButton;
    public Button mBackButton;

    public GridViewDeleteItemDemoScript2()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopGridView.InitGridView(DataSourceMgr.Get.TotalItemCount, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByRowColumn), (LoopGridViewSettingParam) null, (LoopGridViewInitParam) null);
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mSelectAllButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnSelectAllBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mCancelAllButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnCancelAllBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mDeleteButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnDeleteBtnClicked)));
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private LoopGridViewItem OnGetItemByRowColumn(
      LoopGridView gridView,
      int itemIndex,
      int row,
      int column)
    {
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(itemIndex);
      if (itemDataByIndex == null)
        return (LoopGridViewItem) null;
      LoopGridViewItem loopGridViewItem = gridView.NewListViewItem("ItemPrefab0");
      ListItem19 component = (ListItem19) ((Component) loopGridViewItem).GetComponent<ListItem19>();
      if (!loopGridViewItem.IsInitHandlerCalled)
      {
        loopGridViewItem.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, itemIndex, row, column);
      return loopGridViewItem;
    }

    private void OnSelectAllBtnClicked()
    {
      DataSourceMgr.Get.CheckAllItem();
      this.mLoopGridView.RefreshAllShownItem();
    }

    private void OnCancelAllBtnClicked()
    {
      DataSourceMgr.Get.UnCheckAllItem();
      this.mLoopGridView.RefreshAllShownItem();
    }

    private void OnDeleteBtnClicked()
    {
      if (!DataSourceMgr.Get.DeleteAllCheckedItem())
        return;
      this.mLoopGridView.SetListItemCount(DataSourceMgr.Get.TotalItemCount, false);
      this.mLoopGridView.RefreshAllShownItem();
    }
  }
}
