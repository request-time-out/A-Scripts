// Decompiled with JetBrains decompiler
// Type: AIProject.RainSimulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class RainSimulator : MonoBehaviour
  {
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private bool _followsCamera;
    [SerializeField]
    private RainSimulator.RainAudioClipGroup _audioClips;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _rainIntensity;
    [SerializeField]
    private RainSimulator.RainParticleGroup _particles;
    [SerializeField]
    private WindZone _windZone;
    [SerializeField]
    private bool _enableWind;
    [SerializeField]
    private Vector3 _rainOffset;
    [SerializeField]
    private Vector3 _mistOffset;
    protected RainSimulator.LoopingAudioSource _audioSourceRainLight;
    protected RainSimulator.LoopingAudioSource _audioSourceRainMedium;
    protected RainSimulator.LoopingAudioSource _audioSourceRainHeavy;
    protected RainSimulator.LoopingAudioSource _audioSourceRainCurrent;
    protected RainSimulator.LoopingAudioSource _audioSourceWind;
    private float _lastRainIntensityValue;
    private float _nextWindTime;

    public RainSimulator()
    {
      base.\u002Ector();
    }

    public Camera Camera
    {
      get
      {
        return this._camera;
      }
      set
      {
        this._camera = value;
      }
    }

    public bool FollowsCamera
    {
      get
      {
        return this._followsCamera;
      }
      set
      {
        this._followsCamera = value;
      }
    }

    public RainSimulator.RainAudioClipGroup AudioClips
    {
      get
      {
        return this._audioClips;
      }
    }

    public float RainIntensity
    {
      get
      {
        return this._rainIntensity;
      }
      set
      {
        this._rainIntensity = value;
      }
    }

    public RainSimulator.RainParticleGroup Particles
    {
      get
      {
        return this._particles;
      }
    }

    public WindZone WindZone
    {
      get
      {
        return this._windZone;
      }
      set
      {
        this._windZone = value;
      }
    }

    public bool EnableWind
    {
      get
      {
        return this._enableWind;
      }
      set
      {
        this._enableWind = value;
      }
    }

    protected virtual void Start()
    {
      if (Object.op_Equality((Object) this._camera, (Object) null))
        this._camera = Camera.get_main();
      this._audioSourceRainLight = new RainSimulator.LoopingAudioSource((MonoBehaviour) this, this._audioClips.RainLight);
      this._audioSourceRainMedium = new RainSimulator.LoopingAudioSource((MonoBehaviour) this, this._audioClips.RainMedium);
      this._audioSourceRainHeavy = new RainSimulator.LoopingAudioSource((MonoBehaviour) this, this._audioClips.RainHeavy);
      this._audioSourceWind = new RainSimulator.LoopingAudioSource((MonoBehaviour) this, this._audioClips.Wind);
      this._particles.Init(this.UseRainMistSoftPartilces);
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    protected virtual void OnUpdate()
    {
      this.CheckForRainChange();
      this.UpdateWind();
      this._audioSourceRainLight.Update();
      this._audioSourceRainMedium.Update();
      this._audioSourceRainHeavy.Update();
      this.UpdateRain();
    }

    private void UpdateRain()
    {
      if (this._followsCamera)
      {
        if (Object.op_Inequality((Object) this._particles.Fall, (Object) null))
        {
          ParticleSystem fall = this._particles.Fall;
          ParticleSystem.ShapeModule shape = fall.get_shape();
          ((ParticleSystem.ShapeModule) ref shape).set_shapeType((ParticleSystemShapeType) 8);
          ((Component) fall).get_transform().set_position(((Component) this._camera).get_transform().get_position());
          ((Component) fall).get_transform().Translate(this._rainOffset);
          ((Component) fall).get_transform().set_rotation(Quaternion.Euler(0.0f, (float) ((Component) this.Camera).get_transform().get_eulerAngles().y, 0.0f));
        }
        if (!Object.op_Inequality((Object) this._particles.Mist, (Object) null))
          return;
        ParticleSystem mist = this._particles.Mist;
        ParticleSystem.ShapeModule shape1 = mist.get_shape();
        ((ParticleSystem.ShapeModule) ref shape1).set_shapeType((ParticleSystemShapeType) 2);
        Vector3 position = ((Component) this._camera).get_transform().get_position();
        ref Vector3 local = ref position;
        local.y = local.y + this._mistOffset.y;
        ((Component) mist).get_transform().set_position(position);
      }
      else
      {
        if (Object.op_Inequality((Object) this._particles.Fall, (Object) null))
        {
          ParticleSystem.ShapeModule shape = this._particles.Fall.get_shape();
          ((ParticleSystem.ShapeModule) ref shape).set_shapeType((ParticleSystemShapeType) 5);
        }
        if (!Object.op_Inequality((Object) this._particles.Mist, (Object) null))
          return;
        ParticleSystem.ShapeModule shape1 = this._particles.Mist.get_shape();
        ((ParticleSystem.ShapeModule) ref shape1).set_shapeType((ParticleSystemShapeType) 5);
        Vector3 vector3 = Vector3.op_Addition(((Component) this._particles.Fall).get_transform().get_position(), this._mistOffset);
        ref Vector3 local = ref vector3;
        local.y = local.y - this._rainOffset.y;
        ((Component) this._particles.Mist).get_transform().set_position(vector3);
      }
    }

    private void UpdateWind()
    {
      if (this._enableWind)
      {
        if (Object.op_Inequality((Object) this._windZone, (Object) null))
        {
          ((Component) this._windZone).get_gameObject().SetActive(true);
          if (this._followsCamera)
            ((Component) this._windZone).get_transform().set_position(((Component) this._camera).get_transform().get_position());
          if (!this._camera.get_orthographic())
            ((Component) this._windZone).get_transform().Translate(0.0f, this._windZone.get_radius(), 0.0f);
          if ((double) this._nextWindTime < (double) Time.get_time())
          {
            Threshold windSpeedRange = Singleton<Manager.Map>.Instance.EnvironmentProfile.WindSpeedRange;
            this._windZone.set_windMain(windSpeedRange.RandomValue);
            this._windZone.set_windTurbulence(windSpeedRange.RandomValue);
            if (this.Camera.get_orthographic())
              ((Component) this._windZone).get_transform().set_rotation(Quaternion.Euler(0.0f, Random.Range(0, 2) != 0 ? -90f : 90f, 0.0f));
            else
              ((Component) this._windZone).get_transform().set_rotation(Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0.0f, 360f), 0.0f));
            this._nextWindTime = Time.get_time() + Singleton<Manager.Map>.Instance.EnvironmentProfile.WindChangeIntervalThreshold.RandomValue;
            this._audioSourceWind.Play(this._windZone.get_windMain() / Singleton<Manager.Map>.Instance.EnvironmentProfile.WindSoundMultiplier * Singleton<Manager.Map>.Instance.EnvironmentProfile.WindSoundVolumeModifier);
          }
        }
      }
      else
      {
        if (Object.op_Inequality((Object) this._windZone, (Object) null))
          ((Component) this._windZone).get_gameObject().SetActive(false);
        this._audioSourceWind.Stop();
      }
      this._audioSourceWind.Update();
    }

    private void CheckForRainChange()
    {
      float num1 = Singleton<Manager.Map>.Instance.EnvironmentProfile.RainIntensity * this._rainIntensity;
      if ((double) this._lastRainIntensityValue == (double) num1)
        return;
      this._lastRainIntensityValue = num1;
      if ((double) num1 <= 0.00999999977648258)
      {
        if (this._audioSourceRainCurrent != null)
        {
          this._audioSourceRainCurrent.Stop();
          this._audioSourceRainCurrent = (RainSimulator.LoopingAudioSource) null;
        }
        if (Object.op_Inequality((Object) this._particles.Fall, (Object) null))
        {
          ParticleSystem.EmissionModule emission = this._particles.Fall.get_emission();
          ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
          this._particles.Fall.Stop();
        }
        if (!Object.op_Inequality((Object) this._particles.Mist, (Object) null))
          return;
        ParticleSystem.EmissionModule emission1 = this._particles.Mist.get_emission();
        ((ParticleSystem.EmissionModule) ref emission1).set_enabled(false);
        this._particles.Mist.Stop();
      }
      else
      {
        RainSimulator.LoopingAudioSource loopingAudioSource = (double) num1 < 0.670000016689301 ? ((double) num1 < 0.330000013113022 ? this._audioSourceRainLight : this._audioSourceRainMedium) : this._audioSourceRainHeavy;
        if (this._audioSourceRainCurrent != loopingAudioSource)
        {
          if (this._audioSourceRainCurrent != null)
            this._audioSourceRainCurrent.Stop();
          this._audioSourceRainCurrent = loopingAudioSource;
          this._audioSourceRainCurrent.Play(1f);
        }
        if (Object.op_Inequality((Object) this._particles.Fall, (Object) null))
        {
          ParticleSystem.EmissionModule emission = this._particles.Fall.get_emission();
          ref ParticleSystem.EmissionModule local1 = ref emission;
          bool flag = true;
          ((Renderer) ((Component) this._particles.Fall).GetComponent<Renderer>()).set_enabled(flag);
          int num2 = flag ? 1 : 0;
          ((ParticleSystem.EmissionModule) ref local1).set_enabled(num2 != 0);
          if (!this._particles.Fall.get_isPlaying())
            this._particles.Fall.Play();
          ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
          ((ParticleSystem.MinMaxCurve) ref rateOverTime).set_mode((ParticleSystemCurveMode) 0);
          ref ParticleSystem.MinMaxCurve local2 = ref rateOverTime;
          float num3 = this.RainFallEmissionRate();
          ((ParticleSystem.MinMaxCurve) ref rateOverTime).set_constantMax(num3);
          double num4 = (double) num3;
          ((ParticleSystem.MinMaxCurve) ref local2).set_constantMin((float) num4);
        }
        if (!Object.op_Inequality((Object) this._particles.Mist, (Object) null))
          return;
        ParticleSystem mist = this._particles.Mist;
        ParticleSystem.EmissionModule emission1 = mist.get_emission();
        ref ParticleSystem.EmissionModule local3 = ref emission1;
        bool flag1 = true;
        ((Renderer) ((Component) mist).GetComponent<Renderer>()).set_enabled(flag1);
        int num5 = flag1 ? 1 : 0;
        ((ParticleSystem.EmissionModule) ref local3).set_enabled(num5 != 0);
        if (mist.get_isPlaying())
          mist.Play();
        float num6 = (double) num1 >= (double) Singleton<Manager.Map>.Instance.EnvironmentProfile.RainMistThreshold ? this.MistEmissionRate() : 0.0f;
        ParticleSystem.MinMaxCurve rateOverTime1 = ((ParticleSystem.EmissionModule) ref emission1).get_rateOverTime();
        ((ParticleSystem.MinMaxCurve) ref rateOverTime1).set_mode((ParticleSystemCurveMode) 0);
        ref ParticleSystem.MinMaxCurve local4 = ref rateOverTime1;
        float num7 = num6;
        ((ParticleSystem.MinMaxCurve) ref rateOverTime1).set_constantMax(num7);
        double num8 = (double) num7;
        ((ParticleSystem.MinMaxCurve) ref local4).set_constantMin((float) num8);
        ((ParticleSystem.EmissionModule) ref emission1).set_rateOverTime(rateOverTime1);
      }
    }

    protected virtual float RainFallEmissionRate()
    {
      ParticleSystem.MainModule main = this._particles.Fall.get_main();
      double maxParticles = (double) ((ParticleSystem.MainModule) ref main).get_maxParticles();
      ParticleSystem.MinMaxCurve startLifetime = ((ParticleSystem.MainModule) ref main).get_startLifetime();
      double constant = (double) ((ParticleSystem.MinMaxCurve) ref startLifetime).get_constant();
      return (float) (maxParticles / constant) * Singleton<Manager.Map>.Instance.EnvironmentProfile.RainIntensity * this._rainIntensity;
    }

    protected virtual float MistEmissionRate()
    {
      ParticleSystem.MainModule main = this._particles.Mist.get_main();
      float num = Singleton<Manager.Map>.Instance.EnvironmentProfile.RainIntensity * this._rainIntensity;
      double maxParticles = (double) ((ParticleSystem.MainModule) ref main).get_maxParticles();
      ParticleSystem.MinMaxCurve startLifetime = ((ParticleSystem.MainModule) ref main).get_startLifetime();
      double constant = (double) ((ParticleSystem.MinMaxCurve) ref startLifetime).get_constant();
      return (float) (maxParticles / constant) * num * num;
    }

    protected virtual bool UseRainMistSoftPartilces
    {
      get
      {
        return true;
      }
    }

    [Serializable]
    public class RainAudioClipGroup
    {
      [SerializeField]
      private AudioClip _rainAudioLight;
      [SerializeField]
      private AudioClip _rainAudioMedium;
      [SerializeField]
      private AudioClip _rainAudioHeavy;
      [SerializeField]
      private AudioClip _wind;

      public AudioClip RainLight
      {
        get
        {
          return this._rainAudioLight;
        }
        set
        {
          this._rainAudioLight = value;
        }
      }

      public AudioClip RainMedium
      {
        get
        {
          return this._rainAudioMedium;
        }
        set
        {
          this._rainAudioMedium = value;
        }
      }

      public AudioClip RainHeavy
      {
        get
        {
          return this._rainAudioHeavy;
        }
        set
        {
          this._rainAudioHeavy = value;
        }
      }

      public AudioClip Wind
      {
        get
        {
          return this._wind;
        }
        set
        {
          this._wind = value;
        }
      }
    }

    [Serializable]
    public class RainParticleGroup
    {
      [SerializeField]
      private ParticleSystem _fall;
      [SerializeField]
      private ParticleSystem _explosion;
      [SerializeField]
      private ParticleSystem _mist;
      protected Material _fallMaterial;
      protected Material _explosionMaterial;
      protected Material _mistMaterial;

      public ParticleSystem Fall
      {
        get
        {
          return this._fall;
        }
        set
        {
          this._fall = value;
        }
      }

      public ParticleSystem Explosion
      {
        get
        {
          return this._explosion;
        }
        set
        {
          this._explosion = value;
        }
      }

      public ParticleSystem Mist
      {
        get
        {
          return this._mist;
        }
        set
        {
          this._mist = value;
        }
      }

      public void Init(bool useMistSoftParticles)
      {
        if (Object.op_Inequality((Object) this._fall, (Object) null))
        {
          ParticleSystem.EmissionModule emission = this._fall.get_emission();
          ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
          Renderer component = (Renderer) ((Component) this._fall).GetComponent<Renderer>();
          component.set_enabled(false);
          this._fallMaterial = new Material(component.get_material());
          this._fallMaterial.EnableKeyword("SOFTPARTICLES_OFF");
          component.set_material(this._fallMaterial);
        }
        if (Object.op_Inequality((Object) this._explosion, (Object) null))
        {
          ParticleSystem.EmissionModule emission = this._explosion.get_emission();
          ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
          Renderer component = (Renderer) ((Component) this._fall).GetComponent<Renderer>();
          component.set_enabled(false);
          this._explosionMaterial = new Material(component.get_material());
          this._explosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
          component.set_material(this._explosionMaterial);
        }
        if (!Object.op_Inequality((Object) this._mist, (Object) null))
          return;
        ParticleSystem.EmissionModule emission1 = this._mist.get_emission();
        ((ParticleSystem.EmissionModule) ref emission1).set_enabled(false);
        Renderer component1 = (Renderer) ((Component) this._mist).GetComponent<Renderer>();
        component1.set_enabled(false);
        this._mistMaterial = new Material(component1.get_material());
        if (useMistSoftParticles)
          this._mistMaterial.EnableKeyword("SOFTPARTICLES_ON");
        else
          this._mistMaterial.EnableKeyword("SOFTPARTICLES_OFF");
        component1.set_material(this._mistMaterial);
      }
    }

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
        this.AudioSource.set_outputAudioMixerGroup(Sound.Mixer.FindMatchingGroups("ENV")[0]);
        this.TargetVolume = 1f;
      }

      public AudioSource AudioSource { get; private set; }

      public float TargetVolume { get; private set; }

      public void Play(float volume)
      {
        if (!this.AudioSource.get_isPlaying())
        {
          this.AudioSource.set_volume(0.0f);
          this.AudioSource.Play();
        }
        this.TargetVolume = volume;
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
}
