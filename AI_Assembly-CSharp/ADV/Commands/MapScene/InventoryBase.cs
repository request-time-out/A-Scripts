// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.InventoryBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using AIProject.UI;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.MapScene
{
  public abstract class InventoryBase : CommandBase
  {
    protected bool isClose { get; private set; }

    protected StuffItem Item { get; private set; }

    protected void OpenInventory(int cnt, List<StuffItem> itemList)
    {
      MapUIContainer.ReserveSystemMenuMode(SystemMenuUI.MenuMode.InventoryEnter);
      SystemMenuUI systemUI = MapUIContainer.SystemMenuUI;
      InventoryUIController inventoryUI = systemUI.InventoryEnterUI;
      inventoryUI.isConfirm = true;
      inventoryUI.CountViewerVisible(true);
      inventoryUI.itemList = (Func<List<StuffItem>>) (() => itemList);
      inventoryUI.SetItemFilter(InventoryBase.ToFilter(this.GetArgToSplitLastTable(cnt)));
      inventoryUI.OnSubmit = (Action<StuffItem>) (item =>
      {
        this.Item = item;
        InventoryUIController inventoryUiController = inventoryUI;
        if (inventoryUiController == null)
          return;
        inventoryUiController.OnClose();
      });
      inventoryUI.OnClose = (Action) (() =>
      {
        inventoryUI.OnSubmit = (Action<StuffItem>) null;
        inventoryUI.IsActiveControl = false;
        systemUI.IsActiveControl = false;
        Singleton<Input>.Instance.FocusLevel = 0;
        Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
        this.isClose = true;
        inventoryUI.OnClose = (Action) null;
      });
      MapUIContainer.SetActiveSystemMenuUI(true);
    }

    private static InventoryFacadeViewer.ItemFilter[] ToFilter(
      string[][] arrays)
    {
      return ((IEnumerable<string[]>) arrays).Select<string[], InventoryFacadeViewer.ItemFilter>((Func<string[], InventoryFacadeViewer.ItemFilter>) (x =>
      {
        int category = int.Parse(x[0]);
        IEnumerable<string> source = ((IEnumerable<string>) x).Skip<string>(1);
        // ISSUE: reference to a compiler-generated field
        if (InventoryBase.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InventoryBase.\u003C\u003Ef__mg\u0024cache0 = new Func<string, int>(int.Parse);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, int> fMgCache0 = InventoryBase.\u003C\u003Ef__mg\u0024cache0;
        int[] array = source.Select<string, int>(fMgCache0).ToArray<int>();
        return new InventoryFacadeViewer.ItemFilter(category, array);
      })).ToArray<InventoryFacadeViewer.ItemFilter>();
    }
  }
}
