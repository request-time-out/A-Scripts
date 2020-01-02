// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.Position
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using UnityEngine;

namespace ADV.Commands.Object
{
  public class Position : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Name",
          "Type",
          "X",
          "Y",
          "Z"
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
          Position.Type.Add.ToString(),
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
      string index2 = args1[index1];
      string[] args2 = this.args;
      int index3 = num2;
      int cnt = index3 + 1;
      int num3 = args2[index3].Check(true, Enum.GetNames(typeof (Position.Type)));
      Vector3 zero = Vector3.get_zero();
      CommandBase.CountAddV3(this.args, ref cnt, ref zero);
      switch ((Position.Type) num3)
      {
        case Position.Type.Add:
          Transform transform = this.scenario.commandController.Objects[index2].get_transform();
          transform.set_position(Vector3.op_Addition(transform.get_position(), zero));
          break;
        case Position.Type.Set:
          this.scenario.commandController.Objects[index2].get_transform().set_position(zero);
          break;
      }
    }

    private enum Type
    {
      Add,
      Set,
    }
  }
}
