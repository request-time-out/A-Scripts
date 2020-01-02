// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ChatMsgListViewDemoScript
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
  public class ChatMsgListViewDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mScrollToButton;
    private InputField mScrollToInput;
    private Button mBackButton;
    private Button mAppendMsgButton;

    public ChatMsgListViewDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mLoopListView.InitListView(ChatMsgDataSourceMgr.Get.TotalItemCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
      this.mScrollToButton = (Button) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
      this.mScrollToInput = (InputField) GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
      // ISSUE: method pointer
      ((UnityEvent) this.mScrollToButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnJumpBtnClicked)));
      this.mBackButton = (Button) GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
      this.mAppendMsgButton = (Button) GameObject.Find("ButtonPanel/buttonGroup1/AppendButton").GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this.mAppendMsgButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnAppendMsgBtnClicked)));
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private void OnAppendMsgBtnClicked()
    {
      ChatMsgDataSourceMgr.Get.AppendOneMsg();
      this.mLoopListView.SetListItemCount(ChatMsgDataSourceMgr.Get.TotalItemCount, false);
      this.mLoopListView.MovePanelToItemIndex(ChatMsgDataSourceMgr.Get.TotalItemCount - 1, 0.0f);
    }

    private void OnJumpBtnClicked()
    {
      int result = 0;
      if (!int.TryParse(this.mScrollToInput.get_text(), out result) || result < 0)
        return;
      this.mLoopListView.MovePanelToItemIndex(result, 0.0f);
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
      if (index < 0 || index >= ChatMsgDataSourceMgr.Get.TotalItemCount)
        return (LoopListViewItem2) null;
      ChatMsg chatMsgByIndex = ChatMsgDataSourceMgr.Get.GetChatMsgByIndex(index);
      if (chatMsgByIndex == null)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = chatMsgByIndex.mPersonId != 0 ? listView.NewListViewItem("ItemPrefab2") : listView.NewListViewItem("ItemPrefab1");
      ListItem4 component = (ListItem4) ((Component) loopListViewItem2).GetComponent<ListItem4>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      component.SetItemData(chatMsgByIndex, index);
      return loopListViewItem2;
    }
  }
}
