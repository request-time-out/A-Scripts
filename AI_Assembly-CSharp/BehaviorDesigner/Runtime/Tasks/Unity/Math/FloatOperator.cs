// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.FloatOperator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Performs a math operation on two floats: Add, Subtract, Multiply, Divide, Min, or Max.")]
  public class FloatOperator : Action
  {
    [Tooltip("The operation to perform")]
    public FloatOperator.Operation operation;
    [Tooltip("The first float")]
    public SharedFloat float1;
    [Tooltip("The second float")]
    public SharedFloat float2;
    [Tooltip("The variable to store the result")]
    public SharedFloat storeResult;

    public FloatOperator()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case FloatOperator.Operation.Add:
          this.storeResult.set_Value(this.float1.get_Value() + this.float2.get_Value());
          break;
        case FloatOperator.Operation.Subtract:
          this.storeResult.set_Value(this.float1.get_Value() - this.float2.get_Value());
          break;
        case FloatOperator.Operation.Multiply:
          this.storeResult.set_Value(this.float1.get_Value() * this.float2.get_Value());
          break;
        case FloatOperator.Operation.Divide:
          this.storeResult.set_Value(this.float1.get_Value() / this.float2.get_Value());
          break;
        case FloatOperator.Operation.Min:
          this.storeResult.set_Value(Mathf.Min(this.float1.get_Value(), this.float2.get_Value()));
          break;
        case FloatOperator.Operation.Max:
          this.storeResult.set_Value(Mathf.Max(this.float1.get_Value(), this.float2.get_Value()));
          break;
        case FloatOperator.Operation.Modulo:
          this.storeResult.set_Value(this.float1.get_Value() % this.float2.get_Value());
          break;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.operation = FloatOperator.Operation.Add;
      this.float1.set_Value(0.0f);
      this.float2.set_Value(0.0f);
      this.storeResult.set_Value(0.0f);
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
