// Decompiled with JetBrains decompiler
// Type: EnviroInterior
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enviro/Interior Zone")]
public class EnviroInterior : MonoBehaviour
{
  public EnviroInterior.ZoneTriggerType zoneTriggerType;
  public bool directLighting;
  public bool ambientLighting;
  public bool weatherAudio;
  public bool ambientAudio;
  public bool fog;
  public bool fogColor;
  public bool skybox;
  public bool weatherEffects;
  public Color directLightingMod;
  public Color ambientLightingMod;
  public Color ambientEQLightingMod;
  public Color ambientGRLightingMod;
  private Color curDirectLightingMod;
  private Color curAmbientLightingMod;
  private Color curAmbientEQLightingMod;
  private Color curAmbientGRLightingMod;
  public float directLightFadeSpeed;
  public float ambientLightFadeSpeed;
  public Color skyboxColorMod;
  private Color curskyboxColorMod;
  public float skyboxFadeSpeed;
  private bool fadeInDirectLight;
  private bool fadeOutDirectLight;
  private bool fadeInAmbientLight;
  private bool fadeOutAmbientLight;
  private bool fadeInSkybox;
  private bool fadeOutSkybox;
  public float ambientVolume;
  public float weatherVolume;
  public AudioClip zoneAudioClip;
  public float zoneAudioVolume;
  public float zoneAudioFadingSpeed;
  public Color fogColorMod;
  private Color curFogColorMod;
  public float fogFadeSpeed;
  public float minFogMod;
  private bool fadeInFog;
  private bool fadeOutFog;
  private bool fadeInFogColor;
  private bool fadeOutFogColor;
  public float weatherFadeSpeed;
  private bool fadeInWeather;
  private bool fadeOutWeather;
  public List<EnviroTrigger> triggers;
  private Color fadeOutColor;

  public EnviroInterior()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  public void CreateNewTrigger()
  {
    GameObject gameObject = new GameObject();
    ((Object) gameObject).set_name("Trigger " + this.triggers.Count.ToString());
    gameObject.get_transform().SetParent(((Component) this).get_transform(), false);
    ((Collider) gameObject.AddComponent<BoxCollider>()).set_isTrigger(true);
    EnviroTrigger enviroTrigger = (EnviroTrigger) gameObject.AddComponent<EnviroTrigger>();
    enviroTrigger.myZone = this;
    ((Object) enviroTrigger).set_name(((Object) gameObject).get_name());
    this.triggers.Add(enviroTrigger);
  }

  public void RemoveTrigger(EnviroTrigger id)
  {
    Object.DestroyImmediate((Object) ((Component) id).get_gameObject());
    this.triggers.Remove(id);
  }

  public void Enter()
  {
    EnviroSky.instance.interiorMode = true;
    EnviroSky.instance.lastInteriorZone = this;
    if (this.directLighting)
    {
      this.fadeOutDirectLight = false;
      this.fadeInDirectLight = true;
    }
    if (this.ambientLighting)
    {
      this.fadeOutAmbientLight = false;
      this.fadeInAmbientLight = true;
    }
    if (this.skybox)
    {
      this.fadeOutSkybox = false;
      this.fadeInSkybox = true;
    }
    if (this.ambientAudio)
      EnviroSky.instance.Audio.ambientSFXVolumeMod = this.ambientVolume;
    if (this.weatherAudio)
      EnviroSky.instance.Audio.weatherSFXVolumeMod = this.weatherVolume;
    if (Object.op_Inequality((Object) this.zoneAudioClip, (Object) null))
    {
      EnviroSky.instance.currentInteriorZoneAudioFadingSpeed = this.zoneAudioFadingSpeed;
      EnviroSky.instance.AudioSourceZone.FadeIn(this.zoneAudioClip);
      EnviroSky.instance.currentInteriorZoneAudioVolume = this.zoneAudioVolume;
    }
    if (this.fog)
    {
      this.fadeOutFog = false;
      this.fadeInFog = true;
    }
    if (this.fogColor)
    {
      this.fadeOutFogColor = false;
      this.fadeInFogColor = true;
    }
    if (!this.weatherEffects)
      return;
    this.fadeOutWeather = false;
    this.fadeInWeather = true;
  }

