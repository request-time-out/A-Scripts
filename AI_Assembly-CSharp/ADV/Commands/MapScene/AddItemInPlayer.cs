// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.AddItemInPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.MapScene
{
  public class AddItemInPlayer : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "ItemHash", "Num" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "-1", "1" };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      int nameHash = int.Parse(args1[index1]);
      StuffItemInfo itemInfo = Singleton<Resources>.Instance.GameInfo.FindItemInfo(nameHash);
      if (itemInfo == null)
      {
        Debug.LogError((object) string.Format("Item none:{0}", (object) nameHash));
      }
      else
      {
        string[] args2 = this.args;
        int index2 = num2;
        int num3 = index2 + 1;
        int result;
        if (!int.TryParse(args2[index2], out result))
        {
          Debug.LogError((object) string.Format("Num none:{0}[{1}]:{2}", (object) "args", (object) (num3 - 1), (object) this.args[num3 - 1]));
          result = 1;
        }
        StuffItem addItem = new StuffItem(itemInfo.CategoryID, itemInfo.ID, result);
        if (this.AddItem(Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList, Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax, addItem, result))
          Debug.Log((object) string.Format("AddItem(Inventory):{0}x{1}", (object) itemInfo.Name, (object) result));
        else if (this.AddItem(Singleton<Game>.Instance.Environment.ItemListInStorage, Singleton<Resources>.Instance.DefinePack.ItemBoxCapacityDefines.StorageCapacity, addItem, result))
          Debug.Log((object) string.Format("AddItem(ItemBox):{0}x{1}", (object) itemInfo.Name, (object) result));
        else
          Debug.LogError((object) string.Format("AddItem(Failed):{0}x{1}", (object) itemInfo.Name, (object) result));
      }
    }

    private bool AddItem(List<StuffItem> itemList, int capacity, StuffItem addItem, int num)
    {
      if (!((IReadOnlyCollection<StuffItem>) itemList).CanAddItem(capacity, addItem, num))
        return false;
      itemList.AddItem(addItem);
      return true;
    }
  }
}
