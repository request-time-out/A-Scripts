// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.RainScript2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DigitalRuby.RainMaker
{
  public class RainScript2D : BaseRainScript
  {
    private static readonly Color32 explosionColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    private float cameraMultiplier = 1f;
    private Bounds visibleBounds = (Bounds) null;
    private readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[2048];
    [Tooltip("The starting y offset for rain and mist. This will be offset as a percentage of visible height from the top of the visible world.")]
    public float RainHeightMultiplier = 0.15f;
    [Tooltip("The total width of the rain and mist as a percentage of visible width")]
    public float RainWidthMultiplier = 1.5f;
    [Tooltip("Collision mask for the rain particles")]
    public LayerMask CollisionMask = LayerMask.op_Implicit(-1);
    [Tooltip("Lifetime to assign to rain particles that have collided. 0 for instant death. This can allow the rain to penetrate a little bit beyond the collision point.")]
    [Range(0.0f, 0.5f)]
    public float CollisionLifeTimeRain = 0.02f;
    [Tooltip("Multiply the velocity of any mist colliding by this amount")]
    [Range(0.0f, 0.99f)]
    public float RainMistCollisionMultiplier = 0.75f;
    private float yOffset;
    private float visibleWorldWidth;
    private float initialEmissionRain;
    private Vector2 initialStartSpeedRain;
    private Vector2 initialStartSizeRain;
    private Vector2 initialStartSpeedMist;
    private Vector2 initialStartSizeMist;
    private Vector2 initialStartSpeedExplosion;
    private Vector2 initialStartSizeExplosion;

    private void EmitExplosion(ref Vector3 pos)
    {
      for (int index = Random.Range(2, 5); index != 0; --index)
      {
        float num1 = Random.Range(-2f, 2f) * this.cameraMultiplier;
        float num2 = Random.Range(1f, 3f) * this.cameraMultiplier;
        float num3 = Random.Range(0.1f, 0.2f);
        float num4 = Random.Range(0.05f, 0.1f) * this.cameraMultiplier;
        ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
        ((ParticleSystem.EmitParams) ref emitParams).set_position(pos);
        ((ParticleSystem.EmitParams) ref emitParams).set_velocity(new Vector3(num1, num2, 0.0f));
        ((ParticleSystem.EmitParams) ref emitParams).set_startLifetime(num3);
        ((ParticleSystem.EmitParams) ref emitParams).set_startSize(num4);
        ((ParticleSystem.EmitParams) ref emitParams).set_startColor(RainScript2D.explosionColor);
        this.RainExplosionParticleSystem.Emit(emitParams, 1);
      }
    }

    private void TransformParticleSystem(
      ParticleSystem p,
      Vector2 initialStartSpeed,
      Vector2 initialStartSize)
    {
      if (Object.op_Equality((Object) p, (Object) null))
        return;
      if (this.FollowCamera)
        ((Component) p).get_transform().set_position(new Vector3((float) ((Component) this.Camera).get_transform().get_position().x, (float) ((Bounds) ref this.visibleBounds).get_max().y + this.yOffset, (float) ((Component) p).get_transform().get_position().z));
      else
        ((Component) p).get_transform().set_position(new Vector3((float) ((Component) p).get_transform().get_position().x, (float) ((Bounds) ref this.visibleBounds).get_max().y + this.yOffset, (float) ((Component) p).get_transform().get_position().z));
      ((Component) p).get_transform().set_localScale(new Vector3(this.visibleWorldWidth * this.RainWidthMultiplier, 1f, 1f));
      ParticleSystem.MainModule main = p.get_main();
      ParticleSystem.MinMaxCurve startSpeed = ((ParticleSystem.MainModule) ref main).get_startSpeed();
      ParticleSystem.MinMaxCurve startSize = ((ParticleSystem.MainModule) ref main).get_startSize();
      ((ParticleSystem.MinMaxCurve) ref startSpeed).set_constantMin((float) initialStartSpeed.x * this.cameraMultiplier);
      ((ParticleSystem.MinMaxCurve) ref startSpeed).set_constantMax((float) initialStartSpeed.y * this.cameraMultiplier);
      ((ParticleSystem.MinMaxCurve) ref startSize).set_constantMin((float) initialStartSize.x * this.cameraMultiplier);
      ((ParticleSystem.MinMaxCurve) ref startSize).set_constantMax((float) initialStartSize.y * this.cameraMultiplier);
      ((ParticleSystem.MainModule) ref main).set_startSpeed(startSpeed);
      ((ParticleSystem.MainModule) ref main).set_startSize(startSize);
    }

    private void CheckForCollisionsRainParticles()
    {
      int num1 = 0;
      bool flag = false;
      if (LayerMask.op_Implicit(this.CollisionMask) != 0)
      {
        num1 = this.RainFallParticleSystem.GetParticles(this.particles);
        for (int index = 0; index < num1; ++index)
        {
          Vector3 vector3 = Vector3.op_Addition(((ParticleSystem.Particle) ref this.particles[index]).get_position(), ((Component) this.RainFallParticleSystem).get_transform().get_position());
          Vector2 vector2_1 = Vector2.op_Implicit(vector3);
          Vector3 velocity1 = ((ParticleSystem.Particle) ref this.particles[index]).get_velocity();
          Vector2 vector2_2 = Vector2.op_Implicit(((Vector3) ref velocity1).get_normalized());
          Vector3 velocity2 = ((ParticleSystem.Particle) ref this.particles[index]).get_velocity();
          double num2 = (double) ((Vector3) ref velocity2).get_magnitude() * (double) Time.get_deltaTime();
          RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2_1, vector2_2, (float) num2);
          if (Object.op_Inequality((Object) ((RaycastHit2D) ref raycastHit2D).get_collider(), (Object) null) && (1 << ((Component) ((RaycastHit2D) ref raycastHit2D).get_collider()).get_gameObject().get_layer() & LayerMask.op_Implicit(this.CollisionMask)) != 0)
          {
            if ((double) this.CollisionLifeTimeRain == 0.0)
            {
              ((ParticleSystem.Particle) ref this.particles[index]).set_remainingLifetime(0.0f);
            }
            else
            {
              ((ParticleSystem.Particle) ref this.particles[index]).set_remainingLifetime(Mathf.Min(((ParticleSystem.Particle) ref this.particles[index]).get_remainingLifetime(), Random.Range(this.CollisionLifeTimeRain * 0.5f, this.CollisionLifeTimeRain * 2f)));
              Vector3.op_Addition(vector3, Vector3.op_Multiply(((ParticleSystem.Particle) ref this.particles[index]).get_velocity(), Time.get_deltaTime()));
            }
            flag = true;
          }
        }
      }
      if (Object.op_Inequality((Object) this.RainExplosionParticleSystem, (Object) null))
      {
        if (num1 == 0)
          num1 = this.RainFallParticleSystem.GetParticles(this.particles);
        for (int index = 0; index < num1; ++index)
        {
          if ((double) ((ParticleSystem.Particle) ref this.particles[index]).get_remainingLifetime() < 0.239999994635582)
          {
            Vector3 pos = Vector3.op_Addition(((ParticleSystem.Particle) ref this.particles[index]).get_position(), ((Component) this.RainFallParticleSystem).get_transform().get_position());
            this.EmitExplosion(ref pos);
          }
        }
      }
      if (!flag)
        return;
      this.RainFallParticleSystem.SetParticles(this.particles, num1);
    }

    private void CheckForCollisionsMistParticles()
    {
      if (Object.op_Equality((Object) this.RainMistParticleSystem, (Object) null) || LayerMask.op_Implicit(this.CollisionMask) == 0)
        return;
      int particles = this.RainMistParticleSystem.GetParticles(this.particles);
      bool flag = false;
      for (int index = 0; index < particles; ++index)
      {
        Vector2 vector2_1 = Vector2.op_Implicit(Vector3.op_Addition(((ParticleSystem.Particle) ref this.particles[index]).get_position(), ((Component) this.RainMistParticleSystem).get_transform().get_position()));
        Vector3 velocity1 = ((ParticleSystem.Particle) ref this.particles[index]).get_velocity();
        Vector2 vector2_2 = Vector2.op_Implicit(((Vector3) ref velocity1).get_normalized());
        Vector3 velocity2 = ((ParticleSystem.Particle) ref this.particles[index]).get_velocity();
        double num = (double) ((Vector3) ref velocity2).get_magnitude() * (double) Time.get_deltaTime();
        RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2_1, vector2_2, (float) num);
        if (Object.op_Inequality((Object) ((RaycastHit2D) ref raycastHit2D).get_collider(), (Object) null) && (1 << ((Component) ((RaycastHit2D) ref raycastHit2D).get_collider()).get_gameObject().get_layer() & LayerMask.op_Implicit(this.CollisionMask)) != 0)
        {
          ref ParticleSystem.Particle local = ref this.particles[index];
          ((ParticleSystem.Particle) ref local).set_velocity(Vector3.op_Multiply(((ParticleSystem.Particle) ref local).get_velocity(), this.RainMistCollisionMultiplier));
          flag = true;
        }
      }
      if (!flag)
        return;
      this.RainMistParticleSystem.SetParticles(this.particles, particles);
    }

    protected override void Start()
    {
      base.Start();
      ParticleSystem.EmissionModule emission = this.RainFallParticleSystem.get_emission();
      ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
      this.initialEmissionRain = ((ParticleSystem.MinMaxCurve) ref rateOverTime).get_constant();
      ParticleSystem.MainModule main1 = this.RainFallParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSpeed1 = ((ParticleSystem.MainModule) ref main1).get_startSpeed();
      double constantMin1 = (double) ((ParticleSystem.MinMaxCurve) ref startSpeed1).get_constantMin();
      ParticleSystem.MainModule main2 = this.RainFallParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSpeed2 = ((ParticleSystem.MainModule) ref main2).get_startSpeed();
      double constantMax1 = (double) ((ParticleSystem.MinMaxCurve) ref startSpeed2).get_constantMax();
      this.initialStartSpeedRain = new Vector2((float) constantMin1, (float) constantMax1);
      ParticleSystem.MainModule main3 = this.RainFallParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSize1 = ((ParticleSystem.MainModule) ref main3).get_startSize();
      double constantMin2 = (double) ((ParticleSystem.MinMaxCurve) ref startSize1).get_constantMin();
      ParticleSystem.MainModule main4 = this.RainFallParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSize2 = ((ParticleSystem.MainModule) ref main4).get_startSize();
      double constantMax2 = (double) ((ParticleSystem.MinMaxCurve) ref startSize2).get_constantMax();
      this.initialStartSizeRain = new Vector2((float) constantMin2, (float) constantMax2);
      if (Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
      {
        ParticleSystem.MainModule main5 = this.RainMistParticleSystem.get_main();
        ParticleSystem.MinMaxCurve startSpeed3 = ((ParticleSystem.MainModule) ref main5).get_startSpeed();
        double constantMin3 = (double) ((ParticleSystem.MinMaxCurve) ref startSpeed3).get_constantMin();
        ParticleSystem.MainModule main6 = this.RainMistParticleSystem.get_main();
        ParticleSystem.MinMaxCurve startSpeed4 = ((ParticleSystem.MainModule) ref main6).get_startSpeed();
        double constantMax3 = (double) ((ParticleSystem.MinMaxCurve) ref startSpeed4).get_constantMax();
        this.initialStartSpeedMist = new Vector2((float) constantMin3, (float) constantMax3);
        ParticleSystem.MainModule main7 = this.RainMistParticleSystem.get_main();
        ParticleSystem.MinMaxCurve startSize3 = ((ParticleSystem.MainModule) ref main7).get_startSize();
        double constantMin4 = (double) ((ParticleSystem.MinMaxCurve) ref startSize3).get_constantMin();
        ParticleSystem.MainModule main8 = this.RainMistParticleSystem.get_main();
        ParticleSystem.MinMaxCurve startSize4 = ((ParticleSystem.MainModule) ref main8).get_startSize();
        double constantMax4 = (double) ((ParticleSystem.MinMaxCurve) ref startSize4).get_constantMax();
        this.initialStartSizeMist = new Vector2((float) constantMin4, (float) constantMax4);
      }
      if (!Object.op_Inequality((Object) this.RainExplosionParticleSystem, (Object) null))
        return;
      ParticleSystem.MainModule main9 = this.RainExplosionParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSpeed5 = ((ParticleSystem.MainModule) ref main9).get_startSpeed();
      double constantMin5 = (double) ((ParticleSystem.MinMaxCurve) ref startSpeed5).get_constantMin();
      ParticleSystem.MainModule main10 = this.RainExplosionParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSpeed6 = ((ParticleSystem.MainModule) ref main10).get_startSpeed();
      double constantMax5 = (double) ((ParticleSystem.MinMaxCurve) ref startSpeed6).get_constantMax();
      this.initialStartSpeedExplosion = new Vector2((float) constantMin5, (float) constantMax5);
      ParticleSystem.MainModule main11 = this.RainExplosionParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSize5 = ((ParticleSystem.MainModule) ref main11).get_startSize();
      double constantMin6 = (double) ((ParticleSystem.MinMaxCurve) ref startSize5).get_constantMin();
      ParticleSystem.MainModule main12 = this.RainExplosionParticleSystem.get_main();
      ParticleSystem.MinMaxCurve startSize6 = ((ParticleSystem.MainModule) ref main12).get_startSize();
      double constantMax6 = (double) ((ParticleSystem.MinMaxCurve) ref startSize6).get_constantMax();
      this.initialStartSizeExplosion = new Vector2((float) constantMin6, (float) constantMax6);
    }

    protected override void Update()
    {
      base.Update();
      this.cameraMultiplier = this.Camera.get_orthographicSize() * 0.25f;
      ((Bounds) ref this.visibleBounds).set_min(Camera.get_main().ViewportToWorldPoint(Vector3.get_zero()));
      ((Bounds) ref this.visibleBounds).set_max(Camera.get_main().ViewportToWorldPoint(Vector3.get_one()));
      this.visibleWorldWidth = (float) ((Bounds) ref this.visibleBounds).get_size().x;
      this.yOffset = (float) (((Bounds) ref this.visibleBounds).get_max().y - ((Bounds) ref this.visibleBounds).get_min().y) * this.RainHeightMultiplier;
      this.TransformParticleSystem(this.RainFallParticleSystem, this.initialStartSpeedRain, this.initialStartSizeRain);
      this.TransformParticleSystem(this.RainMistParticleSystem, this.initialStartSpeedMist, this.initialStartSizeMist);
      this.TransformParticleSystem(this.RainExplosionParticleSystem, this.initialStartSpeedExplosion, this.initialStartSizeExplosion);
      this.CheckForCollisionsRainParticles();
      this.CheckForCollisionsMistParticles();
    }

    protected override float RainFallEmissionRate()
    {
      return this.initialEmissionRain * this.RainIntensity;
    }

    protected override bool UseRainMistSoftParticles
    {
      get
      {
        return false;
      }
    }
  }
}
