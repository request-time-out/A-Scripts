// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.LookEyes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class LookEyes : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "Ptn" };
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
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      int no = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int ptn = int.Parse(args2[index2]);
      this.scenario.commandController.GetChara(no).chaCtrl.ChangeLookEyesPtn(ptn);
    }
  }
}
