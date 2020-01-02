// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.LookNeckTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class LookNeckTarget : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[7]
        {
          "No",
          "Type",
          "Target",
          "Rate",
          "Deg",
          "Range",
          "Dis"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[7]
        {
          int.MaxValue.ToString(),
          "0",
          string.Empty,
          string.Empty,
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
      int no = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int targetType = int.Parse(args2[index2]);
      CharaData chara = this.scenario.commandController.GetChara(no);
      GameObject obj = (GameObject) null;
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      Action<string> act1 = (Action<string>) (s => obj = GameObject.Find(s));
      args3.SafeProc(index3, act1);
      Transform trfTarg = !Object.op_Equality((Object) obj, (Object) null) ? obj.get_transform() : (Transform) null;
      float rate = 0.5f;
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      Action<string> act2 = (Action<string>) (s => rate = float.Parse(s));
      args4.SafeProc(index4, act2);
      float rotDeg = 0.0f;
      string[] args5 = this.args;
      int index5 = num5;
      int num6 = index5 + 1;
      Action<string> act3 = (Action<string>) (s => rotDeg = float.Parse(s));
      args5.SafeProc(index5, act3);
      float range = 1f;
      string[] args6 = this.args;
      int index6 = num6;
      int num7 = index6 + 1;
      Action<string> act4 = (Action<string>) (s => range = float.Parse(s));
      args6.SafeProc(index6, act4);
      float dis = 0.8f;
      string[] args7 = this.args;
      int index7 = num7;
      int num8 = index7 + 1;
      Action<string> act5 = (Action<string>) (s => dis = float.Parse(s));
      args7.SafeProc(index7, act5);
      chara.chaCtrl.ChangeLookNeckTarget(targetType, trfTarg, rate, rotDeg, range, dis);
    }
  }
}
