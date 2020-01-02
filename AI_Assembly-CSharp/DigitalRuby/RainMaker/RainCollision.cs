// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.RainCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
  public class RainCollision : MonoBehaviour
  {
    private static readonly Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    private readonly List<ParticleCollisionEvent> collisionEvents;
    public ParticleSystem RainExplosion;
    public ParticleSystem RainParticleSystem;

    public RainCollision()
    {
      base.\u002Ector();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void Emit(ParticleSystem p, ref Vector3 pos)
    {
      for (int index = Random.Range(2, 5); index != 0; --index)
      {
        float num1 = Random.Range(1f, 3f);
        float num2 = Random.Range(-2f, 2f);
        float num3 = Random.Range(-2f, 2f);
        float num4 = Random.Range(0.05f, 0.1f);
        ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams) null;
        ((ParticleSystem.EmitParams) ref emitParams).set_position(pos);
        ((ParticleSystem.EmitParams) ref emitParams).set_velocity(new Vector3(num3, num1, num2));
        ((ParticleSystem.EmitParams) ref emitParams).set_startLifetime(0.75f);
        ((ParticleSystem.EmitParams) ref emitParams).set_startSize(num4);
        ((ParticleSystem.EmitParams) ref emitParams).set_startColor(RainCollision.color);
        p.Emit(emitParams, 1);
      }
    }

    private void OnParticleCollision(GameObject obj)
    {
      if (!Object.op_Inequality((Object) this.RainExplosion, (Object) null) || !Object.op_Inequality((Object) this.RainParticleSystem, (Object) null))
        return;
      int collisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this.RainParticleSystem, obj, this.collisionEvents);
      for (int index = 0; index < collisionEvents; ++index)
      {
        ParticleCollisionEvent collisionEvent = this.collisionEvents[index];
        Vector3 intersection = ((ParticleCollisionEvent) ref collisionEvent).get_intersection();
        this.Emit(this.RainExplosion, ref intersection);
      }
    }
  }
}
