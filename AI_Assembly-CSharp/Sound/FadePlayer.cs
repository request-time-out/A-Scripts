// Decompiled with JetBrains decompiler
// Type: Sound.FadePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Sound
{
  [RequireComponent(typeof (AudioSource))]
  public class FadePlayer : MonoBehaviour
  {
    public float currentVolume;
    private float fadeInTime;
    private float fadeOutTime;

    public FadePlayer()
    {
      base.\u002Ector();
    }

    public FadePlayer.State nowState { get; private set; }

    public AudioSource audioSource { get; private set; }

    private void Awake()
    {
      this.audioSource = (AudioSource) ((Component) this).GetComponent<AudioSource>();
      this.nowState = (FadePlayer.State) new FadePlayer.Wait(this);
    }

    private void Update()
    {
      this.nowState.Update();
      if (!(this.nowState is FadePlayer.Wait) || this.audioSource.get_isPlaying())
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }

    public void Play(float fadeTime = 0.0f)
    {
      this.fadeInTime = fadeTime;
      this.nowState.Play();
    }

    public void Pause()
    {
      this.nowState.Pause();
    }

    public void Stop(float fadeTime)
    {
      this.fadeOutTime = fadeTime;
      this.nowState.Stop();
    }

    public abstract class State
    {
      protected FadePlayer player;

      public State(FadePlayer player)
      {
        this.player = player;
      }

      public virtual void Play()
      {
      }

      public virtual void Pause()
      {
      }

      public virtual void Stop()
      {
      }

      public virtual bool Update()
      {
        return true;
      }
    }

    public class Wait : FadePlayer.State
    {
      public Wait(FadePlayer player)
        : base(player)
      {
      }

      public override void Play()
      {
        if ((double) this.player.fadeInTime > 0.0)
          this.player.nowState = (FadePlayer.State) new FadePlayer.FadeIn(this.player);
        else
          this.player.nowState = (FadePlayer.State) new FadePlayer.Playing(this.player);
      }
    }

    public class FadeIn : FadePlayer.State
    {
      private float t;

      public FadeIn(FadePlayer player)
        : base(player)
      {
        player.audioSource.Play();
        player.audioSource.set_volume(0.0f);
      }

      public override void Pause()
      {
        this.player.nowState = (FadePlayer.State) new FadePlayer.Paused(this.player);
      }

      public override void Stop()
      {
        this.player.nowState = (FadePlayer.State) new FadePlayer.FadeOut(this.player);
      }

      public override bool Update()
      {
        this.t += Time.get_deltaTime();
        this.player.audioSource.set_volume(Mathf.Lerp(0.0f, this.player.currentVolume, this.t / this.player.fadeInTime));
        if ((double) this.t < (double) this.player.fadeInTime)
          return false;
        this.player.nowState = (FadePlayer.State) new FadePlayer.Playing(this.player);
        return true;
      }
    }

    public class Playing : FadePlayer.State
    {
      public Playing(FadePlayer player)
        : base(player)
      {
        if (player.audioSource.get_isPlaying())
          return;
        player.audioSource.Play();
      }

      public override void Pause()
      {
        this.player.nowState = (FadePlayer.State) new FadePlayer.Paused(this.player);
      }

      public override void Stop()
      {
        this.player.nowState = (FadePlayer.State) new FadePlayer.FadeOut(this.player);
      }

      public override bool Update()
      {
        this.player.audioSource.set_volume(this.player.currentVolume);
        return false;
      }
    }

    public class Paused : FadePlayer.State
    {
      private FadePlayer.State preState;

      public Paused(FadePlayer player)
        : base(player)
      {
        this.preState = player.nowState;
        player.audioSource.Pause();
      }

      public override void Stop()
      {
        this.player.audioSource.Stop();
        this.player.nowState = (FadePlayer.State) new FadePlayer.Wait(this.player);
      }

      public override void Play()
      {
        this.player.nowState = this.preState;
        this.player.audioSource.Play();
      }
    }

    public class FadeOut : FadePlayer.State
    {
      private float t;

      public FadeOut(FadePlayer player)
        : base(player)
      {
        player.currentVolume = player.audioSource.get_volume();
      }

      public override void Pause()
      {
        this.player.nowState = (FadePlayer.State) new FadePlayer.Paused(this.player);
      }

      public override bool Update()
      {
        this.t += Time.get_deltaTime();
        this.player.audioSource.set_volume(this.player.currentVolume * (float) (1.0 - (double) this.t / (double) this.player.fadeOutTime));
        if ((double) this.t < (double) this.player.fadeOutTime)
          return false;
        this.player.audioSource.set_volume(0.0f);
        this.player.audioSource.Stop();
        this.player.nowState = (FadePlayer.State) new FadePlayer.Wait(this.player);
        return true;
      }
    }
  }
}
