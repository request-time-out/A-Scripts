// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.GetIntegerParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Stores the integer parameter on an animator. Returns Success.")]
  public class GetIntegerParameter : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the parameter")]
    public SharedString paramaterName;
    [Tooltip("The value of the integer parameter")]
    [RequiredField]
    public SharedInt storeValue;
    private Animator animator;
    private GameObject prevGameObject;

    public GetIntegerParameter()
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
      this.storeValue.set_Value(this.animator.GetInteger(this.paramaterName.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.paramaterName = (SharedString) string.Empty;
      this.storeValue = (SharedInt) 0;
    }
  }
}
