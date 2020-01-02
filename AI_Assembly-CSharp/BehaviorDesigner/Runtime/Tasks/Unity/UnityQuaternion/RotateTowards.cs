// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion.RotateTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
  [TaskCategory("Unity/Quaternion")]
  [TaskDescription("Stores the quaternion after a rotation.")]
  public class RotateTowards : Action
  {
    [Tooltip("The from rotation")]
    public SharedQuaternion fromQuaternion;
    [Tooltip("The to rotation")]
    public SharedQuaternion toQuaternion;
    [Tooltip("The maximum degrees delta")]
    public SharedFloat maxDeltaDegrees;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedQuaternion storeResult;

    public RotateTowards()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Quaternion.RotateTowards(this.fromQuaternion.get_Value(), this.toQuaternion.get_Value(), this.maxDeltaDegrees.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.fromQuaternion = this.toQuaternion = this.storeResult = (SharedQuaternion) Quaternion.get_identity();
      this.maxDeltaDegrees = (SharedFloat) 0.0f;
    }
  }
}
