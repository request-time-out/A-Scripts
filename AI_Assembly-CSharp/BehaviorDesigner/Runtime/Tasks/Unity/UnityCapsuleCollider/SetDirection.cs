﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityCapsuleCollider.SetDirection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCapsuleCollider
{
  [TaskCategory("Unity/CapsuleCollider")]
  [TaskDescription("Sets the direction of the CapsuleCollider. Returns Success.")]
  public class SetDirection : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The direction of the CapsuleCollider")]
    public SharedInt direction;
    private CapsuleCollider capsuleCollider;
    private GameObject prevGameObject;

    public SetDirection()
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
      this.capsuleCollider.set_direction(this.direction.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.direction = (SharedInt) 0;
    }
  }
}
