// Decompiled with JetBrains decompiler
// Type: FBSAssist.AudioAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace FBSAssist
{
  public class AudioAssist
  {
    private float[] _samples = new float[1024];
    private float beforeVolume;

    private float RMS(ref float[] samples)
    {
      float num = 0.0f;
      for (int index = 0; index < samples.Length; ++index)
        num += samples[index] * samples[index];
      return Mathf.Sqrt(num / (float) samples.Length);
    }

    public float GetAudioWaveValue(AudioSource audioSource, float correct = 2f)
    {
      float num1 = 0.0f;
      if (!Object.op_Implicit((Object) audioSource.get_clip()) || !audioSource.get_isPlaying())
        return num1;
      float num2 = 1f;
      if (audioSource.get_clip().get_samples() * audioSource.get_clip().get_channels() - audioSource.get_timeSamples() <= 1024)
        return num1;
      audioSource.get_clip().GetData(this._samples, audioSource.get_timeSamples());
      float num3 = Mathf.Clamp(this.RMS(ref this._samples) * correct, 0.0f, num2);
      num1 = (double) num3 >= (double) this.beforeVolume ? (float) (((double) num3 + (double) this.beforeVolume) * 0.5) : (float) ((double) num3 * 0.200000002980232 + (double) this.beforeVolume * 0.800000011920929);
      this.beforeVolume = num3;
      return num1;
    }
  }
}
