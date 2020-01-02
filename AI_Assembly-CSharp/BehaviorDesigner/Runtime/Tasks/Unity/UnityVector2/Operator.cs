// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.Operator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Performs a math operation on two Vector2s: Add, Subtract, Multiply, Divide, Min, or Max.")]
  public class Operator : Action
  {
    [Tooltip("The operation to perform")]
    public Operator.Operation operation;
    [Tooltip("The first Vector2")]
    public SharedVector2 firstVector2;
    [Tooltip("The second Vector2")]
    public SharedVector2 secondVector2;
    [Tooltip("The variable to store the result")]
    public SharedVector2 storeResult;

    public Operator()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case Operator.Operation.Add:
          this.storeResult.set_Value(Vector2.op_Addition(this.firstVector2.get_Value(), this.secondVector2.get_Value()));
          break;
        case Operator.Operation.Subtract:
          this.storeResult.set_Value(Vector2.op_Subtraction(this.firstVector2.get_Value(), this.secondVector2.get_Value()));
          break;
        case Operator.Operation.Scale:
          this.storeResult.set_Value(Vector2.Scale(this.firstVector2.get_Value(), this.secondVector2.get_Value()));
          break;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.operation = Operator.Operation.Add;
      this.firstVector2 = this.secondVector2 = this.storeResult = (SharedVector2) Vector2.get_zero();
    }

    public enum Operation
    {
      Add,
      Subtract,
      Scale,
    }
  }
}
