// Decompiled with JetBrains decompiler
// Type: AIProject.ADVParames
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using System.Collections.Generic;

namespace AIProject
{
  internal class ADVParames : SceneParameter
  {
    public ADVParames(ADV.IData data)
      : base(data)
    {
    }

    public override void Init()
    {
      bool? listVisibleEnabled = this.data.pack?.isCommandListVisibleEnabled;
      if ((!listVisibleEnabled.HasValue ? 1 : (listVisibleEnabled.Value ? 1 : 0)) != 0)
        MapUIContainer.CommandList.Visibled = false;
      this.data.pack?.CommandListVisibleEnabledDefault();
      List<Program.Transfer> transferList = this.data.pack?.Create() ?? Program.Transfer.NewList(true, false);
      SceneParameter.advScene.Scenario.openData.Set(this.data.openData);
      SceneParameter.advScene.Scenario.transferList = transferList;
      SceneParameter.advScene.Scenario.SetPackage(this.data.pack);
      SceneParameter.advScene.Scenario.captions.CanvasGroupON();
    }

    public override void Release()
    {
      this.data.pack?.Receive(SceneParameter.advScene.Scenario);
      SceneParameter.advScene.Scenario.SetPackage((IPack) null);
      MapUIContainer.SetActiveChoiceUI(false);
      Singleton<MapUIContainer>.Instance.CloseADV();
    }
  }
}
