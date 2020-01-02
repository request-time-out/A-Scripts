// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.LoopingAudioSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DigitalRuby.RainMaker
{
  public class LoopingAudioSource
  {
    public LoopingAudioSource(MonoBehaviour script, AudioClip clip)
    {
      this.AudioSource = (AudioSource) ((Component) script).get_gameObject().AddComponent<AudioSource>();
      this.AudioSource.set_loop(true);
      this.AudioSource.set_clip(clip);
      this.AudioSource.set_playOnAwake(false);
      this.AudioSource.set_volume(0.0f);
      this.AudioSource.Stop();
      this.TargetVolume = 1f;
    }

    public AudioSource AudioSource { get; private set; }

    public float TargetVolume { get; private set; }

    public void Play(float targetVolume)
    {
      if (!this.AudioSource.get_isPlaying())
      {
        this.AudioSource.set_volume(0.0f);
        this.AudioSource.Play();
      }
      this.TargetVolume = targetVolume;
    }

    public void Stop()
    {
      this.TargetVolume = 0.0f;
    }

    public void Update()
    {
      if (!this.AudioSource.get_isPlaying())
        return;
      float num = Mathf.Lerp(this.AudioSource.get_volume(), this.TargetVolume, Time.get_deltaTime());
      this.AudioSource.set_volume(num);
      if ((double) num != 0.0)
        return;
      this.AudioSource.Stop();
    }
  }
}
