// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion.Slerp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
  [TaskCategory("Unity/Quaternion")]
  [TaskDescription("Spherically lerp between two quaternions.")]
  public class Slerp : Action
  {
    [Tooltip("The from rotation")]
    public SharedQuaternion fromQuaternion;
    [Tooltip("The to rotation")]
    public SharedQuaternion toQuaternion;
    [Tooltip("The amount to lerp")]
    public SharedFloat amount;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedQuaternion storeResult;

    public Slerp()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Quaternion.Slerp(this.fromQuaternion.get_Value(), this.toQuaternion.get_Value(), this.amount.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.fromQuaternion = this.toQuaternion = this.storeResult = (SharedQuaternion) Quaternion.get_identity();
      this.amount = (SharedFloat) 0.0f;
    }
  }
}
