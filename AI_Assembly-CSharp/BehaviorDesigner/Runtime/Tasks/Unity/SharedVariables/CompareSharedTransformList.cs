// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedTransformList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedTransformList : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedTransformList variable;
    [Tooltip("The variable to compare to")]
    public SharedTransformList compareTo;

    public CompareSharedTransformList()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.variable.get_Value() == null && this.compareTo.get_Value() != null)
        return (TaskStatus) 1;
      if (this.variable.get_Value() == null && this.compareTo.get_Value() == null)
        return (TaskStatus) 2;
      if (this.variable.get_Value().Count != this.compareTo.get_Value().Count)
        return (TaskStatus) 1;
      for (int index = 0; index < this.variable.get_Value().Count; ++index)
      {
        if (Object.op_Inequality((Object) this.variable.get_Value()[index], (Object) this.compareTo.get_Value()[index]))
          return (TaskStatus) 1;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedTransformList) null;
      this.compareTo = (SharedTransformList) null;
    }
  }
}
