// Decompiled with JetBrains decompiler
// Type: ME_ParticleGravityPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class ME_ParticleGravityPoint : MonoBehaviour
{
  public Transform target;
  public float Force;
  public bool DistanceRelative;
  private ParticleSystem ps;
  private ParticleSystem.Particle[] particles;
  private ParticleSystem.MainModule mainModule;
  private Vector3 prevPos;

  public ME_ParticleGravityPoint()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.ps = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
    this.mainModule = this.ps.get_main();
  }

  private void LateUpdate()
  {
    int maxParticles = ((ParticleSystem.MainModule) ref this.mainModule).get_maxParticles();
    if (this.particles == null || this.particles.Length < maxParticles)
      this.particles = new ParticleSystem.Particle[maxParticles];
    int particles = this.ps.GetParticles(this.particles);
    Vector3 vector3_1 = Vector3.get_zero();
    if (((ParticleSystem.MainModule) ref this.mainModule).get_simulationSpace() == null)
      vector3_1 = ((Component) this).get_transform().InverseTransformPoint(this.target.get_position());
    if (((ParticleSystem.MainModule) ref this.mainModule).get_simulationSpace() == 1)
      vector3_1 = this.target.get_position();
    float num1 = Time.get_deltaTime() * this.Force;
    if (this.DistanceRelative)
    {
      double num2 = (double) num1;
      Vector3 vector3_2 = Vector3.op_Subtraction(this.prevPos, vector3_1);
      double num3 = (double) Mathf.Abs(((Vector3) ref vector3_2).get_magnitude());
      num1 = (float) (num2 * num3);
    }
    for (int index = 0; index < particles; ++index)
    {
      Vector3 vector3_2 = Vector3.Normalize(Vector3.op_Subtraction(vector3_1, ((ParticleSystem.Particle) ref this.particles[index]).get_position()));
      if (this.DistanceRelative)
        vector3_2 = Vector3.Normalize(Vector3.op_Subtraction(vector3_1, this.prevPos));
      Vector3 vector3_3 = Vector3.op_Multiply(vector3_2, num1);
      ref ParticleSystem.Particle local = ref this.particles[index];
      ((ParticleSystem.Particle) ref local).set_velocity(Vector3.op_Addition(((ParticleSystem.Particle) ref local).get_velocity(), vector3_3));
    }
    this.ps.SetParticles(this.particles, particles);
    this.prevPos = vector3_1;
  }
}
