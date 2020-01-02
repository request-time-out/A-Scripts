// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.FadeWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using Illusion.Extensions;

namespace ADV.Commands.Effect
{
  public class FadeWait : CommandBase
  {
    private FadeWait.Type type;

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
        return new string[1]
        {
          FadeWait.Type.Scene.ToString()
        };
      }
    }

    public override void Do()
    {
      base.Do();
      this.type = Utils.Enum<FadeWait.Type>.Cast(this.args[0].Check(true, Utils.Enum<FadeWait.Type>.Names));
    }

    public override bool Process()
    {
      base.Process();
      return !this.scenario.Fading;
    }

    public override void Result(bool processEnd)
    {
    }

    private enum Type
    {
      Scene,
      Filter,
    }
  }
}
