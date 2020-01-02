// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.FilterImageRelease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Effect
{
  public class FilterImageRelease : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return (string[]) null;
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override void Do()
    {
      base.Do();
      ((Behaviour) this.scenario.FilterImage).set_enabled(false);
      this.scenario.FilterImage.set_sprite((Sprite) null);
    }
  }
}
