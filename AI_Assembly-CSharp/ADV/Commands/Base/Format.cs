// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Format
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace ADV.Commands.Base
{
  public class Format : CommandBase
  {
    private List<object> parameters = new List<object>();
    public string name;
    public string format;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]
        {
          "Variable",
          nameof (Format),
          "Args"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]{ string.Empty, "{0:00}", "1" };
      }
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.name = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.format = args2[index2];
      int cnt = num3;
      int num4 = cnt + 1;
      string[] argToSplitLast = this.GetArgToSplitLast(cnt);
      Dictionary<string, ValData> vars = this.scenario.Vars;
      int index3 = -1;
      while (++index3 < argToSplitLast.Length)
      {
        ValData valData;
        if (vars.TryGetValue(argToSplitLast[index3], out valData))
          this.parameters.Add(valData.o);
        else
          this.parameters.Add((object) argToSplitLast[index3]);
      }
    }

    public override void Do()
    {
      base.Do();
      this.scenario.Vars[this.name] = new ValData((object) string.Format(this.format, this.parameters.ToArray()));
    }
  }
}
