// Decompiled with JetBrains decompiler
// Type: SuperScrollView.SpinDatePickerDemoScript
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
  public class SpinDatePickerDemoScript : MonoBehaviour
  {
    private static int[] mMonthDayCountArray = new int[12]
    {
      31,
      28,
      31,
      30,
      31,
      30,
      31,
      31,
      30,
      31,
      30,
      31
    };
    private static string[] mMonthNameArray = new string[12]
    {
      "Jan.",
      "Feb.",
      "Mar.",
      "Apr.",
      "May.",
      "Jun.",
      "Jul.",
      "Aug.",
      "Sep.",
      "Oct.",
      "Nov.",
      "Dec."
    };
    public LoopListView2 mLoopListViewMonth;
    public LoopListView2 mLoopListViewDay;
    public LoopListView2 mLoopListViewHour;
    public Button mBackButton;
    private int mCurSelectedMonth;
    private int mCurSelectedDay;
    private int mCurSelectedHour;

    public SpinDatePickerDemoScript()
    {
      base.\u002Ector();
    }

    public int CurSelectedMonth
    {
      get
      {
        return this.mCurSelectedMonth;
      }
    }

    public int CurSelectedDay
    {
      get
      {
        return this.mCurSelectedDay;
      }
    }

    public int CurSelectedHour
    {
      get
      {
        return this.mCurSelectedHour;
      }
    }

    private void Start()
    {
      this.mLoopListViewMonth.mOnSnapNearestChanged = new Action<LoopListView2, LoopListViewItem2>(this.OnMonthSnapTargetChanged);
      this.mLoopListViewDay.mOnSnapNearestChanged = new Action<LoopListView2, LoopListViewItem2>(this.OnDaySnapTargetChanged);
      this.mLoopListViewHour.mOnSnapNearestChanged = new Action<LoopListView2, LoopListViewItem2>(this.OnHourSnapTargetChanged);
      this.mLoopListViewMonth.InitListView(-1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndexForMonth), (LoopListViewInitParam) null);
      this.mLoopListViewDay.InitListView(-1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndexForDay), (LoopListViewInitParam) null);
      this.mLoopListViewHour.InitListView(-1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndexForHour), (LoopListViewInitParam) null);
      this.mLoopListViewMonth.mOnSnapItemFinished = new Action<LoopListView2, LoopListViewItem2>(this.OnMonthSnapTargetFinished);
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private LoopListViewItem2 OnGetItemByIndexForHour(
      LoopListView2 listView,
      int index)
    {
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem7 component = (ListItem7) ((Component) loopListViewItem2).GetComponent<ListItem7>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      int num1 = 1;
      int num2 = 24;
      int num3 = (index < 0 ? num2 + (index + 1) % num2 - 1 : index % num2) + num1;
      component.Value = num3;
      component.mText.set_text(num3.ToString());
      return loopListViewItem2;
    }

    private LoopListViewItem2 OnGetItemByIndexForMonth(
      LoopListView2 listView,
      int index)
    {
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem7 component = (ListItem7) ((Component) loopListViewItem2).GetComponent<ListItem7>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      int num1 = 1;
      int num2 = 12;
      int num3 = (index < 0 ? num2 + (index + 1) % num2 - 1 : index % num2) + num1;
      component.Value = num3;
      component.mText.set_text(SpinDatePickerDemoScript.mMonthNameArray[num3 - 1]);
      return loopListViewItem2;
    }

    private LoopListViewItem2 OnGetItemByIndexForDay(
      LoopListView2 listView,
      int index)
    {
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem7 component = (ListItem7) ((Component) loopListViewItem2).GetComponent<ListItem7>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      int num1 = 1;
      int mMonthDayCount = SpinDatePickerDemoScript.mMonthDayCountArray[this.mCurSelectedMonth - 1];
      int num2 = (index < 0 ? mMonthDayCount + (index + 1) % mMonthDayCount - 1 : index % mMonthDayCount) + num1;
      component.Value = num2;
      component.mText.set_text(num2.ToString());
      return loopListViewItem2;
    }

    private void OnMonthSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
    {
      int indexInShownItemList = listView.GetIndexInShownItemList(item);
      if (indexInShownItemList < 0)
        return;
      this.mCurSelectedMonth = ((ListItem7) ((Component) item).GetComponent<ListItem7>()).Value;
      this.OnListViewSnapTargetChanged(listView, indexInShownItemList);
    }

    private void OnDaySnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
    {
      int indexInShownItemList = listView.GetIndexInShownItemList(item);
      if (indexInShownItemList < 0)
        return;
      this.mCurSelectedDay = ((ListItem7) ((Component) item).GetComponent<ListItem7>()).Value;
      this.OnListViewSnapTargetChanged(listView, indexInShownItemList);
    }

    private void OnHourSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
    {
      int indexInShownItemList = listView.GetIndexInShownItemList(item);
      if (indexInShownItemList < 0)
        return;
      this.mCurSelectedHour = ((ListItem7) ((Component) item).GetComponent<ListItem7>()).Value;
      this.OnListViewSnapTargetChanged(listView, indexInShownItemList);
    }

    private void OnMonthSnapTargetFinished(LoopListView2 listView, LoopListViewItem2 item)
    {
      this.mLoopListViewDay.RefreshAllShownItemWithFirstIndex(((ListItem7) ((Component) this.mLoopListViewDay.GetShownItemByIndex(0)).GetComponent<ListItem7>()).Value - 1);
    }

    private void OnListViewSnapTargetChanged(LoopListView2 listView, int targetIndex)
    {
      int shownItemCount = listView.ShownItemCount;
      for (int index = 0; index < shownItemCount; ++index)
      {
        ListItem7 component = (ListItem7) ((Component) listView.GetShownItemByIndex(index)).GetComponent<ListItem7>();
        if (index == targetIndex)
          ((Graphic) component.mText).set_color(Color.get_red());
        else
          ((Graphic) component.mText).set_color(Color.get_black());
      }
    }
  }
}
