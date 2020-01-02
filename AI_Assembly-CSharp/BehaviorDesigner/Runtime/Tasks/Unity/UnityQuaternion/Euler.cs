// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion.Euler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
  [TaskCategory("Unity/Quaternion")]
  [TaskDescription("Stores the quaternion of a euler vector.")]
  public class Euler : Action
  {
    [Tooltip("The euler vector")]
    public SharedVector3 eulerVector;
    [Tooltip("The stored quaternion")]
    [RequiredField]
    public SharedQuaternion storeResult;

    public Euler()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Quaternion.Euler(this.eulerVector.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.eulerVector = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedQuaternion) Quaternion.get_identity();
    }
  }
}
