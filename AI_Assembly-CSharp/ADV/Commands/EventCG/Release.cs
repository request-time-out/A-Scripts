// Decompiled with JetBrains decompiler
// Type: ADV.Commands.EventCG.Release
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace ADV.Commands.EventCG
{
  public class Release : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "isMotionContinue" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ bool.FalseString };
      }
    }

    public override void Do()
    {
      base.Do();
      if (!Common.Release(this.scenario))
        return;
      foreach (KeyValuePair<int, CharaData> character in this.scenario.commandController.Characters)
        character.Value.backup.Repair();
      this.scenario.commandController.useCorrectCamera = true;
    }
  }
}
