﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation.SetAnimatePhysics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
  [TaskCategory("Unity/Animation")]
  [TaskDescription("Sets animate physics to the specified value. Returns Success.")]
  public class SetAnimatePhysics : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("Are animations executed in the physics loop?")]
    public SharedBool animatePhysics;
    private Animation animation;
    private GameObject prevGameObject;

    public SetAnimatePhysics()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.animation = (Animation) defaultGameObject.GetComponent<Animation>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.animation, (Object) null))
      {
        Debug.LogWarning((object) "Animation is null");
        return (TaskStatus) 1;
      }
      this.animation.set_animatePhysics(this.animatePhysics.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.animatePhysics.set_Value(false);
    }
  }
}
