// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedTransform variable to the specified object. Returns Success.")]
  public class SetSharedTransform : Action
  {
    [Tooltip("The value to set the SharedTransform to. If null the variable will be set to the current Transform")]
    public SharedTransform targetValue;
    [RequiredField]
    [Tooltip("The SharedTransform to set")]
    public SharedTransform targetVariable;

    public SetSharedTransform()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.targetVariable.set_Value(!Object.op_Inequality((Object) this.targetValue.get_Value(), (Object) null) ? (Transform) ((Task) this).transform : this.targetValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetValue = (SharedTransform) null;
      this.targetVariable = (SharedTransform) null;
    }
  }
}
