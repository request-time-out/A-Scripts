// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Switch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.Base
{
  public class Switch : CommandBase
  {
    private const string defaultKey = "default";
    private string key;
    private Dictionary<string, string> answers;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Key", "CaseTag" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "a", "Case,Tag" };
      }
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num = 0;
      string[] args = this.args;
      int index = num;
      int count = index + 1;
      this.key = args[index];
      this.answers = ((IEnumerable<string>) this.args).Skip<string>(count).Select<string, string[]>((Func<string, string[]>) (s => s.Split(','))).Select<string[], string[]>((Func<string[], string[]>) (array =>
      {
        if (array.Length != 1)
          return array;
        return new string[2]{ "default", array[0] };
      })).ToDictionary<string[], string, string>((Func<string[], string>) (v => v[0]), (Func<string[], string>) (v => v[1]));
    }

    public override void Do()
    {
      base.Do();
      string answer;
      if (!this.answers.TryGetValue(this.scenario.Vars[this.key].o.ToString(), out answer))
        answer = this.answers["default"];
      this.scenario.SearchTagJumpOrOpenFile(answer, this.localLine);
    }
  }
}
