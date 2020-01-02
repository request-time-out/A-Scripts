// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.BaseRainScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DigitalRuby.RainMaker
{
  public class BaseRainScript : MonoBehaviour
  {
    [Tooltip("Camera the rain should hover over, defaults to main camera")]
    public Camera Camera;
    [Tooltip("Whether rain should follow the camera. If false, rain must be moved manually and will not follow the camera.")]
    public bool FollowCamera;
    [Tooltip("Light rain looping clip")]
    public AudioClip RainSoundLight;
    [Tooltip("Medium rain looping clip")]
    public AudioClip RainSoundMedium;
    [Tooltip("Heavy rain looping clip")]
    public AudioClip RainSoundHeavy;
    [Tooltip("Intensity of rain (0-1)")]
    [Range(0.0f, 1f)]
    public float RainIntensity;
    [Tooltip("Rain particle system")]
    public ParticleSystem RainFallParticleSystem;
    [Tooltip("Particles system for when rain hits something")]
    public ParticleSystem RainExplosionParticleSystem;
    [Tooltip("Particle system to use for rain mist")]
    public ParticleSystem RainMistParticleSystem;
    [Tooltip("The threshold for intensity (0 - 1) at which mist starts to appear")]
    [Range(0.0f, 1f)]
    public float RainMistThreshold;
    [Tooltip("Wind looping clip")]
    public AudioClip WindSound;
    [Tooltip("Wind sound volume modifier, use this to lower your sound if it's too loud.")]
    public float WindSoundVolumeModifier;
    [Tooltip("Wind zone that will affect and follow the rain")]
    public WindZone WindZone;
    [Tooltip("X = minimum wind speed. Y = maximum wind speed. Z = sound multiplier. Wind speed is divided by Z to get sound multiplier value. Set Z to lower than Y to increase wind sound volume, or higher to decrease wind sound volume.")]
    public Vector3 WindSpeedRange;
    [Tooltip("How often the wind speed and direction changes (minimum and maximum change interval in seconds)")]
    public Vector2 WindChangeInterval;
    [Tooltip("Whether wind should be enabled.")]
    public bool EnableWind;
    protected LoopingAudioSource audioSourceRainLight;
    protected LoopingAudioSource audioSourceRainMedium;
    protected LoopingAudioSource audioSourceRainHeavy;
    protected LoopingAudioSource audioSourceRainCurrent;
    protected LoopingAudioSource audioSourceWind;
    protected Material rainMaterial;
    protected Material rainExplosionMaterial;
    protected Material rainMistMaterial;
    private float lastRainIntensityValue;
    private float nextWindTime;

    public BaseRainScript()
    {
      base.\u002Ector();
    }

    private void UpdateWind()
    {
      if (this.EnableWind)
      {
        if (Object.op_Inequality((Object) this.WindZone, (Object) null))
        {
          ((Component) this.WindZone).get_gameObject().SetActive(true);
          if (this.FollowCamera)
            ((Component) this.WindZone).get_transform().set_position(((Component) this.Camera).get_transform().get_position());
          if (!this.Camera.get_orthographic())
            ((Component) this.WindZone).get_transform().Translate(0.0f, this.WindZone.get_radius(), 0.0f);
          if ((double) this.nextWindTime < (double) Time.get_time())
          {
            this.WindZone.set_windMain(Random.Range((float) this.WindSpeedRange.x, (float) this.WindSpeedRange.y));
            this.WindZone.set_windTurbulence(Random.Range((float) this.WindSpeedRange.x, (float) this.WindSpeedRange.y));
            if (this.Camera.get_orthographic())
              ((Component) this.WindZone).get_transform().set_rotation(Quaternion.Euler(0.0f, Random.Range(0, 2) != 0 ? -90f : 90f, 0.0f));
            else
              ((Component) this.WindZone).get_transform().set_rotation(Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0.0f, 360f), 0.0f));
            this.nextWindTime = Time.get_time() + Random.Range((float) this.WindChangeInterval.x, (float) this.WindChangeInterval.y);
            this.audioSourceWind.Play(this.WindZone.get_windMain() / (float) this.WindSpeedRange.z * this.WindSoundVolumeModifier);
          }
        }
      }
      else
      {
        if (Object.op_Inequality((Object) this.WindZone, (Object) null))
          ((Component) this.WindZone).get_gameObject().SetActive(false);
        this.audioSourceWind.Stop();
      }
      this.audioSourceWind.Update();
    }

    private void CheckForRainChange()
    {
      if ((double) this.lastRainIntensityValue == (double) this.RainIntensity)
        return;
      this.lastRainIntensityValue = this.RainIntensity;
      if ((double) this.RainIntensity <= 0.00999999977648258)
      {
        if (this.audioSourceRainCurrent != null)
        {
          this.audioSourceRainCurrent.Stop();
          this.audioSourceRainCurrent = (LoopingAudioSource) null;
        }
        if (Object.op_Inequality((Object) this.RainFallParticleSystem, (Object) null))
        {
          ParticleSystem.EmissionModule emission = this.RainFallParticleSystem.get_emission();
          ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
          this.RainFallParticleSystem.Stop();
        }
        if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
          return;
        ParticleSystem.EmissionModule emission1 = this.RainMistParticleSystem.get_emission();
        ((ParticleSystem.EmissionModule) ref emission1).set_enabled(false);
        this.RainMistParticleSystem.Stop();
      }
      else
      {
        LoopingAudioSource loopingAudioSource = (double) this.RainIntensity < 0.670000016689301 ? ((double) this.RainIntensity < 0.330000013113022 ? this.audioSourceRainLight : this.audioSourceRainMedium) : this.audioSourceRainHeavy;
        if (this.audioSourceRainCurrent != loopingAudioSource)
        {
          if (this.audioSourceRainCurrent != null)
            this.audioSourceRainCurrent.Stop();
          this.audioSourceRainCurrent = loopingAudioSource;
          this.audioSourceRainCurrent.Play(1f);
        }
        if (Object.op_Inequality((Object) this.RainFallParticleSystem, (Object) null))
        {
          ParticleSystem.EmissionModule emission = this.RainFallParticleSystem.get_emission();
          ref ParticleSystem.EmissionModule local1 = ref emission;
          bool flag = true;
          ((Renderer) ((Component) this.RainFallParticleSystem).GetComponent<Renderer>()).set_enabled(flag);
          int num1 = flag ? 1 : 0;
          ((ParticleSystem.EmissionModule) ref local1).set_enabled(num1 != 0);
          if (!this.RainFallParticleSystem.get_isPlaying())
            this.RainFallParticleSystem.Play();
          ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
          ((ParticleSystem.MinMaxCurve) ref rateOverTime).set_mode((ParticleSystemCurveMode) 0);
          ref ParticleSystem.MinMaxCurve local2 = ref rateOverTime;
          float num2 = this.RainFallEmissionRate();
          ((ParticleSystem.MinMaxCurve) ref rateOverTime).set_constantMax(num2);
          double num3 = (double) num2;
          ((ParticleSystem.MinMaxCurve) ref local2).set_constantMin((float) num3);
          ((ParticleSystem.EmissionModule) ref emission).set_rateOverTime(rateOverTime);
        }
        if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
          return;
        ParticleSystem.EmissionModule emission1 = this.RainMistParticleSystem.get_emission();
        ref ParticleSystem.EmissionModule local3 = ref emission1;
        bool flag1 = true;
        ((Renderer) ((Component) this.RainMistParticleSystem).GetComponent<Renderer>()).set_enabled(flag1);
        int num4 = flag1 ? 1 : 0;
        ((ParticleSystem.EmissionModule) ref local3).set_enabled(num4 != 0);
        if (!this.RainMistParticleSystem.get_isPlaying())
          this.RainMistParticleSystem.Play();
        float num5 = (double) this.RainIntensity >= (double) this.RainMistThreshold ? this.MistEmissionRate() : 0.0f;
        ParticleSystem.MinMaxCurve rateOverTime1 = ((ParticleSystem.EmissionModule) ref emission1).get_rateOverTime();
        ((ParticleSystem.MinMaxCurve) ref rateOverTime1).set_mode((ParticleSystemCurveMode) 0);
        ref ParticleSystem.MinMaxCurve local4 = ref rateOverTime1;
        float num6 = num5;
        ((ParticleSystem.MinMaxCurve) ref rateOverTime1).set_constantMax(num6);
        double num7 = (double) num6;
        ((ParticleSystem.MinMaxCurve) ref local4).set_constantMin((float) num7);
        ((ParticleSystem.EmissionModule) ref emission1).set_rateOverTime(rateOverTime1);
      }
    }

    protected virtual void Start()
    {
      if (Object.op_Equality((Object) this.Camera, (Object) null))
        this.Camera = Camera.get_main();
      this.audioSourceRainLight = new LoopingAudioSource((MonoBehaviour) this, this.RainSoundLight);
      this.audioSourceRainMedium = new LoopingAudioSource((MonoBehaviour) this, this.RainSoundMedium);
      this.audioSourceRainHeavy = new LoopingAudioSource((MonoBehaviour) this, this.RainSoundHeavy);
      this.audioSourceWind = new LoopingAudioSource((MonoBehaviour) this, this.WindSound);
      if (Object.op_Inequality((Object) this.RainFallParticleSystem, (Object) null))
      {
        ParticleSystem.EmissionModule emission = this.RainFallParticleSystem.get_emission();
        ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
        Renderer component = (Renderer) ((Component) this.RainFallParticleSystem).GetComponent<Renderer>();
        component.set_enabled(false);
        this.rainMaterial = new Material(component.get_material());
        this.rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
        component.set_material(this.rainMaterial);
      }
      if (Object.op_Inequality((Object) this.RainExplosionParticleSystem, (Object) null))
      {
        ParticleSystem.EmissionModule emission = this.RainExplosionParticleSystem.get_emission();
        ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
        Renderer component = (Renderer) ((Component) this.RainExplosionParticleSystem).GetComponent<Renderer>();
        this.rainExplosionMaterial = new Material(component.get_material());
        this.rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
        component.set_material(this.rainExplosionMaterial);
      }
      if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
        return;
      ParticleSystem.EmissionModule emission1 = this.RainMistParticleSystem.get_emission();
      ((ParticleSystem.EmissionModule) ref emission1).set_enabled(false);
      Renderer component1 = (Renderer) ((Component) this.RainMistParticleSystem).GetComponent<Renderer>();
      component1.set_enabled(false);
      this.rainMistMaterial = new Material(component1.get_material());
      if (this.UseRainMistSoftParticles)
        this.rainMistMaterial.EnableKeyword("SOFTPARTICLES_ON");
      else
        this.rainMistMaterial.EnableKeyword("SOFTPARTICLES_OFF");
      component1.set_material(this.rainMistMaterial);
    }

    protected virtual void Update()
    {
      this.CheckForRainChange();
      this.UpdateWind();
      this.audioSourceRainLight.Update();
      this.audioSourceRainMedium.Update();
      this.audioSourceRainHeavy.Update();
    }

    protected virtual float RainFallEmissionRate()
    {
      ParticleSystem.MainModule main1 = this.RainFallParticleSystem.get_main();
      double maxParticles = (double) ((ParticleSystem.MainModule) ref main1).get_maxParticles();
      ParticleSystem.MainModule main2 = this.RainFallParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startLifetime = ((ParticleSystem.MainModule) ref main2).get_startLifetime();
      double constant = (double) ((ParticleSystem.MinMaxCurve) ref startLifetime).get_constant();
      return (float) (maxParticles / constant) * this.RainIntensity;
    }

    protected virtual float MistEmissionRate()
    {
      ParticleSystem.MainModule main1 = this.RainMistParticleSystem.get_main();
      double maxParticles = (double) ((ParticleSystem.MainModule) ref main1).get_maxParticles();
      ParticleSystem.MainModule main2 = this.RainMistParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startLifetime = ((ParticleSystem.MainModule) ref main2).get_startLifetime();
      double constant = (double) ((ParticleSystem.MinMaxCurve) ref startLifetime).get_constant();
      return (float) (maxParticles / constant) * this.RainIntensity * this.RainIntensity;
    }

    protected virtual bool UseRainMistSoftParticles
    {
      get
      {
        return true;
      }
    }
  }
}
