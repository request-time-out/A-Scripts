// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.RandomVar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class RandomVar : CommandBase
  {
    private System.Type type;
    private string name;
    private string min;
    private string max;
    private int refMinCnt;
    private int refMaxCnt;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "Type",
          "Variable",
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
          "int",
          string.Empty,
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Convert(string fileName, ref string[] args)
    {
      new VAR().Convert(fileName, ref args);
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.type = System.Type.GetType(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.name = args2[index2];
      if (!(this.type != typeof (bool)))
        return;
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      this.min = args3.SafeGet<string>(index3);
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      this.max = args4.SafeGet<string>(index4);
      this.refMinCnt = VAR.RefCheck(ref this.min);
      this.refMaxCnt = VAR.RefCheck(ref this.max);
    }

    public override void Do()
    {
      base.Do();
      Dictionary<string, ValData> vars = this.scenario.Vars;
      VAR.RefGet(this.type, this.refMinCnt, this.min, vars).SafeProc<string>((Action<string>) (s => this.min = s));
      VAR.RefGet(this.type, this.refMaxCnt, this.max, vars).SafeProc<string>((Action<string>) (s => this.max = s));
      if (this.type == typeof (int))
        vars[this.name] = new ValData(ValData.Convert((object) Random.Range(int.Parse(this.min), int.Parse(this.max) + 1), this.type));
      else if (this.type == typeof (float))
        vars[this.name] = new ValData(ValData.Convert((object) Random.Range(float.Parse(this.min), float.Parse(this.max)), this.type));
      else if (this.type == typeof (string))
      {
        vars[this.name] = new ValData(ValData.Convert(Random.Range(0, 2) != 1 ? (object) this.max : (object) this.min, this.type));
      }
      else
      {
        if (!(this.type == typeof (bool)))
          return;
        vars[this.name] = new ValData(ValData.Convert((object) (Random.Range(0, 2) == 1), this.type));
      }
    }
  }
}
