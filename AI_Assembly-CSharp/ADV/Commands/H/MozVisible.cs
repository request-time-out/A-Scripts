// Decompiled with JetBrains decompiler
// Type: ADV.Commands.H.MozVisible
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.H
{
  public class MozVisible : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "isVisible" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "0", bool.TrueString };
      }
    }

    public override void Do()
    {
      base.Do();
      int cnt = 0;
      int num1 = cnt + 1;
      string[] argToSplit = this.GetArgToSplit(cnt);
      string[] args = this.args;
      int index = num1;
      int num2 = index + 1;
      bool flag = bool.Parse(args[index]);
      foreach (ChaInfo chaInfo in ((IEnumerable<string>) argToSplit).Select<string, CharaData>((Func<string, CharaData>) (s => this.scenario.commandController.GetChara(int.Parse(s)))).Select<CharaData, ChaControl>((Func<CharaData, ChaControl>) (p => p.chaCtrl)))
        chaInfo.hideMoz = !flag;
    }
  }
}
