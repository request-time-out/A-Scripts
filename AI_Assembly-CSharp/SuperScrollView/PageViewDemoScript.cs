// Decompiled with JetBrains decompiler
// Type: SuperScrollView.PageViewDemoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class PageViewDemoScript : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private Button mBackButton;
    private int mPageCount;
    public Transform mDotsRootObj;
    private List<DotElem> mDotElemList;

    public PageViewDemoScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.InitDots();
      LoopListViewInitParam initParam = LoopListViewInitParam.CopyDefaultInitParam();
      initParam.mSnapVecThreshold = 99999f;
      this.mLoopListView.mOnBeginDragAction = new Action(this.OnBeginDrag);
      this.mLoopListView.mOnDragingAction = new Action(this.OnDraging);
      this.mLoopListView.mOnEndDragAction = new Action(this.OnEndDrag);
      this.mLoopListView.mOnSnapNearestChanged = new Action<LoopListView2, LoopListViewItem2>(this.OnSnapNearestChanged);
      this.mLoopListView.InitListView(this.mPageCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), initParam);
      this.mBackButton = (Button) GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this.mBackButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnBackBtnClicked)));
    }

    private void InitDots()
    {
      int childCount = this.mDotsRootObj.get_childCount();
      for (int index1 = 0; index1 < childCount; ++index1)
      {
        Transform child = this.mDotsRootObj.GetChild(index1);
        DotElem dotElem = new DotElem();
        dotElem.mDotElemRoot = ((Component) child).get_gameObject();
        dotElem.mDotSmall = ((Component) child.Find("dotSmall")).get_gameObject();
        dotElem.mDotBig = ((Component) child.Find("dotBig")).get_gameObject();
        ClickEventListener clickEventListener = ClickEventListener.Get(dotElem.mDotElemRoot);
        int index = index1;
        clickEventListener.SetClickEventHandler((Action<GameObject>) (obj => this.OnDotClicked(index)));
        this.mDotElemList.Add(dotElem);
      }
    }

    private void OnDotClicked(int index)
    {
      int nearestItemIndex = this.mLoopListView.CurSnapNearestItemIndex;
      if (nearestItemIndex < 0 || nearestItemIndex >= this.mPageCount || index == nearestItemIndex)
        return;
      if (index > nearestItemIndex)
        this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex + 1);
      else
        this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex - 1);
    }

    private void UpdateAllDots()
    {
      int nearestItemIndex = this.mLoopListView.CurSnapNearestItemIndex;
      if (nearestItemIndex < 0 || nearestItemIndex >= this.mPageCount)
        return;
      int count = this.mDotElemList.Count;
      if (nearestItemIndex >= count)
        return;
      for (int index = 0; index < count; ++index)
      {
        DotElem mDotElem = this.mDotElemList[index];
        if (index != nearestItemIndex)
        {
          mDotElem.mDotSmall.SetActive(true);
          mDotElem.mDotBig.SetActive(false);
        }
        else
        {
          mDotElem.mDotSmall.SetActive(false);
          mDotElem.mDotBig.SetActive(true);
        }
      }
    }

    private void OnSnapNearestChanged(LoopListView2 listView, LoopListViewItem2 item)
    {
      this.UpdateAllDots();
    }

    private void OnBackBtnClicked()
    {
      SceneManager.LoadScene("Menu");
    }

    private LoopListViewItem2 OnGetItemByIndex(
      LoopListView2 listView,
      int pageIndex)
    {
      if (pageIndex < 0 || pageIndex >= this.mPageCount)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab1");
      ListItem14 component = (ListItem14) ((Component) loopListViewItem2).GetComponent<ListItem14>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      List<ListItem14Elem> mElemItemList = component.mElemItemList;
      int count = mElemItemList.Count;
      int num = pageIndex * count;
      int index;
      for (index = 0; index < count; ++index)
      {
        ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num + index);
        if (itemDataByIndex != null)
        {
          ListItem14Elem listItem14Elem = mElemItemList[index];
          listItem14Elem.mRootObj.SetActive(true);
          listItem14Elem.mIcon.set_sprite(ResManager.Get.GetSpriteByName(itemDataByIndex.mIcon));
          listItem14Elem.mName.set_text(itemDataByIndex.mName);
        }
        else
          break;
      }
      if (index < count)
      {
        for (; index < count; ++index)
          mElemItemList[index].mRootObj.SetActive(false);
      }
      return loopListViewItem2;
    }

    private void OnBeginDrag()
    {
    }

    private void OnDraging()
    {
    }

    private void OnEndDrag()
    {
      float x = (float) this.mLoopListView.ScrollRect.get_velocity().x;
      int nearestItemIndex = this.mLoopListView.CurSnapNearestItemIndex;
      LoopListViewItem2 shownItemByItemIndex = this.mLoopListView.GetShownItemByItemIndex(nearestItemIndex);
      if (Object.op_Equality((Object) shownItemByItemIndex, (Object) null))
        this.mLoopListView.ClearSnapData();
      else if ((double) Mathf.Abs(x) < 50.0)
      {
        this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex);
      }
      else
      {
        Vector3 cornerPosInViewPort = this.mLoopListView.GetItemCornerPosInViewPort(shownItemByItemIndex, ItemCornerEnum.LeftTop);
        if (cornerPosInViewPort.x > 0.0)
        {
          if ((double) x > 0.0)
            this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex - 1);
          else
            this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex);
        }
        else if (cornerPosInViewPort.x < 0.0)
        {
          if ((double) x > 0.0)
            this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex);
          else
            this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex + 1);
        }
        else if ((double) x > 0.0)
          this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex - 1);
        else
          this.mLoopListView.SetSnapTargetItemIndex(nearestItemIndex + 1);
      }
    }
  }
}
