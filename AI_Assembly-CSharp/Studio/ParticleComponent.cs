// Decompiled with JetBrains decompiler
// Type: Studio.ParticleComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class ParticleComponent : MonoBehaviour
  {
    [Header("カラー１反映対象")]
    public ParticleSystem[] particleColor1;
    public Color defColor01;
    [Header("読み込まれたタイミングで再生する")]
    public bool playOnLoad;
    public ParticleSystem[] particlesPlay;

    public ParticleComponent()
    {
      base.\u002Ector();
    }

    public bool[] UseColor
    {
      get
      {
        return new bool[3]{ this.UseColor1, false, false };
      }
    }

    public bool UseColor1
    {
      get
      {
        return !((IList<ParticleSystem>) this.particleColor1).IsNullOrEmpty<ParticleSystem>();
      }
    }

    public bool check
    {
      get
      {
        return !((IList<ParticleSystem>) this.particleColor1).IsNullOrEmpty<ParticleSystem>();
      }
    }

    public void UpdateColor(OIItemInfo _info)
    {
      if (((IList<ParticleSystem>) this.particleColor1).IsNullOrEmpty<ParticleSystem>())
        return;
      foreach (ParticleSystem particleSystem in this.particleColor1)
      {
        ParticleSystem.MainModule main = particleSystem.get_main();
        ((ParticleSystem.MainModule) ref main).set_startColor(ParticleSystem.MinMaxGradient.op_Implicit(_info.colors[0].mainColor));
      }
    }

    public void PlayOnLoad()
    {
      if (!this.playOnLoad || ((IList<ParticleSystem>) this.particlesPlay).IsNullOrEmpty<ParticleSystem>())
        return;
      using (IEnumerator<ParticleSystem> enumerator = ((IEnumerable<ParticleSystem>) this.particlesPlay).Where<ParticleSystem>((Func<ParticleSystem, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
          enumerator.Current.Play();
      }
    }

    public void SetColor()
    {
      if (((IList<ParticleSystem>) this.particleColor1).IsNullOrEmpty<ParticleSystem>())
        return;
      ParticleSystem.MainModule main = this.particleColor1[0].get_main();
      ParticleSystem.MinMaxGradient startColor = ((ParticleSystem.MainModule) ref main).get_startColor();
      this.defColor01 = ((ParticleSystem.MinMaxGradient) ref startColor).get_color();
    }
  }
}
