// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.MotionIKSetPartner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.Chara
{
  public class MotionIKSetPartner : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "Partner" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ int.MaxValue.ToString(), "0" };
      }
    }

    public override void Do()
    {
      base.Do();
      int num = 0;
      string[] args = this.args;
      int index = num;
      int cnt = index + 1;
      this.scenario.commandController.GetChara(int.Parse(args[index])).ikMotion.motionIK.SetPartners(((IEnumerable<string>) CommandBase.RemoveArgsEmpty(this.GetArgToSplitLast(cnt))).Select<string, CharaData>((Func<string, CharaData>) (s => this.scenario.commandController.GetChara(int.Parse(s)))).Select<CharaData, MotionIK>((Func<CharaData, MotionIK>) (charaData => charaData.ikMotion.motionIK)).ToArray<MotionIK>());
    }
  }
}
