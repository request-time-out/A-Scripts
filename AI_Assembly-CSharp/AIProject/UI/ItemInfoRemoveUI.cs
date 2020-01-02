// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemInfoRemoveUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using UnityEngine;

namespace AIProject.UI
{
  public class ItemInfoRemoveUI : ItemInfoUI
  {
    public override void Refresh(StuffItem item)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID);
      this._itemName.set_text(stuffItemInfo.Name);
      this._flavorText.set_text(stuffItemInfo.Explanation);
      if (Object.op_Inequality((Object) this._infoLayout, (Object) null))
        this._infoLayout.SetActive(stuffItemInfo.isTrash);
      this.Refresh(item.Count);
    }
  }
}
