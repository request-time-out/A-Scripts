// Decompiled with JetBrains decompiler
// Type: SpriteToParticlesAsset.EffectorRepeler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SpriteToParticlesAsset
{
  [ExecuteInEditMode]
  public class EffectorRepeler : MonoBehaviour
  {
    [Tooltip("Repeler force intensity. A negative strength will attract particles instead of repeling them.")]
    public float strength;
    [Tooltip("Transform at which the particles will repel from. If none is set it will use the current Sprite position.")]
    public Transform repelerCenter;
    private SpriteToParticles emitter;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private Vector3 center;

    public EffectorRepeler()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.emitter = (SpriteToParticles) ((Component) this).GetComponent<SpriteToParticles>();
      if (Object.op_Implicit((Object) this.emitter) && Object.op_Implicit((Object) this.emitter.particlesSystem))
        this.ps = this.emitter.particlesSystem;
      if (Object.op_Implicit((Object) this.repelerCenter))
        return;
      this.repelerCenter = ((Component) this).get_transform();
    }

    public void SetRepelCenterTransform(Transform repeler)
    {
      this.repelerCenter = repeler;
    }

    private void LateUpdate()
    {
      if (!Object.op_Implicit((Object) this.ps))
      {
        if (!Object.op_Implicit((Object) this.emitter) || !Object.op_Implicit((Object) this.emitter.particlesSystem))
          return;
        this.ps = this.emitter.particlesSystem;
      }
      if (this.particles == null || this.particles.Length < this.ps.get_particleCount())
        this.particles = new ParticleSystem.Particle[this.ps.get_particleCount()];
      int particles = this.ps.GetParticles(this.particles);
      ParticleSystem.MainModule main = this.ps.get_main();
      this.center = ((ParticleSystem.MainModule) ref main).get_simulationSpace() != 0 ? this.repelerCenter.get_position() : this.repelerCenter.get_localPosition();
      for (int index = 0; index < particles; ++index)
      {
        Vector3 vector3 = Vector3.op_Subtraction(((ParticleSystem.Particle) ref this.particles[index]).get_position(), this.center);
        ((ParticleSystem.Particle) ref this.particles[index]).set_velocity(Vector3.op_Multiply(vector3, this.strength));
      }
      this.ps.SetParticles(this.particles, particles);
    }
  }
}
