// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.FloatComparison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Performs comparison between two floats: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
  public class FloatComparison : Conditional
  {
    [Tooltip("The operation to perform")]
    public FloatComparison.Operation operation;
    [Tooltip("The first float")]
    public SharedFloat float1;
    [Tooltip("The second float")]
    public SharedFloat float2;

    public FloatComparison()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case FloatComparison.Operation.LessThan:
          return (double) this.float1.get_Value() < (double) this.float2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case FloatComparison.Operation.LessThanOrEqualTo:
          return (double) this.float1.get_Value() <= (double) this.float2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case FloatComparison.Operation.EqualTo:
          return Mathf.Approximately(this.float1.get_Value(), this.float2.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
        case FloatComparison.Operation.NotEqualTo:
          return !Mathf.Approximately(this.float1.get_Value(), this.float2.get_Value()) ? (TaskStatus) 2 : (TaskStatus) 1;
        case FloatComparison.Operation.GreaterThanOrEqualTo:
          return (double) this.float1.get_Value() >= (double) this.float2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case FloatComparison.Operation.GreaterThan:
          return (double) this.float1.get_Value() > (double) this.float2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        default:
          return (TaskStatus) 1;
      }
    }

    public virtual void OnReset()
    {
      this.operation = FloatComparison.Operation.LessThan;
      this.float1.set_Value(0.0f);
      this.float2.set_Value(0.0f);
    }

    public enum Operation
    {
      LessThan,
      LessThanOrEqualTo,
      EqualTo,
      NotEqualTo,
      GreaterThanOrEqualTo,
      GreaterThan,
    }
  }
}
