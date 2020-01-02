// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.ImageLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;

namespace ADV.Commands.Effect
{
  public class ImageLoad : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Bundle", "Asset", "Type" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          string.Empty,
          string.Empty,
          string.Empty,
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
      string bundleName = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string assetName = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      this.scenario.advScene.AdvFade.LoadSprite(!args3[index3].Compare("back", true), bundleName, assetName);
    }
  }
}
