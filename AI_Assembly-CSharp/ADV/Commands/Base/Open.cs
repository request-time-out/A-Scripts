// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Open
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Base
{
  public class Open : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Bundle",
          "Asset",
          "isAdd",
          "isClearCheck",
          "isNext"
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
          string.Empty,
          bool.FalseString,
          bool.TrueString,
          bool.TrueString
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
      string str = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string asset = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      bool flag = bool.Parse(args3[index3]);
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      bool isClearCheck = bool.Parse(args4[index4]);
      string[] args5 = this.args;
      int index5 = num5;
      int num6 = index5 + 1;
      bool isNext = bool.Parse(args5[index5]);
      if (str.IsNullOrEmpty())
        str = this.scenario.LoadBundleName;
      if (!AssetBundleCheck.IsFile(str, string.Empty))
        str = Program.ScenarioBundle(str);
      this.scenario.Vars["BundleFile"] = new ValData((object) str);
      this.scenario.Vars["AssetFile"] = new ValData((object) asset);
      this.scenario.LoadFile(str, asset, !flag, isClearCheck, isNext);
    }
  }
}
