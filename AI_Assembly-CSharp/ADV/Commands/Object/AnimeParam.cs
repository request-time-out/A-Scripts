// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.AnimeParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using UnityEngine;

namespace ADV.Commands.Object
{
  public class AnimeParam : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "Name",
          "Type",
          "Param1",
          "Param2"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          string.Empty,
          AnimeParam.Type.Float.ToString(),
          string.Empty,
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      Animator component = (Animator) this.scenario.commandController.Objects[args1[index1]].GetComponent<Animator>();
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int num4;
      switch (args2[index2].Check(true, Enum.GetNames(typeof (AnimeParam.Type))))
      {
        case 0:
          Animator animator1 = component;
          string[] args3 = this.args;
          int index3 = num3;
          int num5 = index3 + 1;
          string str1 = args3[index3];
          string[] args4 = this.args;
          int index4 = num5;
          num4 = index4 + 1;
          double num6 = (double) float.Parse(args4[index4]);
          animator1.SetFloat(str1, (float) num6);
          break;
        case 1:
          Animator animator2 = component;
          string[] args5 = this.args;
          int index5 = num3;
          int num7 = index5 + 1;
          string str2 = args5[index5];
          string[] args6 = this.args;
          int index6 = num7;
          num4 = index6 + 1;
          int num8 = int.Parse(args6[index6]);
          animator2.SetInteger(str2, num8);
          break;
        case 2:
          Animator animator3 = component;
          string[] args7 = this.args;
          int index7 = num3;
          int num9 = index7 + 1;
          string str3 = args7[index7];
          string[] args8 = this.args;
          int index8 = num9;
          num4 = index8 + 1;
          int num10 = bool.Parse(args8[index8]) ? 1 : 0;
          animator3.SetBool(str3, num10 != 0);
          break;
        case 3:
          Animator animator4 = component;
          string[] args9 = this.args;
          int index9 = num3;
          num4 = index9 + 1;
          string str4 = args9[index9];
          animator4.SetTrigger(str4);
          break;
        case 4:
          Animator animator5 = component;
          string[] args10 = this.args;
          int index10 = num3;
          int num11 = index10 + 1;
          int num12 = int.Parse(args10[index10]);
          string[] args11 = this.args;
          int index11 = num11;
          num4 = index11 + 1;
          double num13 = (double) float.Parse(args11[index11]);
          animator5.SetLayerWeight(num12, (float) num13);
          break;
      }
    }

    private enum Type
    {
      Float,
      Int,
      Bool,
      Trigger,
      Weight,
    }
  }
}
