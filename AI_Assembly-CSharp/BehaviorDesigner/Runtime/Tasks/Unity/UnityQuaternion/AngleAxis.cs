// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion.AngleAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
  [TaskCategory("Unity/Quaternion")]
  [TaskDescription("Stores the rotation which rotates the specified degrees around the specified axis.")]
  public class AngleAxis : Action
  {
    [Tooltip("The number of degrees")]
    public SharedFloat degrees;
    [Tooltip("The axis direction")]
    public SharedVector3 axis;
    [Tooltip("The stored result")]
    [RequiredField]
    public SharedQuaternion storeResult;

    public AngleAxis()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Quaternion.AngleAxis(this.degrees.get_Value(), this.axis.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.degrees = (SharedFloat) 0.0f;
      this.axis = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedQuaternion) Quaternion.get_identity();
    }
  }
}
