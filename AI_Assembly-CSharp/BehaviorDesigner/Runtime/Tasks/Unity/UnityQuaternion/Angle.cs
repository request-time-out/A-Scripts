// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion.Angle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
  [TaskCategory("Unity/Quaternion")]
  [TaskDescription("Stores the angle in degrees between two rotations.")]
  public class Angle : Action
  {
    [Tooltip("The first rotation")]
    public SharedQuaternion firstRotation;
    [Tooltip("The second rotation")]
    public SharedQuaternion secondRotation;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedFloat storeResult;

    public Angle()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Quaternion.Angle(this.firstRotation.get_Value(), this.secondRotation.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.firstRotation = this.secondRotation = (SharedQuaternion) Quaternion.get_identity();
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
