// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Max
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Max : CommandBase
  {
    private string answer;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Answer", "A", "B" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]{ "Answer", "0", "0" };
      }
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      this.answer = this.args[0];
    }

    public override void Do()
    {
      base.Do();
      string[] argToSplitLast = this.GetArgToSplitLast(1);
      // ISSUE: reference to a compiler-generated field
      if (Max.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Max.\u003C\u003Ef__mg\u0024cache0 = new Func<string, float>(float.Parse);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, float> fMgCache0 = Max.\u003C\u003Ef__mg\u0024cache0;
      this.scenario.Vars[this.answer] = new ValData((object) Mathf.Max(((IEnumerable<string>) argToSplitLast).Select<string, float>(fMgCache0).ToArray<float>()));
    }
  }
}
