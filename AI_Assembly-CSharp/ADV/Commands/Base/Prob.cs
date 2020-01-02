// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Prob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;

namespace ADV.Commands.Base
{
  public class Prob : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]
        {
          nameof (Prob),
          "True",
          "False"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]{ "100", "tagA", "tagB" };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      float percent = float.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string str1 = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string str2 = args3[index3];
      this.scenario.SearchTagJumpOrOpenFile(!Utils.ProbabilityCalclator.DetectFromPercent(percent) ? str2 : str1, this.localLine);
    }
  }
}
