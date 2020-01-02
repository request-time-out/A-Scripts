// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
  public class CompareSharedRect : Conditional
  {
    [Tooltip("The first variable to compare")]
    public SharedRect variable;
    [Tooltip("The variable to compare to")]
    public SharedRect compareTo;

    public CompareSharedRect()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      Rect rect = this.variable.get_Value();
      return ((Rect) ref rect).Equals(this.compareTo.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    public virtual void OnReset()
    {
      this.variable = (SharedRect) (Rect) null;
      this.compareTo = (SharedRect) (Rect) null;
    }
  }
}
