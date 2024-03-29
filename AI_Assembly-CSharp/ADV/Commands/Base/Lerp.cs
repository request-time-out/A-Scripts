﻿// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Lerp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Lerp : CommandBase
  {
    private string answer;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Answer",
          "A",
          "B",
          "T",
          "isUnclamped"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          "Answer",
          "0",
          "0",
          "0",
          bool.FalseString
        };
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
      int num1 = 1;
      Func<float, float, float, bool, float> func1 = (Func<float, float, float, bool, float>) ((a, b, t, isUnclamped) => isUnclamped ? Mathf.Lerp(a, b, t) : Mathf.LerpUnclamped(a, b, t));
      Dictionary<string, ValData> vars = this.scenario.Vars;
      string answer = this.answer;
      Func<float, float, float, bool, float> func2 = func1;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      double num3 = (double) float.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num4 = index2 + 1;
      double num5 = (double) float.Parse(args2[index2]);
      string[] args3 = this.args;
      int index3 = num4;
      int num6 = index3 + 1;
      double num7 = (double) float.Parse(args3[index3]);
      string[] args4 = this.args;
      int index4 = num6;
      int num8 = index4 + 1;
      int num9 = bool.Parse(args4[index4]) ? 1 : 0;
      ValData valData = new ValData((object) func2((float) num3, (float) num5, (float) num7, num9 != 0));
      vars[answer] = valData;
    }
  }
}
