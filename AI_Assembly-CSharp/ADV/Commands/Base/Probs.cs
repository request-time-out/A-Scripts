// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Probs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.Base
{
  public class Probs : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "ProbTag" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ "Prob,Tag" };
      }
    }

    public override void Do()
    {
      base.Do();
      this.scenario.SearchTagJumpOrOpenFile(Utils.ProbabilityCalclator.DetermineFromDict<string>(((IEnumerable<string>) this.args).Select<string, string[]>((Func<string, string[]>) (s => s.Split(','))).ToDictionary<string[], string, int>((Func<string[], string>) (v => this.scenario.ReplaceVars(v[1])), (Func<string[], int>) (v =>
      {
        int result;
        int.TryParse(this.scenario.ReplaceVars(v[0]), out result);
        return result;
      }))), this.localLine);
    }
  }
}
