// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.ItemAnime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class ItemAnime : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "No",
          "ItemNo",
          "Bundle",
          "Asset",
          "State"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          int.MaxValue.ToString(),
          "0",
          string.Empty,
          string.Empty,
          "Idle"
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
      int index3 = int.Parse(args2[index2]);
      string[] args3 = this.args;
      int index4 = num3;
      int num4 = index4 + 1;
      string bundle = args3[index4];
      string[] args4 = this.args;
      int index5 = num4;
      int num5 = index5 + 1;
      string asset = args4[index5];
      string[] args5 = this.args;
      int index6 = num5;
      int num6 = index6 + 1;
      string state = args5[index6];
      this.scenario.commandController.GetChara(no).itemDic[index3].LoadAnimator(bundle, asset, state);
    }
  }
}
