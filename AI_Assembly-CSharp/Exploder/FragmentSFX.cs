// Decompiled with JetBrains decompiler
// Type: Exploder.FragmentSFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder
{
  [Serializable]
  public class FragmentSFX
  {
    public int ChanceToPlay = 100;
    public GameObject FragmentEmitter;
    public bool PlayOnlyOnce;
    public bool MixMultipleSounds;
    public int EmitersMax;
    public float ParticleTimeout;

    public FragmentSFX Clone()
    {
      return new FragmentSFX()
      {
        FragmentEmitter = this.FragmentEmitter,
        ChanceToPlay = this.ChanceToPlay,
        PlayOnlyOnce = this.PlayOnlyOnce,
        MixMultipleSounds = this.MixMultipleSounds,
        EmitersMax = this.EmitersMax,
        ParticleTimeout = this.ParticleTimeout
      };
    }
  }
}
