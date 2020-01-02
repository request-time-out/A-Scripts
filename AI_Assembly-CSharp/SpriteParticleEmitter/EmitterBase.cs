// Decompiled with JetBrains decompiler
// Type: SpriteParticleEmitter.EmitterBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SpriteParticleEmitter
{
  [SerializeField]
  public abstract class EmitterBase : MonoBehaviour
  {
    public bool verboseDebug;
    [Header("References")]
    [Tooltip("If none is provided the script will look for one in this game object.")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("If none is provided the script will look for one in this game object.")]
    public ParticleSystem particlesSystem;
    [Header("Emission Options")]
    [Tooltip("Start emitting as soon as able. (On static emission activating this will force CacheOnAwake)")]
    public bool PlayOnAwake;
    [Tooltip("Particles to emit per second")]
    public float EmissionRate;
    [Tooltip("Should new particles override ParticleSystem's startColor and use the color in the pixel they're emitting from?")]
    public bool UsePixelSourceColor;
    public EmitterBase.BorderEmission borderEmission;
    [Space(10f)]
    public bool UseEmissionFromColor;
    [Tooltip("Emission will take this color as only source position")]
    public Color EmitFromColor;
    [Range(0.01f, 1f)]
    [Tooltip("In conjunction with EmitFromColor. Defines how much can it deviate from red spectrum for selected color.")]
    public float RedTolerance;
    [Range(0.0f, 1f)]
    [Tooltip("In conjunction with EmitFromColor. Defines how much can it deviate from green spectrum for selected color.")]
    public float GreenTolerance;
    [Range(0.0f, 1f)]
    [Tooltip("In conjunction with EmitFromColor. Defines how much can it deviate from blue spectrum for selected color.")]
    public float BlueTolerance;
    protected ParticleSystemSimulationSpace SimulationSpace;
    protected bool isPlaying;
    protected ParticleSystem.MainModule mainModule;
    [Header("Advanced")]
    [Tooltip("This will save memory size when dealing with same sprite being loaded repeatedly by different GameObjects.")]
    public bool useSpritesSharingCache;
    public bool useBetweenFramesPrecision;
    protected Vector3 lastTransformPosition;
    protected float ParticlesToEmitThisFrame;

    protected EmitterBase()
    {
      base.\u002Ector();
    }

    protected virtual void Awake()
    {
      if (!Object.op_Implicit((Object) this.spriteRenderer))
      {
        if (this.verboseDebug)
          Debug.LogWarning((object) "Sprite Renderer not defined, trying to find in same GameObject");
        this.spriteRenderer = (SpriteRenderer) ((Component) this).GetComponent<SpriteRenderer>();
        if (!Object.op_Implicit((Object) this.spriteRenderer) && this.verboseDebug)
          Debug.LogWarning((object) "Sprite Renderer not found");
      }
      if (!Object.op_Implicit((Object) this.particlesSystem))
      {
        this.particlesSystem = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
        if (!Object.op_Implicit((Object) this.particlesSystem))
        {
          if (!this.verboseDebug)
            return;
          Debug.LogError((object) "No particle system found. Static Sprite Emission won't work");
          return;
        }
      }
      this.mainModule = this.particlesSystem.get_main();
      ((ParticleSystem.MainModule) ref this.mainModule).set_loop(false);
      ((ParticleSystem.MainModule) ref this.mainModule).set_playOnAwake(false);
      this.particlesSystem.Stop();
      this.SimulationSpace = ((ParticleSystem.MainModule) ref this.mainModule).get_simulationSpace();
    }

    public abstract void Play();

    public abstract void Pause();

    public abstract void Stop();

    public abstract bool IsPlaying();

    public abstract bool IsAvailableToPlay();

    public virtual event SimpleEvent OnCacheEnded;

    public virtual event SimpleEvent OnAvailableToPlay;

    private void DummyMethod()
    {
      if (this.OnAvailableToPlay != null)
        this.OnAvailableToPlay();
      if (this.OnCacheEnded == null)
        return;
      this.OnCacheEnded();
    }

    public enum BorderEmission
    {
      Off,
      Fast,
      Precise,
    }
  }
}
