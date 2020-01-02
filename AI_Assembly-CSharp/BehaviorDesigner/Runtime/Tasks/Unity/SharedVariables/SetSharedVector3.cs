// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedVector3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedVector3 variable to the specified object. Returns Success.")]
  public class SetSharedVector3 : Action
  {
    [Tooltip("The value to set the SharedVector3 to")]
    public SharedVector3 targetValue;
    [RequiredField]
    [Tooltip("The SharedVector3 to set")]
    public SharedVector3 targetVariable;

    public SetSharedVector3()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.targetVariable.set_Value(this.targetValue.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetValue = (SharedVector3) Vector3.get_zero();
      this.targetVariable = (SharedVector3) Vector3.get_zero();
    }
  }
}
