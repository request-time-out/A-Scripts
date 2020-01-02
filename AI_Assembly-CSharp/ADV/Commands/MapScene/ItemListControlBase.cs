// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.ItemListControlBase
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
  public abstract class ItemListControlBase : CommandBase
  {
    protected abstract void ItemListProc(
      StuffItemInfo itemInfo,
      List<StuffItem> itemList,
      StuffItem stuffItem);

    protected void SetItem(ref int cnt)
    {
      CharaData chara = this.GetChara(ref cnt);
      if (chara == null)
        return;
      string[] args1 = this.args;
      int num1;
      cnt = (num1 = cnt) + 1;
      int index1 = num1;
      int nameHash = int.Parse(args1[index1]);
      StuffItemInfo itemInfo = Singleton<Resources>.Instance.GameInfo.FindItemInfo(nameHash);
      if (itemInfo == null)
      {
        Debug.LogError((object) string.Format("Item none:{0}", (object) nameHash));
      }
      else
      {
        string[] args2 = this.args;
        int num2;
        cnt = (num2 = cnt) + 1;
        int index2 = num2;
        int result;
        if (!int.TryParse(args2[index2], out result))
        {
          Debug.LogError((object) string.Format("Num none:{0}[{1}]:{2}", (object) "args", (object) (cnt - 1), (object) this.args[cnt - 1]));
          result = 1;
        }
        this.ItemListProc(itemInfo, chara.data.characterInfo.ItemList, new StuffItem(itemInfo.CategoryID, itemInfo.ID, result));
      }
    }
  }
}
