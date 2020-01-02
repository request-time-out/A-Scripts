// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.InventoryCheck
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
  public class InventoryCheck : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "True", "False", "Type" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          string.Empty,
          string.Empty,
          "0"
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num = int.Parse(this.args[2]);
      List<StuffItem> stuffItemList;
      int capacity;
      switch (num)
      {
        case 0:
          stuffItemList = Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList;
          capacity = Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax;
          break;
        case 1:
          stuffItemList = Singleton<Game>.Instance.Environment.ItemListInStorage;
          capacity = Singleton<Resources>.Instance.DefinePack.ItemBoxCapacityDefines.StorageCapacity;
          break;
        case 2:
          stuffItemList = Singleton<Game>.Instance.Environment.ItemListInPantry;
          capacity = Singleton<Resources>.Instance.DefinePack.ItemBoxCapacityDefines.PantryCapacity;
          break;
        default:
          Debug.LogError((object) string.Format("{0}:{1}", (object) "type", (object) num));
          return;
      }
      bool flag = ((IReadOnlyCollection<StuffItem>) stuffItemList).CanAddItem(capacity, (StuffItem) null, 1);
      string str = !flag ? this.args[1] : this.args[0];
      if (str.IsNullOrEmpty())
        Debug.LogWarning((object) string.Format("IF[tag:Empty]{0}:{1}", (object) "answer", (object) flag));
      else
        this.scenario.SearchTagJumpOrOpenFile(str, this.localLine);
    }
  }
}
