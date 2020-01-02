// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.SetBoolParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Sets the bool parameter on an animator. Returns Success.")]
  public class SetBoolParameter : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the parameter")]
    public SharedString paramaterName;
    [Tooltip("The value of the bool parameter")]
    public SharedBool boolValue;
    [Tooltip("Should the value be reverted back to its original value after it has been set?")]
    public bool setOnce;
    private int hashID;
    private Animator animator;
    private GameObject prevGameObject;

    public SetBoolParameter()
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
      this.hashID = Animator.StringToHash(this.paramaterName.get_Value());
      bool origVale = this.animator.GetBool(this.hashID);
      this.animator.SetBool(this.hashID, this.boolValue.get_Value());
      if (this.setOnce)
        ((Task) this).StartCoroutine(this.ResetValue(origVale));
      return (TaskStatus) 2;
    }

    [DebuggerHidden]
    public IEnumerator ResetValue(bool origVale)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SetBoolParameter.\u003CResetValue\u003Ec__Iterator0()
      {
        origVale = origVale,
        \u0024this = this
      };
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.paramaterName = (SharedString) string.Empty;
      this.boolValue = (SharedBool) false;
    }
  }
}
