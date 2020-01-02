// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.Motion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV.Commands.Chara
{
  public class Motion : ADV.Commands.Base.Motion
  {
    public override void Do()
    {
      List<ADV.Commands.Base.Motion.Data> source = ADV.Commands.Base.Motion.Convert(ref this.args, this.scenario, this.ArgsLabel.Length);
      if (source.Any<ADV.Commands.Base.Motion.Data>())
        this.scenario.CrossFadeStart();
      source.ForEach((Action<ADV.Commands.Base.Motion.Data>) (p => p.Play(this.scenario)));
    }
  }
}
