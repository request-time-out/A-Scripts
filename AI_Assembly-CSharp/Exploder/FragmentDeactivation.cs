// Decompiled with JetBrains decompiler
// Type: Exploder.FragmentDeactivation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Exploder
{
  [Serializable]
  public class FragmentDeactivation
  {
    public float DeactivateTimeout = 10f;
    public DeactivateOptions DeactivateOptions;
    public FadeoutOptions FadeoutOptions;

    public FragmentDeactivation Clone()
    {
      return new FragmentDeactivation()
      {
        DeactivateOptions = this.DeactivateOptions,
        DeactivateTimeout = this.DeactivateTimeout,
        FadeoutOptions = this.FadeoutOptions
      };
    }
  }
}
