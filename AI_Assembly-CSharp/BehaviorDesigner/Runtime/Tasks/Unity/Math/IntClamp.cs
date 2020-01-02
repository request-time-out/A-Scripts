// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.IntClamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Clamps the int between two values.")]
  public class IntClamp : Action
  {
    [Tooltip("The int to clamp")]
    public SharedInt intVariable;
    [Tooltip("The maximum value of the int")]
    public SharedInt minValue;
    [Tooltip("The maximum value of the int")]
    public SharedInt maxValue;

    public IntClamp()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.intVariable.set_Value(Mathf.Clamp(this.intVariable.get_Value(), this.minValue.get_Value(), this.maxValue.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.intVariable = this.minValue = this.maxValue = (SharedInt) 0;
    }
  }
}
