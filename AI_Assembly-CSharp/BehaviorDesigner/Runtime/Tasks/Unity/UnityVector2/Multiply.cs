// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2.Multiply
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
  [TaskCategory("Unity/Vector2")]
  [TaskDescription("Multiply the Vector2 by a float.")]
  public class Multiply : Action
  {
    [Tooltip("The Vector2 to multiply of")]
    public SharedVector2 vector2Variable;
    [Tooltip("The value to multiply the Vector2 of")]
    public SharedFloat multiplyBy;
    [Tooltip("The multiplication resut")]
    [RequiredField]
    public SharedVector2 storeResult;

    public Multiply()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector2.op_Multiply(this.vector2Variable.get_Value(), this.multiplyBy.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector2Variable = this.storeResult = (SharedVector2) Vector2.get_zero();
      this.multiplyBy = (SharedFloat) 0.0f;
    }
  }
}
