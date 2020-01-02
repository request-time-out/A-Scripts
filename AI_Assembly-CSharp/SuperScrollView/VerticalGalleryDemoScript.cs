﻿// Decompiled with JetBrains decompiler
// Type: SuperScrollView.VerticalGalleryDemoScript
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
  public class VerticalGalleryDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;

    public VerticalGalleryDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
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
      if (index < 0 || index >= DataSourceMgr.Get.TotalItemCount)
        return (LoopListViewItem2) null;
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(index);
      if (itemDataByIndex == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem2 component = (ListItem2) ((Component) loopListViewItem2).GetComponent<ListItem2>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, index);
      return loopListViewItem2;
    }

    private void LateUpdate()
    {
      this.mLoopListView.UpdateAllShownItemSnapData();
      int shownItemCount = this.mLoopListView.ShownItemCount;
      for (int index = 0; index < shownItemCount; ++index)
      {
        LoopListViewItem2 shownItemByIndex = this.mLoopListView.GetShownItemByIndex(index);
        ListItem2 component = (ListItem2) ((Component) shownItemByIndex).GetComponent<ListItem2>();
        float num = Mathf.Clamp((float) (1.0 - (double) Mathf.Abs(shownItemByIndex.DistanceWithViewPortSnapCenter) / 700.0), 0.4f, 1f);
        ((CanvasGroup) component.mContentRootObj.GetComponent<CanvasGroup>()).set_alpha(num);
        component.mContentRootObj.get_transform().set_localScale(new Vector3(num, num, 1f));
      }
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      int itemIndex = result - 2;
      if (itemIndex < 0)
        itemIndex = 0;
      this.mLoopListView.MovePanelToItemIndex(itemIndex, 0.0f);
      this.mLoopListView.FinishSnapImmediately();
    }

    private void OnAddItemBtnClicked()
    {
      if (this.mLoopListView.ItemTotalCount < 0)
        return;
      int result = 0;
      if (!int.TryParse(this.mAddItemInput.get_text(), out result))
        return;
      result = this.mLoopListView.ItemTotalCount + result;
      if (result < 0 || result > DataSourceMgr.Get.TotalItemCount)
        return;
      this.mLoopListView.SetListItemCount(result, false);
    }

    private void OnSetItemCountBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mSetCountInput.get_text(), out result) || result < 0 || result > DataSourceMgr.Get.TotalItemCount)
        return;
      this.mLoopListView.SetListItemCount(result, false);
    }
  }
}
