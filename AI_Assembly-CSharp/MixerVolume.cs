// Decompiled with JetBrains decompiler
// Type: MixerVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Audio;

public static class MixerVolume
{
  public static void Set(AudioMixer mixer, MixerVolume.Names name, float volume)
  {
    float num = 20f * Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f));
    mixer.SetFloat(name.ToString(), Mathf.Clamp(num, -80f, 0.0f));
  }

  public static float Get(AudioMixer mixer, MixerVolume.Names name)
  {
    float num = 0.0f;
    return !mixer.GetFloat(name.ToString(), ref num) ? 0.0f : Mathf.InverseLerp(-80f, 0.0f, num);
  }

  public enum Names
  {
    MasterVolume,
    BGMVolume,
    PCMVolume,
    ENVVolume,
    GameSEVolume,
    SystemSEVolume,
  }
}
