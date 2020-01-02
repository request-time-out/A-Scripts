// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SetSharedVector2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedVector2 variable to the specified object. Returns Success.")]
  public class SetSharedVector2 : Action
  {
    [Tooltip("The value to set the SharedVector2 to")]
    public SharedVector2 targetValue;
    [RequiredField]
    [Tooltip("The SharedVector2 to set")]
    public SharedVector2 targetVariable;

    public SetSharedVector2()
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
      this.targetValue = (SharedVector2) Vector2.get_zero();
      this.targetVariable = (SharedVector2) Vector2.get_zero();
    }
  }
}
