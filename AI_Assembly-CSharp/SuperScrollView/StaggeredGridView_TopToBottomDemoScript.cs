// Decompiled with JetBrains decompiler
// Type: SuperScrollView.StaggeredGridView_TopToBottomDemoScript
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
  public class StaggeredGridView_TopToBottomDemoScript : MonoBehaviour
  {
    public LoopStaggeredGridView mLoopListView;
    private Button mScrollToButton;
    private Button mAddItemButton;
    private Button mSetCountButton;
    private InputField mScrollToInput;
    private InputField mAddItemInput;
    private InputField mSetCountInput;
    private Button mBackButton;
    private int[] mItemHeightArrayForDemo;

    public StaggeredGridView_TopToBottomDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
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
      this.InitItemHeightArrayForDemo();
      this.mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount, new GridViewLayoutParam()
      {
        mPadding1 = 10f,
        mPadding2 = 10f,
        mColumnOrRowCount = 3,
        mItemWidthOrHeight = 217f
      }, new Func<LoopStaggeredGridView, int, LoopStaggeredGridViewItem>(this.OnGetItemByIndex), (StaggeredGridViewInitParam) null);
    }

    private LoopStaggeredGridViewItem OnGetItemByIndex(
      LoopStaggeredGridView listView,
      int index)
    {
      if (index < 0)
        return (LoopStaggeredGridViewItem) null;
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(index);
      if (itemDataByIndex == null)
        return (LoopStaggeredGridViewItem) null;
      LoopStaggeredGridViewItem staggeredGridViewItem = listView.NewListViewItem("ItemPrefab0");
      ListItem5 component = (ListItem5) ((Component) staggeredGridViewItem).GetComponent<ListItem5>();
      if (!staggeredGridViewItem.IsInitHandlerCalled)
      {
        staggeredGridViewItem.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(itemDataByIndex, index);
      float num = (float) (300.0 + (double) this.mItemHeightArrayForDemo[index % this.mItemHeightArrayForDemo.Length] * 10.0);
      staggeredGridViewItem.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, num);
      return staggeredGridViewItem;
    }

    private void InitItemHeightArrayForDemo()
    {
      this.mItemHeightArrayForDemo = new int[100];
      for (int index = 0; index < this.mItemHeightArrayForDemo.Length; ++index)
        this.mItemHeightArrayForDemo[index] = Random.Range(0, 20);
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      if (result < 0)
        result = 0;
      this.mLoopListView.MovePanelToItemIndex(result, 0.0f);
    }

    private void OnAddItemBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mAddItemInput.get_text(), out result))
        return;
      int itemCount = this.mLoopListView.ItemTotalCount + result;
      if (itemCount < 0 || itemCount > DataSourceMgr.Get.TotalItemCount)
        return;
      this.mLoopListView.SetListItemCount(itemCount, false);
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
