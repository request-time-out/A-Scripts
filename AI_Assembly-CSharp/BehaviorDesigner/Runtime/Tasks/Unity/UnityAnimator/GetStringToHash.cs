// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator.GetStringToHash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
  [TaskCategory("Unity/Animator")]
  [TaskDescription("Converts the state name to its corresponding hash code. Returns Success.")]
  public class GetStringToHash : Action
  {
    [Tooltip("The name of the state to convert to a hash code")]
    public SharedString stateName;
    [Tooltip("The hash value")]
    [RequiredField]
    public SharedInt storeValue;

    public GetStringToHash()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeValue.set_Value(Animator.StringToHash(this.stateName.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.stateName = (SharedString) string.Empty;
      this.storeValue = (SharedInt) 0;
    }
  }
}
