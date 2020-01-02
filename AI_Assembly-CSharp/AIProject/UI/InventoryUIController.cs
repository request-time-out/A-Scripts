// Decompiled with JetBrains decompiler
// Type: AIProject.UI.InventoryUIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.UI
{
  public class InventoryUIController : InventoryFilterUIController
  {
    public Action<StuffItem> OnSubmit { get; set; }

    public bool isConfirm { get; set; } = true;

    protected override void ItemInfoEvent()
    {
      int selectIndex = -1;
      ItemNodeUI selectItem = (ItemNodeUI) null;
      this.itemListUI.CurrentChanged += (Action<int, ItemNodeUI>) ((i, option) =>
      {
        if (Object.op_Equality((Object) option, (Object) null))
          return;
        selectIndex = i;
        selectItem = option;
        this.SelectItem(selectItem.Item);
      });
      this._itemInfoUI.OnSubmit += (Action) (() =>
      {
        if (!this.isConfirm || selectItem.isNone)
        {
          this.EnterItem(selectIndex, selectItem);
        }
        else
        {
          ConfirmScene.Sentence = this._itemInfoUI.ConfirmLabel;
          ConfirmScene.OnClickedYes = (Action) (() =>
          {
            this.EnterItem(selectIndex, selectItem);
            this.playSE.Play(SoundPack.SystemSE.OK_L);
          });
          ConfirmScene.OnClickedNo = (Action) (() => this.playSE.Play(SoundPack.SystemSE.Cancel));
          Singleton<Game>.Instance.LoadDialog();
        }
      });
    }

    private void EnterItem(int selectIndex, ItemNodeUI selectItem)
    {
      StuffItem stuffItem = selectItem.Item;
      stuffItem.Count -= this._itemInfoUI.Count;
      Action<StuffItem> onSubmit = this.OnSubmit;
      if (onSubmit != null)
        onSubmit(new StuffItem(stuffItem.CategoryID, stuffItem.ID, this._itemInfoUI.Count));
      bool flag = stuffItem.Count <= 0;
      this._itemInfoUI.Refresh(stuffItem);
      List<StuffItem> stuffItemList = this.itemList();
      if (flag)
      {
        stuffItemList.Remove(stuffItem);
        this.itemListUI.RemoveItemNode(selectIndex);
        this.itemListUI.ForceSetNonSelect();
        this._itemInfoUI.Close();
      }
      this._inventoryUI.Refresh();
    }
  }
}
