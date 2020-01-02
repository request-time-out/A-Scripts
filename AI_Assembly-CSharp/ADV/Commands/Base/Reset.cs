// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Reset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Base
{
  public class Reset : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Type", "Pos", "Ang" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          "0",
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args = this.args;
      int index = num1;
      int num2 = index + 1;
      int num3 = int.Parse(args[index]);
      int cnt1 = 0;
      Vector3 zero1 = Vector3.get_zero();
      int cnt2 = num2;
      int num4 = cnt2 + 1;
      CommandBase.CountAddV3(this.GetArgToSplit(cnt2), ref cnt1, ref zero1);
      Vector3 zero2 = Vector3.get_zero();
      int cnt3 = 0;
      int cnt4 = num4;
      int num5 = cnt4 + 1;
      CommandBase.CountAddV3(this.GetArgToSplit(cnt4), ref cnt3, ref zero2);
      if (num3 != 0)
      {
        if (num3 == 1)
          this.scenario.commandController.CharaRoot.SetPositionAndRotation(zero1, Quaternion.Euler(zero2));
        else
          Debug.LogError((object) ("Reset Type : " + (object) num3));
      }
      else
        this.scenario.commandController.BaseRoot.SetPositionAndRotation(zero1, Quaternion.Euler(zero2));
    }
  }
}
