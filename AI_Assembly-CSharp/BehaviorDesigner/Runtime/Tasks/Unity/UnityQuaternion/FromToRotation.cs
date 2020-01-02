// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion.FromToRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
  [TaskCategory("Unity/Quaternion")]
  [TaskDescription("Stores a rotation which rotates from the first direction to the second.")]
  public class FromToRotation : Action
  {
    [Tooltip("The from rotation")]
    public SharedVector3 fromDirection;
    [Tooltip("The to rotation")]
    public SharedVector3 toDirection;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedQuaternion storeResult;

    public FromToRotation()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Quaternion.FromToRotation(this.fromDirection.get_Value(), this.toDirection.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.fromDirection = this.toDirection = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedQuaternion) Quaternion.get_identity();
    }
  }
}