  public void Exit()
  {
    EnviroSky.instance.interiorMode = false;
    if (this.directLighting)
    {
      this.fadeInDirectLight = false;
      this.fadeOutDirectLight = true;
    }
    if (this.ambientLighting)
    {
      this.fadeOutAmbientLight = true;
      this.fadeInAmbientLight = false;
    }
    if (this.skybox)
    {
      this.fadeOutSkybox = true;
      this.fadeInSkybox = false;
    }
    if (this.ambientAudio)
      EnviroSky.instance.Audio.ambientSFXVolumeMod = 0.0f;
    if (this.weatherAudio)
      EnviroSky.instance.Audio.weatherSFXVolumeMod = 0.0f;
    if (Object.op_Inequality((Object) this.zoneAudioClip, (Object) null))
    {
      EnviroSky.instance.currentInteriorZoneAudioFadingSpeed = this.zoneAudioFadingSpeed;
      EnviroSky.instance.AudioSourceZone.FadeOut();
    }
    if (this.fog)
    {
      this.fadeOutFog = true;
      this.fadeInFog = false;
    }
    if (this.fogColor)
    {
      this.fadeOutFogColor = true;
      this.fadeInFogColor = false;
    }
    if (!this.weatherEffects)
      return;
    this.fadeOutWeather = true;
    this.fadeInWeather = false;
  }

