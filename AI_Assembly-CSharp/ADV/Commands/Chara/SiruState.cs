// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.SiruState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;

namespace ADV.Commands.Chara
{
  public class SiruState : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "Parts", "State" };
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
      string str1 = args2[index2];
      int result1;
      if (!int.TryParse(str1, out result1))
        result1 = str1.Check(true, Enum.GetNames(typeof (ChaFileDefine.SiruParts)));
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string str2 = args3[index3];
      int result2;
      if (!int.TryParse(str2, out result2))
        result2 = str2.Check(true, "なし", "少ない", "多い");
      this.scenario.commandController.GetChara(no).chaCtrl.SetSiruFlag((ChaFileDefine.SiruParts) result1, (byte) result2);
    }
  }
}
