// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.FloatClamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Clamps the float between two values.")]
  public class FloatClamp : Action
  {
    [Tooltip("The float to clamp")]
    public SharedFloat floatVariable;
    [Tooltip("The maximum value of the float")]
    public SharedFloat minValue;
    [Tooltip("The maximum value of the float")]
    public SharedFloat maxValue;

    public FloatClamp()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.floatVariable.set_Value(Mathf.Clamp(this.floatVariable.get_Value(), this.minValue.get_Value(), this.maxValue.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.floatVariable = this.minValue = this.maxValue = (SharedFloat) 0.0f;
    }
  }
}
