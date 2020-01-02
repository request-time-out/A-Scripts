// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.LookEyesTargetChara
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class LookEyesTargetChara : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "TargetNo", "Key" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          int.MaxValue.ToString(),
          "0",
          string.Empty
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      CommandController commandController1 = this.scenario.commandController;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      int no1 = int.Parse(args1[index1]);
      CharaData chara1 = commandController1.GetChara(no1);
      CommandController commandController2 = this.scenario.commandController;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int no2 = int.Parse(args2[index2]);
      CharaData chara2 = commandController2.GetChara(no2);
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string str = args3[index3];
      int result;
      if (!int.TryParse(str, out result))
        result = str.Check(true, Enum.GetNames(typeof (ChaReference.RefObjKey)));
      GameObject referenceInfo = chara2.chaCtrl.GetReferenceInfo((ChaReference.RefObjKey) result);
      chara1.chaCtrl.ChangeLookEyesTarget(-1, referenceInfo.get_transform(), 0.5f, 0.0f, 1f, 2f);
    }
  }
}
