// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedVector4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedVector4 : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedVector4 variable;
    [Tooltip("The variable to compare to")]
    public SharedVector4 compareTo;

    public CompareSharedVector4()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Vector4 vector4 = this.variable.get_Value();
      return ((Vector4) ref vector4).Equals(this.compareTo.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedVector4) Vector4.get_zero();
      this.compareTo = (SharedVector4) Vector4.get_zero();
    }
  }
}
