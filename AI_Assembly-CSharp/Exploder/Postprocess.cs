// Decompiled with JetBrains decompiler
// Type: Exploder.Postprocess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  internal abstract class Postprocess : ExploderTask
  {
    protected Postprocess(Core Core)
      : base(Core)
    {
    }

    public override void Init()
    {
      base.Init();
      this.core.poolIdx = 0;
      if (!Object.op_Implicit((Object) this.core.audioSource))
        return;
      this.core.audioSource.Play();
    }
  }
}
