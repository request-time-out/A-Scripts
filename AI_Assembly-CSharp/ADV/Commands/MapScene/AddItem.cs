// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.AddItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.MapScene
{
  public class AddItem : ItemListControlBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "ItemHash", "Num" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]{ "-1", "-1", "1" };
      }
    }

    protected override void ItemListProc(
      StuffItemInfo itemInfo,
      List<StuffItem> itemList,
      StuffItem stuffItem)
    {
      Debug.Log((object) string.Format("AddItem:{0}x{1}", (object) itemInfo.Name, (object) stuffItem.Count));
      itemList.AddItem(stuffItem);
    }

    public override void Do()
    {
      base.Do();
      int cnt = 0;
      this.SetItem(ref cnt);
    }
  }
}
