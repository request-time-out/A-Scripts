// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.MatchTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress. Returns Success.")]
  public class MatchTarget : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The position we want the body part to reach")]
    public SharedVector3 matchPosition;
    [Tooltip("The rotation in which we want the body part to be")]
    public SharedQuaternion matchRotation;
    [Tooltip("The body part that is involved in the match")]
    public AvatarTarget targetBodyPart;
    [Tooltip("Weights for matching position")]
    public Vector3 weightMaskPosition;
    [Tooltip("Weights for matching rotation")]
    public float weightMaskRotation;
    [Tooltip("Start time within the animation clip")]
    public float startNormalizedTime;
    [Tooltip("End time within the animation clip")]
    public float targetNormalizedTime;
    private Animator animator;
    private GameObject prevGameObject;

    public MatchTarget()
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
      this.animator.MatchTarget(this.matchPosition.get_Value(), this.matchRotation.get_Value(), this.targetBodyPart, new MatchTargetWeightMask(this.weightMaskPosition, this.weightMaskRotation), this.startNormalizedTime, this.targetNormalizedTime);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.matchPosition = (SharedVector3) Vector3.get_zero();
      this.matchRotation = (SharedQuaternion) Quaternion.get_identity();
      this.targetBodyPart = (AvatarTarget) 0;
      this.weightMaskPosition = Vector3.get_zero();
      this.weightMaskRotation = 0.0f;
      this.startNormalizedTime = 0.0f;
      this.targetNormalizedTime = 1f;
    }
  }
}
