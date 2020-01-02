// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.RainScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DigitalRuby.RainMaker
{
  public class RainScript : BaseRainScript
  {
    [Tooltip("The height above the camera that the rain will start falling from")]
    public float RainHeight = 25f;
    [Tooltip("How far the rain particle system is ahead of the player")]
    public float RainForwardOffset = -7f;
    [Tooltip("The top y value of the mist particles")]
    public float RainMistHeight = 3f;

    private void UpdateRain()
    {
      if (!Object.op_Inequality((Object) this.RainFallParticleSystem, (Object) null))
        return;
      if (this.FollowCamera)
      {
        ParticleSystem.ShapeModule shape1 = this.RainFallParticleSystem.get_shape();
        ((ParticleSystem.ShapeModule) ref shape1).set_shapeType((ParticleSystemShapeType) 8);
        ((Component) this.RainFallParticleSystem).get_transform().set_position(((Component) this.Camera).get_transform().get_position());
        ((Component) this.RainFallParticleSystem).get_transform().Translate(0.0f, this.RainHeight, this.RainForwardOffset);
        Transform transform = ((Component) this.RainFallParticleSystem).get_transform();
        Quaternion rotation = ((Component) this.Camera).get_transform().get_rotation();
        Quaternion quaternion = Quaternion.Euler(0.0f, (float) ((Quaternion) ref rotation).get_eulerAngles().y, 0.0f);
        transform.set_rotation(quaternion);
        if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
          return;
        ParticleSystem.ShapeModule shape2 = this.RainMistParticleSystem.get_shape();
        ((ParticleSystem.ShapeModule) ref shape2).set_shapeType((ParticleSystemShapeType) 2);
        Vector3 position = ((Component) this.Camera).get_transform().get_position();
        ref Vector3 local = ref position;
        local.y = (__Null) (local.y + (double) this.RainMistHeight);
        ((Component) this.RainMistParticleSystem).get_transform().set_position(position);
      }
      else
      {
        ParticleSystem.ShapeModule shape1 = this.RainFallParticleSystem.get_shape();
        ((ParticleSystem.ShapeModule) ref shape1).set_shapeType((ParticleSystemShapeType) 5);
        if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
          return;
        ParticleSystem.ShapeModule shape2 = this.RainMistParticleSystem.get_shape();
        ((ParticleSystem.ShapeModule) ref shape2).set_shapeType((ParticleSystemShapeType) 5);
        Vector3 position = ((Component) this.RainFallParticleSystem).get_transform().get_position();
        ref Vector3 local1 = ref position;
        local1.y = (__Null) (local1.y + (double) this.RainMistHeight);
        ref Vector3 local2 = ref position;
        local2.y = (__Null) (local2.y - (double) this.RainHeight);
        ((Component) this.RainMistParticleSystem).get_transform().set_position(position);
      }
    }

    protected override void Start()
    {
      base.Start();
    }

    protected override void Update()
    {
      base.Update();
      this.UpdateRain();
    }
  }
}
