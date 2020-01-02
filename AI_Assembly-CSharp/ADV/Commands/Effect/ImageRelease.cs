// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.ImageRelease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;

namespace ADV.Commands.Effect
{
  public class ImageRelease : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "Type" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]
        {
          string.Empty,
          bool.TrueString
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
      this.scenario.advScene.AdvFade.ReleaseSprite(args[index].Compare("front", true));
    }
  }
}
