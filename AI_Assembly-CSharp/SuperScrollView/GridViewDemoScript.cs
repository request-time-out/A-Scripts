// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridViewDemoScript
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
  public class GridViewDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;
    private const int mItemCountPerRow = 3;
    private int mListItemTotalCount;

    public GridViewDemoScript()
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

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem6 component = (ListItem6) ((Component) loopListViewItem2).GetComponent<ListItem6>();
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

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      if (result < 0)
        result = 0;
      int num = result + 1;
      int itemIndex = num / 3;
      if (num % 3 > 0)
        ++itemIndex;
      if (itemIndex > 0)
        --itemIndex;
      this.mLoopListView.MovePanelToItemIndex(itemIndex, 0.0f);
    }

    private void OnAddItemBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mAddItemInput.get_text(), out result))
        return;
      this.SetListItemTotalCount(this.mListItemTotalCount + result);
    }

    private void OnSetItemCountBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mSetCountInput.get_text(), out result))
        return;
      this.SetListItemTotalCount(result);
    }
  }
}
