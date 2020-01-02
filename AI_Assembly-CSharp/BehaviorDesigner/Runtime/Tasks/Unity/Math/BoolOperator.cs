// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.Math.BoolOperator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
  [TaskCategory("Unity/Math")]
  [TaskDescription("Performs a math operation on two bools: AND, OR, NAND, or XOR.")]
  public class BoolOperator : Action
  {
    [Tooltip("The operation to perform")]
    public BoolOperator.Operation operation;
    [Tooltip("The first bool")]
    public SharedBool bool1;
    [Tooltip("The second bool")]
    public SharedBool bool2;
    [Tooltip("The variable to store the result")]
    public SharedBool storeResult;

    public BoolOperator()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      switch (this.operation)
      {
        case BoolOperator.Operation.AND:
          this.storeResult.set_Value(this.bool1.get_Value() && this.bool2.get_Value());
          break;
        case BoolOperator.Operation.OR:
          this.storeResult.set_Value(this.bool1.get_Value() || this.bool2.get_Value());
          break;
        case BoolOperator.Operation.NAND:
          this.storeResult.set_Value((!this.bool1.get_Value() ? 0 : (this.bool2.get_Value() ? 1 : 0)) == 0);
          break;
        case BoolOperator.Operation.XOR:
          this.storeResult.set_Value(this.bool1.get_Value() ^ this.bool2.get_Value());
          break;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.operation = BoolOperator.Operation.AND;
      this.bool1.set_Value(false);
      this.bool2.set_Value(false);
      this.storeResult.set_Value(false);
    }

    public enum Operation
    {
      AND,
      OR,
      NAND,
      XOR,
    }
  }
}
