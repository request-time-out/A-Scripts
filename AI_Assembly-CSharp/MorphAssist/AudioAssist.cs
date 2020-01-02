// Decompiled with JetBrains decompiler
// Type: MorphAssist.AudioAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace MorphAssist
{
  public class AudioAssist
  {
    private float beforeVolume;

    private float RMS(float[] samples)
    {
      float num = 0.0f;
      for (int index = 0; index < samples.Length; ++index)
        num += samples[index] * samples[index];
      return Mathf.Sqrt(num / (float) samples.Length);
    }

    public float GetAudioWaveValue(AudioSource audioSource)
    {
      float num1 = 0.0f;
      if (!Object.op_Implicit((Object) audioSource.get_clip()) || !audioSource.get_isPlaying())
        return num1;
      float[] samples = new float[256];
      float num2 = 1f;
      audioSource.GetSpectrumData(samples, 0, (FFTWindow) 5);
      num1 = (float) (((double) Mathf.Clamp(this.RMS(samples) * 50f, 0.0f, num2) + (double) this.beforeVolume) * 0.5);
      this.beforeVolume = num1;
      return num1;
    }
  }
}
