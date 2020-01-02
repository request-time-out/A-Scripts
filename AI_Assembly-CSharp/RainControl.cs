// Decompiled with JetBrains decompiler
// Type: RainControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using DigitalRuby.RainMaker;
using System;
using UniRx;
using UnityEngine;

public class RainControl : BaseRainScript
{
  [SerializeField]
  private Vector3 _rainOffset = new Vector3(0.0f, 25f, -7f);
  [SerializeField]
  private Vector3 _mistOffset = new Vector3(0.0f, 3f, 0.0f);

  public Vector3 RainOffset
  {
    get
    {
      return this._rainOffset;
    }
  }

  public Vector3 MistOffset
  {
    get
    {
      return this._mistOffset;
    }
  }

  protected override void Start()
  {
    base.Start();
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
  }

  private void OnUpdate()
  {
    base.Update();
    this.UpdateRain();
  }

  protected override void Update()
  {
  }

  private void UpdateRain()
  {
    if (this.FollowCamera)
    {
      if (Object.op_Inequality((Object) this.RainFallParticleSystem, (Object) null))
      {
        ParticleSystem.ShapeModule shape = this.RainFallParticleSystem.get_shape();
        ((ParticleSystem.ShapeModule) ref shape).set_shapeType((ParticleSystemShapeType) 8);
        ((Component) this.RainFallParticleSystem).get_transform().set_position(((Component) this.Camera).get_transform().get_position());
        ((Component) this.RainFallParticleSystem).get_transform().Translate(this._rainOffset);
        ((Component) this.RainFallParticleSystem).get_transform().set_rotation(Quaternion.Euler(0.0f, (float) ((Component) this.Camera).get_transform().get_eulerAngles().y, 0.0f));
      }
      if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
        return;
      ParticleSystem.ShapeModule shape1 = this.RainMistParticleSystem.get_shape();
      ((ParticleSystem.ShapeModule) ref shape1).set_shapeType((ParticleSystemShapeType) 2);
      Vector3 position = ((Component) this.Camera).get_transform().get_position();
      ref Vector3 local = ref position;
      local.y = local.y + this._mistOffset.y;
      ((Component) this.RainMistParticleSystem).get_transform().set_position(position);
    }
    else
    {
      if (Object.op_Inequality((Object) this.RainFallParticleSystem, (Object) null))
      {
        ParticleSystem.ShapeModule shape = this.RainFallParticleSystem.get_shape();
        ((ParticleSystem.ShapeModule) ref shape).set_shapeType((ParticleSystemShapeType) 5);
      }
      if (!Object.op_Inequality((Object) this.RainMistParticleSystem, (Object) null))
        return;
      ParticleSystem.ShapeModule shape1 = this.RainMistParticleSystem.get_shape();
      ((ParticleSystem.ShapeModule) ref shape1).set_shapeType((ParticleSystemShapeType) 5);
      Vector3 vector3 = Vector3.op_Addition(((Component) this.RainFallParticleSystem).get_transform().get_position(), this._mistOffset);
      ref Vector3 local = ref vector3;
      local.y = local.y - this._rainOffset.y;
      ((Component) this.RainMistParticleSystem).get_transform().set_position(vector3);
    }
  }
}
