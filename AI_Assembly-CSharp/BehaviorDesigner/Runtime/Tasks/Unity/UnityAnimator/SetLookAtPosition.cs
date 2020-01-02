// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.SetLookAtPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Sets the look at position. Returns Success.")]
  public class SetLookAtPosition : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The position to lookAt")]
    public SharedVector3 position;
    private Animator animator;
    private GameObject prevGameObject;
    private bool positionSet;

    public SetLookAtPosition()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
      {
        this.animator = (Animator) defaultGameObject.GetComponent<Animator>();
        this.prevGameObject = defaultGameObject;
      }
      this.positionSet = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
      {
        Debug.LogWarning((object) "Animator is null");
        return (TaskStatus) 1;
      }
      return this.positionSet ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnAnimatorIK()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
        return;
      this.animator.SetLookAtPosition(this.position.get_Value());
      this.positionSet = true;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.position = (SharedVector3) Vector3.get_zero();
    }
  }
}
