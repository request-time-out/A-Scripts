// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.Extension
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
  internal static class Extension
  {
    public static bool AddItemVars(this TextScenario self, StuffItem Item)
    {
      Dictionary<string, ValData> vars = self.Vars;
      string index1 = nameof (Item);
      StuffItemInfo stuffItemInfo = (StuffItemInfo) null;
      if (Item != null && Singleton<Resources>.IsInstance())
        stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(Item.CategoryID, Item.ID);
      vars[index1] = new ValData((object) (stuffItemInfo?.Name ?? string.Empty));
      Dictionary<string, ValData> dictionary = vars;
      string index2 = string.Format("{0}.Hash", (object) index1);
      int? nameHash = stuffItemInfo?.nameHash;
      ValData valData = new ValData((object) (!nameHash.HasValue ? -1 : nameHash.Value));
      dictionary[index2] = valData;
      if (Item == null)
        return false;
      vars[string.Format("{0}.{1}", (object) index1, (object) "CategoryID")] = new ValData((object) Item.CategoryID);
      vars[string.Format("{0}.{1}", (object) index1, (object) "ID")] = new ValData((object) Item.ID);
      vars[string.Format("{0}.{1}", (object) index1, (object) "Count")] = new ValData((object) Item.Count);
      return true;
    }

    public static bool AddItemVars(this TextScenario self, StuffItemInfo Item, int Count)
    {
      Dictionary<string, ValData> vars = self.Vars;
      string index1 = nameof (Item);
      vars[index1] = new ValData((object) (Item?.Name ?? string.Empty));
      Dictionary<string, ValData> dictionary = vars;
      string index2 = string.Format("{0}.Hash", (object) index1);
      int? nameHash = Item?.nameHash;
      ValData valData = new ValData((object) (!nameHash.HasValue ? -1 : nameHash.Value));
      dictionary[index2] = valData;
      if (Item == null)
        return false;
      vars[string.Format("{0}.{1}", (object) index1, (object) "CategoryID")] = new ValData((object) Item.CategoryID);
      vars[string.Format("{0}.{1}", (object) index1, (object) "ID")] = new ValData((object) Item.ID);
      vars[string.Format("{0}.{1}", (object) index1, (object) nameof (Count))] = new ValData((object) Count);
      return true;
    }

    public static CharaData GetChara(this CommandBase self, ref int cnt)
    {
      int num;
      cnt = (num = cnt) + 1;
      int index = num;
      if (self.args.IsNullOrEmpty(index))
      {
        Debug.LogError((object) string.Format("Arg none:{0}[{1}]", (object) "args", (object) index));
        return (CharaData) null;
      }
      CharaData charaData = (CharaData) null;
      int result;
      if (!int.TryParse(self.args[index], out result) || (charaData = self.scenario.commandController.GetChara(result)) == null)
        Debug.LogError((object) string.Format("Chara none:{0}({1}[{2}]:{3})", (object) result, (object) "args", (object) index, (object) self.args[index]));
      return charaData;
    }
  }
}
