// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.ReplaceLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace ADV.Commands.Base
{
  public class ReplaceLanguage : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Src", "Dst" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string index2 = args1[index1];
      string dst = string.Empty;
      string[] args2 = this.args;
      int index3 = num2;
      int num3 = index3 + 1;
      Action<string> act = (Action<string>) (s => dst = s);
      args2.SafeProc(index3, act);
      this.scenario.Replaces[index2] = dst;
    }
  }
}
