// Decompiled with JetBrains decompiler
// Type: EnviroQualitySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroQualitySettings
{
  [Range(0.0f, 1f)]
  [Tooltip("Modifies the amount of particles used in weather effects.")]
  public float GlobalParticleEmissionRates = 1f;
  [Tooltip("How often Enviro Growth Instances should be updated. Lower value = smoother growth and more frequent updates but more perfomance hungry!")]
  public float UpdateInterval = 0.5f;
}
