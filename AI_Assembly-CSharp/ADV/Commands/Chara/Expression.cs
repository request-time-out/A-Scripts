// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.Expression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace ADV.Commands.Chara
{
  public class Expression : ADV.Commands.Base.Expression
  {
    public override void Do()
    {
      ADV.Commands.Base.Expression.Convert(ref this.args, this.scenario).ForEach((Action<ADV.Commands.Base.Expression.Data>) (p => p.Play(this.scenario)));
    }
  }
}
