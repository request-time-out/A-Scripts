// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.RotateTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Rotate the current rotation to the target rotation.")]
  public class RotateTowards : Action
  {
    [Tooltip("The current rotation in euler angles")]
    public SharedVector3 currentRotation;
    [Tooltip("The target rotation in euler angles")]
    public SharedVector3 targetRotation;
    [Tooltip("The maximum delta of the degrees")]
    public SharedFloat maxDegreesDelta;
    [Tooltip("The maximum delta of the magnitude")]
    public SharedFloat maxMagnitudeDelta;
    [Tooltip("The rotation resut")]
    [RequiredField]
    public SharedVector3 storeResult;

    public RotateTowards()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.storeResult.set_Value(Vector3.RotateTowards(this.currentRotation.get_Value(), this.targetRotation.get_Value(), this.maxDegreesDelta.get_Value() * ((float) Math.PI / 180f) * Time.get_deltaTime(), this.maxMagnitudeDelta.get_Value()));
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.currentRotation = this.targetRotation = this.storeResult = (SharedVector3) Vector3.get_zero();
      this.maxDegreesDelta = this.maxMagnitudeDelta = (SharedFloat) 0.0f;
    }
  }
}
