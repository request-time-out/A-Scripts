// Decompiled with JetBrains decompiler
// Type: SuperScrollView.GridViewSampleDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SuperScrollView
{
  public class GridViewSampleDemo : MonoBehaviour
  {
    public LoopListView2 mLoopListView;
    private const int mItemCountPerRow = 3;
    private int mItemTotalCount;

    public GridViewSampleDemo()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      int itemTotalCount = this.mItemTotalCount / 3;
      if (this.mItemTotalCount % 3 > 0)
        ++itemTotalCount;
      this.mLoopListView.InitListView(itemTotalCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), (LoopListViewInitParam) null);
    }

    private LoopListViewItem2 OnGetItemByIndex(
      LoopListView2 listView,
      int rowIndex)
    {
      if (rowIndex < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("RowPrefab");
      ListItem15 component = (ListItem15) ((Component) loopListViewItem2).GetComponent<ListItem15>();
      if (!loopListViewItem2.IsInitHandlerCalled)
      {
        loopListViewItem2.IsInitHandlerCalled = true;
        component.Init();
      }
      for (int index = 0; index < 3; ++index)
      {
        int num = rowIndex * 3 + index;
        if (num >= this.mItemTotalCount)
        {
          ((Component) component.mItemList[index]).get_gameObject().SetActive(false);
        }
        else
        {
          ((Component) component.mItemList[index]).get_gameObject().SetActive(true);
          component.mItemList[index].mNameText.set_text("Item" + (object) num);
        }
      }
      return loopListViewItem2;
    }
  }
}
