// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridViewDeleteItemDemoScript
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
  public class GridViewDeleteItemDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    public Button mSelectAllButton;
    public Button mCancelAllButton;
    public Button mDeleteButton;
    public Button mBackButton;
    private const int mItemCountPerRow = 3;
    private int mListItemTotalCount;

    public GridViewDeleteItemDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
      int itemTotalCount = this.mListItemTotalCount / 3;
      if (this.mListItemTotalCount % 3 > 0)
        ++itemTotalCount;
      this.mLoopListView.InitListView(itemTotalCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
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

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem10 component = (ListItem10) ((Component) loopListViewItem2).GetComponent<ListItem10>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int num = index * 3 + index1;
        if (num >= this.mListItemTotalCount)
        {
          ((Component) component.mItemList[index1]).get_gameObject().SetActive(false);
        }
        else
        {
          ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num);
          if (itemDataByIndex != null)
          {
            ((Component) component.mItemList[index1]).get_gameObject().SetActive(true);
            component.mItemList[index1].SetItemData(itemDataByIndex, num);
          }
          else
            ((Component) component.mItemList[index1]).get_gameObject().SetActive(false);
        }
      }
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
      this.SetListItemTotalCount(DataSourceMgr.Get.TotalItemCount);
    }

    private void SetListItemTotalCount(int count)
    {
      this.mListItemTotalCount = count;
      if (this.mListItemTotalCount < 0)
        this.mListItemTotalCount = 0;
      if (this.mListItemTotalCount > DataSourceMgr.Get.TotalItemCount)
        this.mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
      int itemCount = this.mListItemTotalCount / 3;
      if (this.mListItemTotalCount % 3 > 0)
        ++itemCount;
      this.mLoopListView.SetListItemCount(itemCount, false);
      this.mLoopListView.RefreshAllShownItem();
    }
  }
}
