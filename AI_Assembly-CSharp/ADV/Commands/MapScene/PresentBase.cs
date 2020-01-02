// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.PresentBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;

namespace ADV.Commands.MapScene
{
  public abstract class PresentBase : CommandBase
  {
    protected abstract Resources.GameInfoTables.AdvPresentItemInfo GetAdvPresentItemInfo(
      ChaControl chaCtrl);

    protected void SetPresentInfo(ref int cnt)
    {
      CharaData chara = this.GetChara(ref cnt);
      if (chara == null)
        return;
      Resources.GameInfoTables.AdvPresentItemInfo advPresentItemInfo = this.GetAdvPresentItemInfo(chara.chaCtrl);
      if (advPresentItemInfo == null)
        return;
      int eventItemId = advPresentItemInfo.eventItemID;
      string name = advPresentItemInfo.itemInfo.Name;
      int nameHash = advPresentItemInfo.itemInfo.nameHash;
      PlayState autoPlayState = chara.GetAutoPlayState();
      for (int index = 0; index < autoPlayState.ItemInfoCount; ++index)
      {
        ActionItemInfo eventItemInfo;
        if (Singleton<Resources>.Instance.Map.EventItemList.TryGetValue(eventItemId, out eventItemInfo))
          chara.data.actor.LoadEventItem(eventItemId, autoPlayState.GetItemInfo(index), eventItemInfo);
      }
      this.scenario.Vars["Item"] = new ValData((object) name);
      this.scenario.Vars[string.Format("{0}.{1}", (object) "Item", (object) "Hash")] = new ValData((object) nameHash);
    }
  }
}
