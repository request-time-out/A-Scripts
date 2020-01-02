// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.FormatVAR
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace ADV.Commands.Base
{
  public class FormatVAR : CommandBase
  {
    private List<object> parameters = new List<object>();
    public string name;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Variable", "Format", "Args" };
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
      this.name = this.args[0];
      string[] argToSplitLast = this.GetArgToSplitLast(2);
      Dictionary<string, ValData> vars = this.scenario.Vars;
      int index = -1;
      while (++index < argToSplitLast.Length)
      {
        ValData valData;
        if (vars.TryGetValue(argToSplitLast[index], out valData))
          this.parameters.Add(valData.o);
        else
          this.parameters.Add((object) argToSplitLast[index]);
      }
    }

    public override void Do()
    {
      base.Do();
      this.scenario.Vars[this.name] = new ValData((object) string.Format(this.args[1], this.parameters.ToArray()));
    }
  }
}
