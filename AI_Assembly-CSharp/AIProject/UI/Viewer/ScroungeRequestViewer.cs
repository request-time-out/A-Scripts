// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.ScroungeRequestViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace AIProject.UI.Viewer
{
  public class ScroungeRequestViewer : MonoBehaviour
  {
    [SerializeField]
    private ItemListUI _itemListUI;

    public ScroungeRequestViewer()
    {
      base.\u002Ector();
    }

    public ItemListUI itemListUI
    {
      get
      {
        return this._itemListUI;
      }
    }

    public ShopViewer.ItemListController controller { get; }

    public AgentActor agent { get; set; }

    public ItemScrounge itemScrounge
    {
      get
      {
        return this.agent.AgentData.ItemScrounge;
      }
    }

    public bool Check(IReadOnlyCollection<StuffItem> itemList)
    {
      if (!((IEnumerable<StuffItem>) itemList).Any<StuffItem>())
        return false;
      List<StuffItem> list1 = ((IEnumerable<StuffItem>) itemList).Select<StuffItem, StuffItem>((Func<StuffItem, StuffItem>) (item => new StuffItem(item))).ToList<StuffItem>();
      foreach (StuffItem stuffItem in this.itemScrounge.ItemList)
        list1.RemoveItem(stuffItem);
      if (list1.Any<StuffItem>())
        return false;
      List<StuffItem> list2 = this.itemScrounge.ItemList.Select<StuffItem, StuffItem>((Func<StuffItem, StuffItem>) (item => new StuffItem(item))).ToList<StuffItem>();
      foreach (StuffItem stuffItem in (IEnumerable<StuffItem>) itemList)
        list2.RemoveItem(stuffItem);
      return !list2.Any<StuffItem>();
    }

    public bool initialized { get; private set; }

    private void Awake()
    {
      this.controller.Bind(this._itemListUI);
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ScroungeRequestViewer.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
