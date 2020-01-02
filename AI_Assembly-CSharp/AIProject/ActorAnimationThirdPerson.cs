// Decompiled with JetBrains decompiler
// Type: AIProject.ActorAnimationThirdPerson
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public abstract class ActorAnimationThirdPerson : ActorAnimation
  {
    protected Vector3 _lastForward = Vector3.get_zero();

    public virtual void UpdateState(ActorLocomotion.AnimationState state)
    {
    }

    public override Vector3 GetPivotPoint()
    {
      return ((Component) this).get_transform().get_position();
    }

    protected virtual void OnLateUpdate()
    {
      this.Follow();
    }
  }
}
