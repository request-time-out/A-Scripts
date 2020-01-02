// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.SetLookAtWeight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Sets the look at weight. Returns success immediately after.")]
  public class SetLookAtWeight : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("(0-1) the global weight of the LookAt, multiplier for other parameters.")]
    public SharedFloat weight;
    [Tooltip("(0-1) determines how much the body is involved in the LookAt.")]
    public float bodyWeight;
    [Tooltip("(0-1) determines how much the head is involved in the LookAt.")]
    public float headWeight;
    [Tooltip("(0-1) determines how much the eyes are involved in the LookAt.")]
    public float eyesWeight;
    [Tooltip("(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).")]
    public float clampWeight;
    private Animator animator;
    private GameObject prevGameObject;
    private bool weightSet;

    public SetLookAtWeight()
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
      this.weightSet = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
      {
        Debug.LogWarning((object) "Animator is null");
        return (TaskStatus) 1;
      }
      return this.weightSet ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnAnimatorIK()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
        return;
      this.animator.SetLookAtWeight(this.weight.get_Value(), this.bodyWeight, this.headWeight, this.eyesWeight, this.clampWeight);
      this.weightSet = true;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.weight = (SharedFloat) 0.0f;
      this.bodyWeight = 0.0f;
      this.headWeight = 1f;
      this.eyesWeight = 0.0f;
      this.clampWeight = 0.5f;
    }
  }
}
