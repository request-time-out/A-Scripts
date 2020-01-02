// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.LerpV3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class LerpV3 : CommandBase
  {
    private string answer;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "Answer",
          "Value",
          "Min",
          "Max"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          "Answer",
          "0",
          string.Empty,
          string.Empty
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
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      float shape = float.Parse(args1[index1]);
      Dictionary<string, Vector3> v3Dic = this.scenario.commandController.V3Dic;
      Dictionary<string, Vector3> dictionary1 = v3Dic;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string index3 = args2[index2];
      Vector3 min = dictionary1[index3];
      Dictionary<string, Vector3> dictionary2 = v3Dic;
      string[] args3 = this.args;
      int index4 = num3;
      int num4 = index4 + 1;
      string index5 = args3[index4];
      Vector3 max = dictionary2[index5];
      v3Dic[this.answer] = MathfEx.GetShapeLerpPositionValue(shape, min, max);
    }
  }
}
