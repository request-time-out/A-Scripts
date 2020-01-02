﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3.GetSqrMagnitude
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
  [TaskCategory("Unity/Vector3")]
  [TaskDescription("Stores the square magnitude of the Vector3.")]
  public class GetSqrMagnitude : Action
  {
    [Tooltip("The Vector3 to get the square magnitude of")]
    public SharedVector3 vector3Variable;
    [Tooltip("The square magnitude of the vector")]
    [RequiredField]
    public SharedFloat storeResult;

    public GetSqrMagnitude()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      SharedFloat storeResult = this.storeResult;
      Vector3 vector3 = this.vector3Variable.get_Value();
      double sqrMagnitude = (double) ((Vector3) ref vector3).get_sqrMagnitude();
      storeResult.set_Value((float) sqrMagnitude);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.vector3Variable = (SharedVector3) Vector3.get_zero();
      this.storeResult = (SharedFloat) 0.0f;
    }
  }
}
