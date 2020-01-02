// Decompiled with JetBrains decompiler
// Type: ME_ParticleCollisionDecal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ME_ParticleCollisionDecal : MonoBehaviour
{
  public ParticleSystem DecalParticles;
  public bool IsBilboard;
  public bool InstantiateWhenZeroSpeed;
  public float MaxGroundAngleDeviation;
  public float MinDistanceBetweenDecals;
  public float MinDistanceBetweenSurface;
  private List<ParticleCollisionEvent> collisionEvents;
  private ParticleSystem.Particle[] particles;
  private ParticleSystem initiatorPS;
  private List<GameObject> collidedGameObjects;

  public ME_ParticleCollisionDecal()
  {
    base.\u002Ector();
  }

  private void OnEnable()
  {
    this.collisionEvents.Clear();
    this.collidedGameObjects.Clear();
    this.initiatorPS = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
    ParticleSystem.MainModule main = this.DecalParticles.get_main();
    this.particles = new ParticleSystem.Particle[((ParticleSystem.MainModule) ref main).get_maxParticles()];
    if (!this.InstantiateWhenZeroSpeed)
      return;
    this.InvokeRepeating("CollisionDetect", 0.0f, 0.1f);
  }

  private void OnDisable()
  {
    if (!this.InstantiateWhenZeroSpeed)
      return;
    this.CancelInvoke("CollisionDetect");
  }

  private void CollisionDetect()
  {
    int aliveParticles = 0;
    if (this.InstantiateWhenZeroSpeed)
      aliveParticles = this.DecalParticles.GetParticles(this.particles);
    using (List<GameObject>.Enumerator enumerator = this.collidedGameObjects.GetEnumerator())
    {
      while (enumerator.MoveNext())
        this.OnParticleCollisionManual(enumerator.Current, aliveParticles);
    }
  }

  private void OnParticleCollisionManual(GameObject other, int aliveParticles = -1)
  {
    this.collisionEvents.Clear();
    int collisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this.initiatorPS, other, this.collisionEvents);
    for (int index1 = 0; index1 < collisionEvents; ++index1)
    {
      ParticleCollisionEvent collisionEvent1 = this.collisionEvents[index1];
      if ((double) Vector3.Angle(((ParticleCollisionEvent) ref collisionEvent1).get_normal(), Vector3.get_up()) <= (double) this.MaxGroundAngleDeviation)
      {
        if (this.InstantiateWhenZeroSpeed)
        {
          ParticleCollisionEvent collisionEvent2 = this.collisionEvents[index1];
          Vector3 velocity = ((ParticleCollisionEvent) ref collisionEvent2).get_velocity();
          if ((double) ((Vector3) ref velocity).get_sqrMagnitude() <= 0.100000001490116)
          {
            bool flag = false;
            for (int index2 = 0; index2 < aliveParticles; ++index2)
            {
              ParticleCollisionEvent collisionEvent3 = this.collisionEvents[index1];
              if ((double) Vector3.Distance(((ParticleCollisionEvent) ref collisionEvent3).get_intersection(), ((ParticleSystem.Particle) ref this.particles[index2]).get_position()) < (double) this.MinDistanceBetweenDecals)
                flag = true;
            }
            if (flag)
              continue;
          }
          else
            continue;
        }
        ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
        ref ParticleSystem.EmitParams local = ref emitParams;
        ParticleCollisionEvent collisionEvent4 = this.collisionEvents[index1];
        Vector3 intersection = ((ParticleCollisionEvent) ref collisionEvent4).get_intersection();
        ParticleCollisionEvent collisionEvent5 = this.collisionEvents[index1];
        Vector3 vector3_1 = Vector3.op_Multiply(((ParticleCollisionEvent) ref collisionEvent5).get_normal(), this.MinDistanceBetweenSurface);
        Vector3 vector3_2 = Vector3.op_Addition(intersection, vector3_1);
        ((ParticleSystem.EmitParams) ref local).set_position(vector3_2);
        ParticleCollisionEvent collisionEvent6 = this.collisionEvents[index1];
        Quaternion quaternion = Quaternion.LookRotation(Vector3.op_UnaryNegation(((ParticleCollisionEvent) ref collisionEvent6).get_normal()));
        Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
        eulerAngles.z = (__Null) (double) Random.Range(0, 360);
        ((ParticleSystem.EmitParams) ref emitParams).set_rotation3D(eulerAngles);
        this.DecalParticles.Emit(emitParams, 1);
      }
    }
  }

  private void OnParticleCollision(GameObject other)
  {
    if (this.InstantiateWhenZeroSpeed)
    {
      if (this.collidedGameObjects.Contains(other))
        return;
      this.collidedGameObjects.Add(other);
    }
    else
      this.OnParticleCollisionManual(other, -1);
  }
}
