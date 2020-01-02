// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedObject : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedObject variable;
    [Tooltip("The variable to compare to")]
    public SharedObject compareTo;

    public CompareSharedObject()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality(this.variable.get_Value(), (Object) null) && Object.op_Inequality(this.compareTo.get_Value(), (Object) null))
        return (TaskStatus) 1;
      if (Object.op_Equality(this.variable.get_Value(), (Object) null) && Object.op_Equality(this.compareTo.get_Value(), (Object) null))
        return (TaskStatus) 2;
      return ((object) this.variable.get_Value()).Equals((object) this.compareTo.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedObject) null;
      this.compareTo = (SharedObject) null;
    }
  }
}
