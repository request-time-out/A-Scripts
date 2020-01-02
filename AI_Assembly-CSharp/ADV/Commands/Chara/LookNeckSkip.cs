// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.LookNeckSkip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class LookNeckSkip : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "isSkip" };
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
      CommandController commandController = this.scenario.commandController;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      int no = int.Parse(args1[index1]);
      NeckLookCalcVer2 neckLookScript = commandController.GetChara(no).chaCtrl.neckLookCtrl.neckLookScript;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int num4 = bool.Parse(args2[index2]) ? 1 : 0;
      neckLookScript.skipCalc = num4 != 0;
    }
  }
}
