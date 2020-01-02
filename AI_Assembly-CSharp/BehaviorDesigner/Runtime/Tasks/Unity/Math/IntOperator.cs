// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.IntOperator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Performs a math operation on two integers: Add, Subtract, Multiply, Divide, Min, or Max.")]
  public class IntOperator : Action
  {
    [Tooltip("The operation to perform")]
    public IntOperator.Operation operation;
    [Tooltip("The first integer")]
    public SharedInt integer1;
    [Tooltip("The second integer")]
    public SharedInt integer2;
    [RequiredField]
    [Tooltip("The variable to store the result")]
    public SharedInt storeResult;

    public IntOperator()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case IntOperator.Operation.Add:
          this.storeResult.set_Value(this.integer1.get_Value() + this.integer2.get_Value());
          break;
        case IntOperator.Operation.Subtract:
          this.storeResult.set_Value(this.integer1.get_Value() - this.integer2.get_Value());
          break;
        case IntOperator.Operation.Multiply:
          this.storeResult.set_Value(this.integer1.get_Value() * this.integer2.get_Value());
          break;
        case IntOperator.Operation.Divide:
          this.storeResult.set_Value(this.integer1.get_Value() / this.integer2.get_Value());
          break;
        case IntOperator.Operation.Min:
          this.storeResult.set_Value(Mathf.Min(this.integer1.get_Value(), this.integer2.get_Value()));
          break;
        case IntOperator.Operation.Max:
          this.storeResult.set_Value(Mathf.Max(this.integer1.get_Value(), this.integer2.get_Value()));
          break;
        case IntOperator.Operation.Modulo:
          this.storeResult.set_Value(this.integer1.get_Value() % this.integer2.get_Value());
          break;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.operation = IntOperator.Operation.Add;
      this.integer1.set_Value(0);
      this.integer2.set_Value(0);
      this.storeResult.set_Value(0);
    }

    public enum Operation
    {
      Add,
      Subtract,
      Multiply,
      Divide,
      Min,
      Max,
      Modulo,
    }
  }
}
