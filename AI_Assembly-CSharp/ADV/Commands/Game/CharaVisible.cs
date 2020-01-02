// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.CharaVisible
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using UnityEngine;

namespace ADV.Commands.Game
{
  public class CharaVisible : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "No",
          "Target",
          "isActive",
          "Stand"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          int.MaxValue.ToString(),
          CharaVisible.Target.All.ToString(),
          bool.TrueString,
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
      CharaData chara = this.scenario.commandController.GetChara(int.Parse(args1[index1]));
      ChaControl chaCtrl = chara.chaCtrl;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int num4 = args2[index2].Check(true, Enum.GetNames(typeof (CharaVisible.Target)));
      string[] args3 = this.args;
      int index3 = num3;
      int num5 = index3 + 1;
      bool flag = bool.Parse(args3[index3]);
      string[] args4 = this.args;
      int index4 = num5;
      int num6 = index4 + 1;
      Action<string> act = (Action<string>) (findName =>
      {
        Transform characterStandNull = this.scenario.commandController.characterStandNulls[findName];
        chara.transform.SetPositionAndRotation(characterStandNull.get_position(), characterStandNull.get_rotation());
      });
      args4.SafeProc(index4, act);
      ChaFileStatus fileStatus = chaCtrl.fileStatus;
      switch (num4)
      {
        case 0:
          chaCtrl.visibleAll = flag;
          break;
        case 1:
          fileStatus.visibleHeadAlways = flag;
          break;
        case 2:
          fileStatus.visibleBodyAlways = flag;
          break;
        case 3:
          fileStatus.visibleSonAlways = flag;
          break;
        case 4:
          fileStatus.visibleGomu = flag;
          break;
      }
    }

    private enum Target
    {
      All,
      Head,
      Body,
      Son,
      Gomu,
    }
  }
}
