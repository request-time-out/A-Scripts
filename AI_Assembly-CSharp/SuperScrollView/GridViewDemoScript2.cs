// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridViewDemoScript2
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
  public class GridViewDemoScript2 : MonoBehaviour
  {
    public LoopGridView mLoopGridView;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;

    public GridViewDemoScript2()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopGridView.InitGridView(DataSourceMgr.Get.TotalItemCount, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByRowColumn), (LoopGridViewSettingParam) null, (LoopGridViewInitParam) null);
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
      ListItem18 component = (ListItem18) ((Component) loopGridViewItem).GetComponent<ListItem18>();
      if (!loopGridViewItem.IsInitHandlerCalled)
      {
        loopGridViewItem.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, itemIndex, row, column);
      return loopGridViewItem;
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      this.mLoopGridView.MovePanelToItemByIndex(result, 0.0f, 0.0f);
    }

    private void OnAddItemBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mAddItemInput.get_text(), out result))
        return;
      this.mLoopGridView.SetListItemCount(result + this.mLoopGridView.ItemTotalCount, false);
    }

    private void OnSetItemCountBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mSetCountInput.get_text(), out result))
        return;
      if (result > DataSourceMgr.Get.TotalItemCount)
        result = DataSourceMgr.Get.TotalItemCount;
      if (result < 0)
        result = 0;
      this.mLoopGridView.SetListItemCount(result, true);
    }
  }
}
