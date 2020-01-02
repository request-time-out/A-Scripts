// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.FixMouth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace ADV.Commands.Chara
{
  public class FixMouth : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "Fix" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]
        {
          int.MaxValue.ToString(),
          bool.FalseString
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      CharaData chara = this.scenario.commandController.GetChara(int.Parse(args1[index1]));
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      Action<string> act = (Action<string>) (s => chara.chaCtrl.ChangeMouthFixed(bool.Parse(s)));
      args2.SafeProc(index2, act);
    }
  }
}
