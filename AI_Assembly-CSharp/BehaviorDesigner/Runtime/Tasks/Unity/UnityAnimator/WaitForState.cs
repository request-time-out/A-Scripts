// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.WaitForState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Waits for the Animator to reach the specified state.")]
  public class WaitForState : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the state")]
    public SharedString stateName;
    [Tooltip("The layer where the state is")]
    public SharedInt layer;
    private Animator animator;
    private GameObject prevGameObject;
    private int stateHash;

    public WaitForState()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      this.stateHash = Animator.StringToHash(this.stateName.get_Value());
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.animator = (Animator) defaultGameObject.GetComponent<Animator>();
      this.prevGameObject = defaultGameObject;
      if (this.animator.HasState(this.layer.get_Value(), this.stateHash))
        return;
      Debug.LogError((object) ("Error: The Animator does not have the state " + this.stateName.get_Value() + " on layer " + (object) this.layer.get_Value()));
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
      {
        Debug.LogWarning((object) "Animator is null");
        return (TaskStatus) 1;
      }
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(this.layer.get_Value());
      return ((AnimatorStateInfo) ref animatorStateInfo).get_fullPathHash() == this.stateHash ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.stateName = (SharedString) string.Empty;
      this.layer = (SharedInt) -1;
    }
  }
}
