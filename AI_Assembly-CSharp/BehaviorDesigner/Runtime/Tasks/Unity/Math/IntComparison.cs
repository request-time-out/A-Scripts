// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.IntComparison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Performs comparison between two integers: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
  public class IntComparison : Conditional
  {
    [Tooltip("The operation to perform")]
    public IntComparison.Operation operation;
    [Tooltip("The first integer")]
    public SharedInt integer1;
    [Tooltip("The second integer")]
    public SharedInt integer2;

    public IntComparison()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case IntComparison.Operation.LessThan:
          return this.integer1.get_Value() < this.integer2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case IntComparison.Operation.LessThanOrEqualTo:
          return this.integer1.get_Value() <= this.integer2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case IntComparison.Operation.EqualTo:
          return this.integer1.get_Value() == this.integer2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case IntComparison.Operation.NotEqualTo:
          return this.integer1.get_Value() != this.integer2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case IntComparison.Operation.GreaterThanOrEqualTo:
          return this.integer1.get_Value() >= this.integer2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        case IntComparison.Operation.GreaterThan:
          return this.integer1.get_Value() > this.integer2.get_Value() ? (TaskStatus) 2 : (TaskStatus) 1;
        default:
          return (TaskStatus) 1;
      }
    }

    public virtual void OnReset()
    {
      this.operation = IntComparison.Operation.LessThan;
      this.integer1.set_Value(0);
      this.integer2.set_Value(0);
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