  public void StopAllFading()
  {
    if (this.directLighting)
    {
      this.fadeInDirectLight = false;
      this.fadeOutDirectLight = false;
    }
    if (this.ambientLighting)
    {
      this.fadeOutAmbientLight = false;
      this.fadeInAmbientLight = false;
    }
    if (Object.op_Inequality((Object) this.zoneAudioClip, (Object) null))
      EnviroSky.instance.AudioSourceZone.FadeOut();
    if (this.skybox)
    {
      this.fadeOutSkybox = false;
      this.fadeInSkybox = false;
    }
    if (this.fog)
    {
      this.fadeOutFog = false;
      this.fadeInFog = false;
    }
    if (this.fogColor)
    {
      this.fadeOutFogColor = false;
      this.fadeInFogColor = false;
    }
    if (!this.weatherEffects)
      return;
    this.fadeOutWeather = false;
    this.fadeInWeather = false;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    if (this.directLighting)
    {
      if (this.fadeInDirectLight)
      {
        this.curDirectLightingMod = Color.Lerp(this.curDirectLightingMod, this.directLightingMod, this.directLightFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorDirectLightMod = this.curDirectLightingMod;
        if (Color.op_Equality(this.curDirectLightingMod, this.directLightingMod))
          this.fadeInDirectLight = false;
      }
      else if (this.fadeOutDirectLight)
      {
        this.curDirectLightingMod = Color.Lerp(this.curDirectLightingMod, this.fadeOutColor, this.directLightFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorDirectLightMod = this.curDirectLightingMod;
        if (Color.op_Equality(this.curDirectLightingMod, this.fadeOutColor))
          this.fadeOutDirectLight = false;
      }
    }
    if (this.ambientLighting)
    {
      if (this.fadeInAmbientLight)
      {
        this.curAmbientLightingMod = Color.Lerp(this.curAmbientLightingMod, this.ambientLightingMod, this.ambientLightFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorAmbientLightMod = this.curAmbientLightingMod;
        if (EnviroSky.instance.lightSettings.ambientMode == 1)
        {
          this.curAmbientEQLightingMod = Color.Lerp(this.curAmbientEQLightingMod, this.ambientEQLightingMod, this.ambientLightFadeSpeed * Time.get_deltaTime());
          EnviroSky.instance.currentInteriorAmbientEQLightMod = this.curAmbientEQLightingMod;
          this.curAmbientGRLightingMod = Color.Lerp(this.curAmbientGRLightingMod, this.ambientGRLightingMod, this.ambientLightFadeSpeed * Time.get_deltaTime());
          EnviroSky.instance.currentInteriorAmbientGRLightMod = this.curAmbientGRLightingMod;
        }
        if (Color.op_Equality(this.curAmbientLightingMod, this.ambientLightingMod))
          this.fadeInAmbientLight = false;
      }
      else if (this.fadeOutAmbientLight)
      {
        this.curAmbientLightingMod = Color.Lerp(this.curAmbientLightingMod, this.fadeOutColor, 2f * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorAmbientLightMod = this.curAmbientLightingMod;
        if (EnviroSky.instance.lightSettings.ambientMode == 1)
        {
          this.curAmbientEQLightingMod = Color.Lerp(this.curAmbientEQLightingMod, this.fadeOutColor, 2f * Time.get_deltaTime());
          EnviroSky.instance.currentInteriorAmbientEQLightMod = this.curAmbientEQLightingMod;
          this.curAmbientGRLightingMod = Color.Lerp(this.curAmbientGRLightingMod, this.fadeOutColor, 2f * Time.get_deltaTime());
          EnviroSky.instance.currentInteriorAmbientGRLightMod = this.curAmbientGRLightingMod;
        }
        if (Color.op_Equality(this.curAmbientLightingMod, this.fadeOutColor))
          this.fadeOutAmbientLight = false;
      }
    }
    if (this.skybox)
    {
      if (this.fadeInSkybox)
      {
        this.curskyboxColorMod = Color.Lerp(this.curskyboxColorMod, this.skyboxColorMod, this.skyboxFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorSkyboxMod = this.curskyboxColorMod;
        if (Color.op_Equality(this.curskyboxColorMod, this.skyboxColorMod))
          this.fadeInSkybox = false;
      }
      else if (this.fadeOutSkybox)
      {
        this.curskyboxColorMod = Color.Lerp(this.curskyboxColorMod, this.fadeOutColor, this.skyboxFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorSkyboxMod = this.curskyboxColorMod;
        if (Color.op_Equality(this.curskyboxColorMod, this.fadeOutColor))
          this.fadeOutSkybox = false;
      }
    }
    if (this.fog)
    {
      if (this.fadeInFog)
      {
        EnviroSky.instance.currentInteriorFogMod = Mathf.Lerp(EnviroSky.instance.currentInteriorFogMod, this.minFogMod, this.fogFadeSpeed * Time.get_deltaTime());
        if ((double) EnviroSky.instance.currentInteriorFogMod <= (double) this.minFogMod + 0.001)
          this.fadeInFog = false;
      }
      else if (this.fadeOutFog)
      {
        EnviroSky.instance.currentInteriorFogMod = Mathf.Lerp(EnviroSky.instance.currentInteriorFogMod, 1f, this.fogFadeSpeed * 2f * Time.get_deltaTime());
        if ((double) EnviroSky.instance.currentInteriorFogMod >= 0.999)
          this.fadeOutFog = false;
      }
    }
    if (this.fogColor)
    {
      if (this.fadeInFogColor)
      {
        this.curFogColorMod = Color.Lerp(this.curFogColorMod, this.fogColorMod, this.fogFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorFogColorMod = this.curFogColorMod;
        if (Color.op_Equality(this.curFogColorMod, this.fogColorMod))
          this.fadeInFogColor = false;
      }
      else if (this.fadeOutFogColor)
      {
        this.curFogColorMod = Color.Lerp(this.curFogColorMod, this.fadeOutColor, this.fogFadeSpeed * Time.get_deltaTime());
        EnviroSky.instance.currentInteriorFogColorMod = this.curFogColorMod;
        if (Color.op_Equality(this.curFogColorMod, this.fadeOutColor))
          this.fadeOutFogColor = false;
      }
    }
    if (!this.weatherEffects)
      return;
    if (this.fadeInWeather)
    {
      EnviroSky.instance.currentInteriorWeatherEffectMod = Mathf.Lerp(EnviroSky.instance.currentInteriorWeatherEffectMod, 0.0f, this.weatherFadeSpeed * Time.get_deltaTime());
      if ((double) EnviroSky.instance.currentInteriorWeatherEffectMod > 0.001)
        return;
      this.fadeInWeather = false;
    }
    else
    {
      if (!this.fadeOutWeather)
        return;
      EnviroSky.instance.currentInteriorWeatherEffectMod = Mathf.Lerp(EnviroSky.instance.currentInteriorWeatherEffectMod, 1f, this.weatherFadeSpeed * 2f * Time.get_deltaTime());
      if ((double) EnviroSky.instance.currentInteriorWeatherEffectMod < 0.999)
        return;
      this.fadeOutWeather = false;
    }
  }

  public enum ZoneTriggerType
  {
    Entry_Exit,
    Zone,
  }
}
