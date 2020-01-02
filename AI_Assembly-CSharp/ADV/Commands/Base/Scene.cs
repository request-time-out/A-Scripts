// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ADV.Commands.Base
{
  public class Scene : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[7]
        {
          "Bundle",
          "Asset",
          "isLoad",
          "isAsync",
          "isFade",
          "isOverlap",
          "isImageDraw"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[7]
        {
          string.Empty,
          string.Empty,
          bool.FalseString,
          bool.TrueString,
          bool.TrueString,
          bool.FalseString,
          bool.FalseString
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
      string str1 = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string str2 = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      bool flag1 = bool.Parse(args3[index3]);
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      bool flag2 = bool.Parse(args4[index4]);
      string[] args5 = this.args;
      int index5 = num5;
      int num6 = index5 + 1;
      bool flag3 = bool.Parse(args5[index5]);
      string[] args6 = this.args;
      int index6 = num6;
      int num7 = index6 + 1;
      bool flag4 = bool.Parse(args6[index6]);
      string[] args7 = this.args;
      int index7 = num7;
      int num8 = index7 + 1;
      Singleton<Manager.Scene>.Instance.LoadReserve(new Manager.Scene.Data()
      {
        assetBundleName = str1,
        levelName = str2,
        isAdd = !flag1,
        isAsync = flag2,
        isFade = flag3,
        isOverlap = flag4
      }, (bool.Parse(args7[index7]) ? 1 : 0) != 0);
    }
  }
}
