// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.Multiply
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Multiply the Vector3 by a float.")]
  public class Multiply : Action
  {
    [Tooltip("The Vector3 to multiply of")]
    public SharedVector3 vector3Variable;
    [Tooltip("The value to multiply the Vector3 of")]
    public SharedFloat multiplyBy;
    [Tooltip("The multiplication resut")]
    [RequiredField]
    public SharedVector3 storeResult;

    public Multiply()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.op_Multiply(this.vector3Variable.get_Value(), this.multiplyBy.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Variable = this.storeResult = (SharedVector3) Vector3.get_zero();
      this.multiplyBy = (SharedFloat) 0.0f;
    }
  }
}
