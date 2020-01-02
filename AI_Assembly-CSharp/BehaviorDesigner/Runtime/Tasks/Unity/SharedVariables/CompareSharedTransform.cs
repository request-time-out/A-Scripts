// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedTransform : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedTransform variable;
    [Tooltip("The variable to compare to")]
    public SharedTransform compareTo;

    public CompareSharedTransform()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.variable.get_Value(), (Object) null) && Object.op_Inequality((Object) this.compareTo.get_Value(), (Object) null))
        return (TaskStatus) 1;
      if (Object.op_Equality((Object) this.variable.get_Value(), (Object) null) && Object.op_Equality((Object) this.compareTo.get_Value(), (Object) null))
        return (TaskStatus) 2;
      return ((object) this.variable.get_Value()).Equals((object) this.compareTo.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedTransform) null;
      this.compareTo = (SharedTransform) null;
    }
  }
}
