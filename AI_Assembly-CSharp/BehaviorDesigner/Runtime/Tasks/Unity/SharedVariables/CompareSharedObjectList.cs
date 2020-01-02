// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedObjectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedObjectList : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedObjectList variable;
    [Tooltip("The variable to compare to")]
    public SharedObjectList compareTo;

    public CompareSharedObjectList()
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
        if (Object.op_Inequality(this.variable.get_Value()[index], this.compareTo.get_Value()[index]))
          return (TaskStatus) 1;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedObjectList) null;
      this.compareTo = (SharedObjectList) null;
    }
  }
}
