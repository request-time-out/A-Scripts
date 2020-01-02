﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.CrossFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Creates a dynamic transition between the current state and the destination state. Returns Success.")]
  public class CrossFade : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the state")]
    public SharedString stateName;
    [Tooltip("The duration of the transition. Value is in source state normalized time")]
    public SharedFloat transitionDuration;
    [Tooltip("The layer where the state is")]
    public int layer;
    [Tooltip("The normalized time at which the state will play")]
    public float normalizedTime;
    private Animator animator;
    private GameObject prevGameObject;

    public CrossFade()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.animator = (Animator) defaultGameObject.GetComponent<Animator>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
      {
        Debug.LogWarning((object) "Animator is null");
        return (TaskStatus) 1;
      }
      this.animator.CrossFade(this.stateName.get_Value(), this.transitionDuration.get_Value(), this.layer, this.normalizedTime);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.stateName = (SharedString) string.Empty;
      this.transitionDuration = (SharedFloat) 0.0f;
      this.layer = -1;
      this.normalizedTime = float.NegativeInfinity;
    }
  }
}
