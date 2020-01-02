// Decompiled with JetBrains decompiler
// Type: SuperScrollView.TreeViewWithStickyHeadDemoScript
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
  public class TreeViewWithStickyHeadDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mScrollToButton;
    private Button mExpandAllButton;
    private Button mCollapseAllButton;
    private InputField mScrollToInputItem;
    private InputField mScrollToInputChild;
    private Button mBackButton;
    private TreeViewItemCountMgr mTreeItemCountMgr;
    public ListItem12 mStickeyHeadItem;
    private RectTransform mStickeyHeadItemRf;
    private float mStickeyHeadItemHeight;

    public TreeViewWithStickyHeadDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      int treeViewItemCount = TreeViewDataSourceMgr.Get.TreeViewItemCount;
      for (int index = 0; index < treeViewItemCount; ++index)
        this.mTreeItemCountMgr.AddTreeItem(TreeViewDataSourceMgr.Get.GetItemDataByIndex(index).ChildCount, true);
      this.mLoopListView.InitListView(this.mTreeItemCountMgr.GetTotalItemAndChildCount(), new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mExpandAllButton = (Button) GameObject.Find("ButtonPanel/buttonGroup1/ExpandAllButton").GetComponent<Button>();
      this.mScrollToButton = (Button) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
      this.mCollapseAllButton = (Button) GameObject.Find("ButtonPanel/buttonGroup3/CollapseAllButton").GetComponent<Button>();
      this.mScrollToInputItem = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputFieldItem").GetComponent<InputField>();
      this.mScrollToInputChild = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputFieldChild").GetComponent<InputField>();
      // ISSUE: method pointer
      ((UnityEvent) this.mScrollToButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnJumpBtnClicked)));
      this.mBackButton = (Button) GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mExpandAllButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnExpandAllBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mCollapseAllButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnCollapseAllBtnClicked)));
      Rect rect = ((RectTransform) ((Component) this.mStickeyHeadItem).GetComponent<RectTransform>()).get_rect();
      this.mStickeyHeadItemHeight = ((Rect) ref rect).get_height();
      this.mStickeyHeadItem.Init();
      this.mStickeyHeadItem.SetClickCallBack(new Action<int>(this.OnExpandClicked));
      this.mStickeyHeadItemRf = (RectTransform) ((Component) this.mStickeyHeadItem).get_gameObject().GetComponent<RectTransform>();
      // ISSUE: method pointer
      ((UnityEvent<Vector2>) this.mLoopListView.ScrollRect.get_onValueChanged()).AddListener(new UnityAction<Vector2>((object) this, __methodptr(OnScrollContentPosChanged)));
      this.UpdateStickeyHeadPos();
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0)
        return (LoopListViewItem2) null;
      TreeViewItemCountData viewItemCountData = this.mTreeItemCountMgr.QueryTreeItemByTotalIndex(index);
      if (viewItemCountData == null)
        return (LoopListViewItem2) null;
      int mTreeItemIndex = viewItemCountData.mTreeItemIndex;
      TreeViewItemData itemDataByIndex = TreeViewDataSourceMgr.Get.GetItemDataByIndex(mTreeItemIndex);
      if (!viewItemCountData.IsChild(index))
      {
        LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
        ListItem12 component = (ListItem12) ((Component) loopListViewItem2).GetComponent<ListItem12>();
        if (!loopListViewItem2.IsInitHandlerCalled)
        {
          loopListViewItem2.IsInitHandlerCalled = true;
          component.Init();
          component.SetClickCallBack(new Action<int>(this.OnExpandClicked));
        }
        loopListViewItem2.UserIntData1 = mTreeItemIndex;
        loopListViewItem2.UserIntData2 = 0;
        component.mText.set_text(itemDataByIndex.mName);
        component.SetItemData(mTreeItemIndex, viewItemCountData.mIsExpand);
        return loopListViewItem2;
      }
      int childIndex = viewItemCountData.GetChildIndex(index);
      ItemData child = itemDataByIndex.GetChild(childIndex);
      if (child == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2_1 = listView.NewListViewItem("ItemPrefab2");
      ListItem13 component1 = (ListItem13) ((Component) loopListViewItem2_1).GetComponent<ListItem13>();
      if (!loopListViewItem2_1.IsInitHandlerCalled)
      {
        loopListViewItem2_1.IsInitHandlerCalled = true;
        component1.Init();
      }
      loopListViewItem2_1.UserIntData1 = mTreeItemIndex;
      loopListViewItem2_1.UserIntData2 = childIndex;
      component1.SetItemData(child, mTreeItemIndex, childIndex);
      return loopListViewItem2_1;
    }

    public void OnExpandClicked(int index)
    {
      this.mTreeItemCountMgr.ToggleItemExpand(index);
      this.mLoopListView.SetListItemCount(this.mTreeItemCountMgr.GetTotalItemAndChildCount(), false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnJumpBtnClicked()
    {
      int result1 = 0;
      int result2 = 0;
      if (!int.TryParse(this.mScrollToInputItem.get_text(), out result1))
        return;
      if (!int.TryParse(this.mScrollToInputChild.get_text(), out result2))
        result2 = 0;
      if (result2 < 0)
        result2 = 0;
      TreeViewItemCountData treeItem = this.mTreeItemCountMgr.GetTreeItem(result1);
      if (treeItem == null)
        return;
      int mChildCount = treeItem.mChildCount;
      int itemIndex;
      if (!treeItem.mIsExpand || mChildCount == 0 || result2 == 0)
      {
        itemIndex = treeItem.mBeginIndex;
      }
      else
      {
        if (result2 > mChildCount)
          result2 = mChildCount;
        if (result2 < 1)
          result2 = 1;
        itemIndex = treeItem.mBeginIndex + result2;
      }
      this.mLoopListView.MovePanelToItemIndex(itemIndex, this.mStickeyHeadItemHeight);
    }

    private void OnExpandAllBtnClicked()
    {
      int treeViewItemCount = this.mTreeItemCountMgr.TreeViewItemCount;
      for (int treeIndex = 0; treeIndex < treeViewItemCount; ++treeIndex)
        this.mTreeItemCountMgr.SetItemExpand(treeIndex, true);
      this.mLoopListView.SetListItemCount(this.mTreeItemCountMgr.GetTotalItemAndChildCount(), false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnCollapseAllBtnClicked()
    {
      int treeViewItemCount = this.mTreeItemCountMgr.TreeViewItemCount;
      for (int treeIndex = 0; treeIndex < treeViewItemCount; ++treeIndex)
        this.mTreeItemCountMgr.SetItemExpand(treeIndex, false);
      this.mLoopListView.SetListItemCount(this.mTreeItemCountMgr.GetTotalItemAndChildCount(), false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void UpdateStickeyHeadPos()
    {
      bool activeSelf = ((Component) this.mStickeyHeadItem).get_gameObject().get_activeSelf();
      int shownItemCount = this.mLoopListView.ShownItemCount;
      if (shownItemCount == 0)
      {
        if (!activeSelf)
          return;
        ((Component) this.mStickeyHeadItem).get_gameObject().SetActive(false);
      }
      else
      {
        LoopListViewItem2 shownItemByIndex = this.mLoopListView.GetShownItemByIndex(0);
        Vector3 cornerPosInViewPort = this.mLoopListView.GetItemCornerPosInViewPort(shownItemByIndex, ItemCornerEnum.LeftTop);
        LoopListViewItem2 loopListViewItem2 = (LoopListViewItem2) null;
        float y = (float) cornerPosInViewPort.y;
        float num1 = y - shownItemByIndex.ItemSizeWithPadding;
        int num2 = -1;
        if ((double) y <= 0.0)
        {
          if (!activeSelf)
            return;
          ((Component) this.mStickeyHeadItem).get_gameObject().SetActive(false);
        }
        else
        {
          if ((double) num1 < 0.0)
          {
            loopListViewItem2 = shownItemByIndex;
            num2 = 0;
          }
          else
          {
            for (int index = 1; index < shownItemCount; ++index)
            {
              LoopListViewItem2 indexWithoutCheck = this.mLoopListView.GetShownItemByIndexWithoutCheck(index);
              float num3 = num1;
              num1 = num3 - indexWithoutCheck.ItemSizeWithPadding;
              if ((double) num3 >= 0.0 && (double) num1 <= 0.0)
              {
                loopListViewItem2 = indexWithoutCheck;
                num2 = index;
                break;
              }
            }
          }
          if (Object.op_Equality((Object) loopListViewItem2, (Object) null))
          {
            if (!activeSelf)
              return;
            ((Component) this.mStickeyHeadItem).get_gameObject().SetActive(false);
          }
          else
          {
            int userIntData1 = loopListViewItem2.UserIntData1;
            int userIntData2 = loopListViewItem2.UserIntData2;
            TreeViewItemCountData treeItem = this.mTreeItemCountMgr.GetTreeItem(userIntData1);
            if (treeItem == null)
            {
              if (!activeSelf)
                return;
              ((Component) this.mStickeyHeadItem).get_gameObject().SetActive(false);
            }
            else if (!treeItem.mIsExpand || treeItem.mChildCount == 0)
            {
              if (!activeSelf)
                return;
              ((Component) this.mStickeyHeadItem).get_gameObject().SetActive(false);
            }
            else
            {
              if (!activeSelf)
                ((Component) this.mStickeyHeadItem).get_gameObject().SetActive(true);
              if (this.mStickeyHeadItem.TreeItemIndex != userIntData1)
              {
                this.mStickeyHeadItem.mText.set_text(TreeViewDataSourceMgr.Get.GetItemDataByIndex(userIntData1).mName);
                this.mStickeyHeadItem.SetItemData(userIntData1, treeItem.mIsExpand);
              }
              ((Component) this.mStickeyHeadItem).get_gameObject().get_transform().set_localPosition(Vector3.get_zero());
              float num3 = -num1;
              float padding = loopListViewItem2.Padding;
              if ((double) num3 - (double) padding >= (double) this.mStickeyHeadItemHeight)
                return;
              for (int index = num2 + 1; index < shownItemCount; ++index)
              {
                LoopListViewItem2 indexWithoutCheck = this.mLoopListView.GetShownItemByIndexWithoutCheck(index);
                if (indexWithoutCheck.UserIntData1 == userIntData1)
                {
                  num3 += indexWithoutCheck.ItemSizeWithPadding;
                  padding = indexWithoutCheck.Padding;
                  if ((double) num3 - (double) padding >= (double) this.mStickeyHeadItemHeight)
                    return;
                }
                else
                  break;
              }
              this.mStickeyHeadItemRf.set_anchoredPosition3D(new Vector3(0.0f, this.mStickeyHeadItemHeight - (num3 - padding), 0.0f));
            }
          }
        }
      }
    }

    private void OnScrollContentPosChanged(Vector2 pos)
    {
      this.UpdateStickeyHeadPos();
    }
  }
}
