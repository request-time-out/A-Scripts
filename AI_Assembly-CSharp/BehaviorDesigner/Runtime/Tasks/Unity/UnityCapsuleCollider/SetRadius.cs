﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityCapsuleCollider.SetRadius
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCapsuleCollider
{
  [TaskCategory("Unity/CapsuleCollider")]
  [TaskDescription("Sets the radius of the CapsuleCollider. Returns Success.")]
  public class SetRadius : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The radius of the CapsuleCollider")]
    public SharedFloat radius;
    private CapsuleCollider capsuleCollider;
    private GameObject prevGameObject;

    public SetRadius()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.capsuleCollider = (CapsuleCollider) defaultGameObject.GetComponent<CapsuleCollider>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.capsuleCollider, (Object) null))
      {
        Debug.LogWarning((object) "CapsuleCollider is null");
        return (TaskStatus) 1;
      }
      this.capsuleCollider.set_radius(this.radius.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.radius = (SharedFloat) 0.0f;
    }
  }
}
