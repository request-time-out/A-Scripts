// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.SetItemScrounge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;

namespace ADV.Commands.MapScene
{
  public class SetItemScrounge : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "No" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ "0" };
      }
    }

    public override void Do()
    {
      base.Do();
      int cnt = 0;
      CharaData chara = this.GetChara(ref cnt);
      if (chara == null)
        return;
      Tuple<StuffItemInfo, int> advScroungeInfo = Singleton<Resources>.Instance.GameInfo.GetAdvScroungeInfo(chara.chaCtrl);
      StuffItemInfo stuffItemInfo = advScroungeInfo.Item1;
      int num = advScroungeInfo.Item2;
      this.scenario.AddItemVars(stuffItemInfo, num);
      chara.data.agentActor.AgentData.ItemScrounge.Set((IReadOnlyCollection<StuffItem>) new StuffItem[1]
      {
        new StuffItem(stuffItemInfo.CategoryID, stuffItemInfo.ID, num)
      });
    }
  }
}
