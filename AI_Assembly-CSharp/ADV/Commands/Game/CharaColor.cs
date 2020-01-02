// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.CharaColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;

namespace ADV.Commands.Game
{
  public class CharaColor : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "isActive", "Color" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          int.MaxValue.ToString(),
          bool.FalseString,
          null
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
      ChaFileStatus status = this.scenario.commandController.GetChara(int.Parse(args1[index1])).chaCtrl.fileStatus;
      ChaFileStatus chaFileStatus = status;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int num4 = bool.Parse(args2[index2]) ? 1 : 0;
      chaFileStatus.visibleSimple = num4 != 0;
      string[] args3 = this.args;
      int index3 = num3;
      int num5 = index3 + 1;
      Action<string> act = (Action<string>) (colorStr => status.simpleColor = colorStr.GetColorCheck().Value);
      args3.SafeProc(index3, act);
    }
  }
}
