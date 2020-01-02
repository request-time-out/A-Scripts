// Decompiled with JetBrains decompiler
// Type: EnviroAudioSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroAudioSource : MonoBehaviour
{
  public EnviroAudioSource.AudioSourceFunction myFunction;
  public AudioSource audiosrc;
  public bool isFadingIn;
  public bool isFadingOut;
  private float currentAmbientVolume;
  private float currentWeatherVolume;
  private float currentZoneVolume;

  public EnviroAudioSource()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.audiosrc, (Object) null))
      this.audiosrc = (AudioSource) ((Component) this).GetComponent<AudioSource>();
    if (this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather1 || this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather2)
    {
      this.audiosrc.set_loop(true);
      this.audiosrc.set_volume(0.0f);
    }
    this.currentAmbientVolume = EnviroSky.instance.Audio.ambientSFXVolume;
    this.currentWeatherVolume = EnviroSky.instance.Audio.weatherSFXVolume;
  }

  public void FadeOut()
  {
    this.isFadingOut = true;
    this.isFadingIn = false;
  }

  public void FadeIn(AudioClip clip)
  {
    this.isFadingIn = true;
    this.isFadingOut = false;
    this.audiosrc.set_clip(clip);
    this.audiosrc.Play();
  }

  private void Update()
  {
    if (!EnviroSky.instance.started || Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    this.currentAmbientVolume = Mathf.Lerp(this.currentAmbientVolume, EnviroSky.instance.Audio.ambientSFXVolume + EnviroSky.instance.Audio.ambientSFXVolumeMod, 10f * Time.get_deltaTime());
    this.currentWeatherVolume = Mathf.Lerp(this.currentWeatherVolume, EnviroSky.instance.Audio.weatherSFXVolume + EnviroSky.instance.Audio.weatherSFXVolumeMod, 10f * Time.get_deltaTime());
    if (this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather1 || this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather2 || this.myFunction == EnviroAudioSource.AudioSourceFunction.Thunder)
    {
      if (this.isFadingIn && (double) this.audiosrc.get_volume() < (double) this.currentWeatherVolume)
      {
        AudioSource audiosrc = this.audiosrc;
        audiosrc.set_volume(audiosrc.get_volume() + EnviroSky.instance.weatherSettings.audioTransitionSpeed * Time.get_deltaTime());
      }
      else if (this.isFadingIn && (double) this.audiosrc.get_volume() >= (double) this.currentWeatherVolume - 0.00999999977648258)
        this.isFadingIn = false;
      if (this.isFadingOut && (double) this.audiosrc.get_volume() > 0.0)
      {
        AudioSource audiosrc = this.audiosrc;
        audiosrc.set_volume(audiosrc.get_volume() - EnviroSky.instance.weatherSettings.audioTransitionSpeed * Time.get_deltaTime());
      }
      else if (this.isFadingOut && (double) this.audiosrc.get_volume() == 0.0)
      {
        this.audiosrc.Stop();
        this.isFadingOut = false;
      }
      if (!this.audiosrc.get_isPlaying() || this.isFadingOut || this.isFadingIn)
        return;
      this.audiosrc.set_volume(this.currentWeatherVolume);
    }
    else if (this.myFunction == EnviroAudioSource.AudioSourceFunction.Ambient || this.myFunction == EnviroAudioSource.AudioSourceFunction.Ambient2)
    {
      if (this.isFadingIn && (double) this.audiosrc.get_volume() < (double) this.currentAmbientVolume)
      {
        AudioSource audiosrc = this.audiosrc;
        audiosrc.set_volume(audiosrc.get_volume() + EnviroSky.instance.weatherSettings.audioTransitionSpeed * Time.get_deltaTime());
      }
      else if (this.isFadingIn && (double) this.audiosrc.get_volume() >= (double) this.currentAmbientVolume - 0.00999999977648258)
        this.isFadingIn = false;
      if (this.isFadingOut && (double) this.audiosrc.get_volume() > 0.0)
      {
        AudioSource audiosrc = this.audiosrc;
        audiosrc.set_volume(audiosrc.get_volume() - EnviroSky.instance.weatherSettings.audioTransitionSpeed * Time.get_deltaTime());
      }
      else if (this.isFadingOut && (double) this.audiosrc.get_volume() == 0.0)
      {
        this.audiosrc.Stop();
        this.isFadingOut = false;
      }
      if (!this.audiosrc.get_isPlaying() || this.isFadingOut || this.isFadingIn)
        return;
      this.audiosrc.set_volume(this.currentAmbientVolume);
    }
    else
    {
      if (this.myFunction != EnviroAudioSource.AudioSourceFunction.ZoneAmbient)
        return;
      if (this.isFadingIn && (double) this.audiosrc.get_volume() < (double) EnviroSky.instance.currentInteriorZoneAudioVolume)
      {
        AudioSource audiosrc = this.audiosrc;
        audiosrc.set_volume(audiosrc.get_volume() + EnviroSky.instance.currentInteriorZoneAudioFadingSpeed * Time.get_deltaTime());
      }
      else if (this.isFadingIn && (double) this.audiosrc.get_volume() >= (double) EnviroSky.instance.currentInteriorZoneAudioVolume - 0.00999999977648258)
        this.isFadingIn = false;
      if (this.isFadingOut && (double) this.audiosrc.get_volume() > 0.0)
      {
        AudioSource audiosrc = this.audiosrc;
        audiosrc.set_volume(audiosrc.get_volume() - EnviroSky.instance.currentInteriorZoneAudioFadingSpeed * Time.get_deltaTime());
      }
      else if (this.isFadingOut && (double) this.audiosrc.get_volume() == 0.0)
      {
        this.audiosrc.Stop();
        this.isFadingOut = false;
      }
      if (!this.audiosrc.get_isPlaying() || this.isFadingOut || this.isFadingIn)
        return;
      this.audiosrc.set_volume(EnviroSky.instance.currentInteriorZoneAudioVolume);
    }
  }

  public enum AudioSourceFunction
  {
    Weather1,
    Weather2,
    Ambient,
    Ambient2,
    Thunder,
    ZoneAmbient,
  }
}
