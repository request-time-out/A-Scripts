// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.Operator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Performs a math operation on two Vector3s: Add, Subtract, Multiply, Divide, Min, or Max.")]
  public class Operator : Action
  {
    [Tooltip("The operation to perform")]
    public Operator.Operation operation;
    [Tooltip("The first Vector3")]
    public SharedVector3 firstVector3;
    [Tooltip("The second Vector3")]
    public SharedVector3 secondVector3;
    [Tooltip("The variable to store the result")]
    public SharedVector3 storeResult;

    public Operator()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case Operator.Operation.Add:
          this.storeResult.set_Value(Vector3.op_Addition(this.firstVector3.get_Value(), this.secondVector3.get_Value()));
          break;
        case Operator.Operation.Subtract:
          this.storeResult.set_Value(Vector3.op_Subtraction(this.firstVector3.get_Value(), this.secondVector3.get_Value()));
          break;
        case Operator.Operation.Scale:
          this.storeResult.set_Value(Vector3.Scale(this.firstVector3.get_Value(), this.secondVector3.get_Value()));
          break;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.operation = Operator.Operation.Add;
      this.firstVector3 = this.secondVector3 = this.storeResult = (SharedVector3) Vector3.get_zero();
    }

    public enum Operation
    {
      Add,
      Subtract,
      Scale,
    }
  }
}
