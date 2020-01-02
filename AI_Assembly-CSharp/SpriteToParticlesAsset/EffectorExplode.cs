// Decompiled with JetBrains decompiler
// Type: SpriteToParticlesAsset.EffectorExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SpriteToParticlesAsset
{
  public class EffectorExplode : MonoBehaviour
  {
    [Tooltip("Weather the system is being used for Sprite or Image component. ")]
    public float destroyObjectAfterExplosionIn;
    private SpriteToParticles emitter;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    [HideInInspector]
    public bool exploded;

    public EffectorExplode()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.emitter = (SpriteToParticles) ((Component) this).GetComponent<SpriteToParticles>();
      if (!Object.op_Implicit((Object) this.emitter) || !Object.op_Implicit((Object) this.emitter.particlesSystem))
        return;
      this.ps = this.emitter.particlesSystem;
    }

    public void ExplodeAt(
      Vector3 sourcePos,
      float radius,
      float angle,
      float startRot,
      float strenght)
    {
      if (!Object.op_Implicit((Object) this.ps))
      {
        if (!Object.op_Implicit((Object) this.emitter) || !Object.op_Implicit((Object) this.emitter.particlesSystem))
          return;
        this.ps = this.emitter.particlesSystem;
      }
      this.emitter.EmitAll(true);
      if (this.particles == null || this.particles.Length < this.ps.get_particleCount())
        this.particles = new ParticleSystem.Particle[this.ps.get_particleCount()];
      int particles = this.ps.GetParticles(this.particles);
      float num1 = radius / 2f;
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector(Mathf.Cos((float) Math.PI / 180f * startRot), Mathf.Sin((float) Math.PI / 180f * startRot));
      for (int index = 0; index < particles; ++index)
      {
        ParticleSystem.Particle particle = this.particles[index];
        float num2 = Vector3.Distance(sourcePos, ((ParticleSystem.Particle) ref particle).get_position());
        if ((double) num2 < (double) num1)
        {
          Vector3 vector3 = Vector3.op_Subtraction(((ParticleSystem.Particle) ref particle).get_position(), sourcePos);
          float num3 = Vector3.Angle(Vector2.op_Implicit(vector2), vector3);
          if (Vector3.Cross(Vector2.op_Implicit(vector2), vector3).z < 0.0)
            num3 = 360f - num3;
          if ((double) num3 < (double) angle)
          {
            ((Vector3) ref vector3).Normalize();
            float num4 = radius - num2;
            float num5 = Random.Range(num4 / 2f, num4);
            ref ParticleSystem.Particle local = ref particle;
            ((ParticleSystem.Particle) ref local).set_velocity(Vector3.op_Addition(((ParticleSystem.Particle) ref local).get_velocity(), Vector3.op_Multiply(Vector3.op_Multiply(vector3, num5), strenght)));
            this.particles[index] = particle;
          }
        }
      }
      this.ps.SetParticles(this.particles, particles);
      this.exploded = true;
      if ((double) this.destroyObjectAfterExplosionIn < 0.0)
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject(), this.destroyObjectAfterExplosionIn);
    }

    public void ExplodeTest()
    {
      this.ExplodeAt(((Component) this).get_transform().get_position(), 10f, 360f, 0.0f, 2f);
    }
  }
}
