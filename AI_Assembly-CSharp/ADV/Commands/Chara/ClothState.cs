// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.ClothState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;

namespace ADV.Commands.Chara
{
  public class ClothState : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "Kind", "State" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          "0",
          ChaFileDefine.ClothesKind.top.ToString(),
          "0"
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
      string str = args2[index2];
      int result;
      if (!int.TryParse(str, out result))
        result = str.Check(true, Enum.GetNames(typeof (ChaFileDefine.ClothesKind)));
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      int num5 = int.Parse(args3[index3]);
      this.scenario.commandController.GetChara(no).chaCtrl.SetClothesState(result, (byte) num5, true);
    }
  }
}
