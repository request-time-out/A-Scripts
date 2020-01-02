// Decompiled with JetBrains decompiler
// Type: SuperScrollView.TopToBottomSampleDemoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class TopToBottomSampleDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mScrollToButton;
    private Button mAppendItemButton;
    private Button mInsertItemButton;
    private InputField mScrollToInput;
    private List<CustomData> mDataList;
    private int mTotalInsertedCount;

    public TopToBottomSampleDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.InitData();
      this.mLoopListView.InitListView(this.mDataList.Count, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mScrollToButton = (Button) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
      this.mAppendItemButton = (Button) GameObject.Find("ButtonPanel/buttonGroup3/AppendItemButton").GetComponent<Button>();
      this.mInsertItemButton = (Button) GameObject.Find("ButtonPanel/buttonGroup3/InsertItemButton").GetComponent<Button>();
      this.mScrollToInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
      // ISSUE: method pointer
      ((UnityEvent) this.mScrollToButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnJumpBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mAppendItemButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnAppendItemBtnClicked)));
      // ISSUE: method pointer
      ((UnityEvent) this.mInsertItemButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnInsertItemBtnClicked)));
    }

    private void InitData()
    {
      this.mDataList = new List<CustomData>();
      int num = 100;
      for (int index = 0; index < num; ++index)
        this.mDataList.Add(new CustomData()
        {
          mContent = "Item" + (object) index
        });
    }

    private void AppendOneData()
    {
      this.mDataList.Add(new CustomData()
      {
        mContent = "Item" + (object) this.mDataList.Count
      });
    }

    private void InsertOneData()
    {
      ++this.mTotalInsertedCount;
      this.mDataList.Insert(0, new CustomData()
      {
        mContent = "Item(-" + (object) this.mTotalInsertedCount + ")"
      });
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0 || index >= this.mDataList.Count)
        return (LoopListViewItem2) null;
      CustomData mData = this.mDataList[index];
      if (mData == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem16 component = (ListItem16) ((Component) loopListViewItem2).GetComponent<ListItem16>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      component.mNameText.set_text(mData.mContent);
      return loopListViewItem2;
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result))
        return;
      this.mLoopListView.MovePanelToItemIndex(result, 0.0f);
    }

    private void OnAppendItemBtnClicked()
    {
      this.AppendOneData();
      this.mLoopListView.SetListItemCount(this.mDataList.Count, false);
      this.mLoopListView.RefreshAllShownItem();
    }

    private void OnInsertItemBtnClicked()
    {
      this.InsertOneData();
      this.mLoopListView.SetListItemCount(this.mDataList.Count, false);
      this.mLoopListView.RefreshAllShownItem();
    }
  }
}
